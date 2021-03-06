﻿using System;
using System.Threading.Tasks;
using System.Linq;

using Discord;
using Discord.WebSocket;

using ArnoBot.Core;
using ArnoBot.Core.Responses;
using ArnoBot.Interface;

using ArnoBot.DiscordBot.Interface;

namespace ArnoBot.FrontEnd.DiscordBot
{
    public class MessageHandler
    {
        private Bot bot;
        private DiscordSocketClient client;
        private readonly string[] prefixes;
        private readonly bool listenToMentions;
        private MessageHandlerModule module = new MessageHandlerModule();

        private Response noNSFWChannelResponse = new TextResponse(Response.Type.NotFound, "You can't use NSFW commands here.");

        public MessageHandler(DiscordSocketClient client, Bot bot, string[] prefixes, bool listenToMentions)
        {
            this.bot = bot;
            this.client = client;
            this.prefixes = prefixes;
            this.listenToMentions = listenToMentions;
            client.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            TriggerInfo triggerInfo;
            if (IsDirectedAtBot(message, out triggerInfo))
                bot.QueryAsync(
                    SanitizeMessageContent(message, triggerInfo),
                    (cmd, ctx) => ExecuteCommand(cmd, ctx, message),
                    (response) => OnQueryResultCallback(message.Author, message.Channel, response)
                    );
            else
                await Task.CompletedTask;
        }

        private Response ExecuteCommand(ICommand command, CommandContext context, SocketMessage socketMessage)
        {
            if (command is IDiscordCommand)
            {
                if ((command as IDiscordCommand).IsNSFW && !IsNSFWChannel(socketMessage.Channel))
                    return noNSFWChannelResponse;

                DiscordCommandContext discordCommandContext = new DiscordCommandContext(
                    context,
                    socketMessage,
                    socketMessage.Channel,
                    socketMessage.Author,
                    client.CurrentUser
                    );
                return (command as IDiscordCommand).Execute(discordCommandContext);
            }
            else
                return command.Execute(context);
        }

        private bool IsNSFWChannel(ISocketMessageChannel channel)
        {
            if (channel is ITextChannel)
                return (channel as ITextChannel).IsNsfw;
            else
                return true;
        }

        private void OnQueryResultCallback(SocketUser user, ISocketMessageChannel channel, Response response)
            => Task.Run(() => ParseResponseIntoDiscordMessage(user, channel, response));

        private async Task ParseResponseIntoDiscordMessage(SocketUser user, ISocketMessageChannel channel, Response response)
        {
            try
            {
                EmbedBuilder builder = new EmbedBuilder()
                    .WithColor(GetColorFromResponseType(response.ResponseType))
                    .WithAuthor(
                    new EmbedAuthorBuilder()
                        .WithName(user.Username)
                        .WithIconUrl(user.GetAvatarUrl(size: 64))
                    );
                SetEmbedContentFromResponse(builder, response);

                if (response is FileResponse && (response as FileResponse).Body.IsAttachment)
                    await channel.SendFileAsync((response as FileResponse).Body.ImageURL, embed: builder.Build());
                else
                    await channel.SendMessageAsync(embed: builder.Build());
            }
            catch(Exception ex)
            {
                Logger.LogError(module, ex);
            }
        }

        private void SetEmbedContentFromResponse(EmbedBuilder builder, Response response)
        {
            if (response is TextResponse)
                SetEmbedContentFromResponse(builder, response as TextResponse);
            else if (response is ExtendedResponse)
                SetEmbedContentFromResponse(builder, response as ExtendedResponse);
            else if (response is ErrorResponse)
                SetEmbedContentFromResponse(builder, response as ErrorResponse);
            else if (response is FileResponse)
                SetEmbedContentFromResponse(builder, response as FileResponse);
        }

        private void SetEmbedContentFromResponse(EmbedBuilder builder, TextResponse textResponse)
        {
            builder.WithDescription(textResponse.Body);
        }

        private void SetEmbedContentFromResponse(EmbedBuilder builder, ExtendedResponse extendedResponse)
        {
            builder.WithTitle(extendedResponse.Body.Title)
                .WithFooter(extendedResponse.Body.Footer)
                .WithFields(extendedResponse.Body.Paragraphs
                    .Where((paragraph) => !paragraph.Body.Equals(string.Empty))
                    .Select((paragraph) => {
                        return new EmbedFieldBuilder()
                            .WithName(paragraph.Title)
                            .WithValue(paragraph.Body)
                            .WithIsInline(true);
                }));
        }

        private void SetEmbedContentFromResponse(EmbedBuilder builder, ErrorResponse errorResponse)
        {
            if (errorResponse.Exception is ArnoBotException)
            {
                builder.WithTitle((errorResponse.Exception as ArnoBotException).SimpleName)
                    .WithDescription(errorResponse.Exception.Message);
            }
            else
            {
                Logger.LogError(module, errorResponse.Exception);
                builder.WithTitle("An error occured!")
                    .WithDescription("Oops! Please try again later!");
            }
        }

        private void SetEmbedContentFromResponse(EmbedBuilder builder, FileResponse fileResponse)
        {
            builder.WithDescription(fileResponse.Body.Text)
                .WithImageUrl((fileResponse.Body.IsAttachment ? "attachment://" : "") + fileResponse.Body.ImageFileName);
        }

        private Color GetColorFromResponseType(Response.Type responseType)
        {
            switch(responseType)
            {
                case Response.Type.Executed:
                    return new Color(0.0f, 1.0f, 0.0f);
                case Response.Type.Error:
                    return new Color(1.0f, 0.0f, 0.0f);
                case Response.Type.NotFound:
                    return new Color(0.7f, 0.5f, 0.0f);
                default:
                    return new Color(0.25f, 0.25f, 0.25f);
            }
        }

        private bool IsDirectedAtBot(SocketMessage message, out TriggerInfo triggerInfo)
        {
            string selectedPrefix = null;
            if(prefixes.Any((prefix) => message.Content.StartsWith(selectedPrefix = prefix)))
            {
                triggerInfo = new TriggerInfo(selectedPrefix, false);
                string prefixCutMessage = message.Content.Remove(0, selectedPrefix.Length);

                // Bot should ignore messages that start with a prefix followed by non-alphanumeric characters.
                return prefixCutMessage.Length > 0 && char.IsLetterOrDigit(prefixCutMessage[0]);
            }
            else if(listenToMentions && MessageStartsWithBotMention(message))
            {
                triggerInfo = new TriggerInfo($"<@!{client.CurrentUser.Id}>", true);
                return true;
            }
            else
            {
                triggerInfo = null;
                return false;
            }
        }

        private bool MessageStartsWithBotMention(SocketMessage message)
        {
            return message.Content.StartsWith($"<@!{client.CurrentUser.Id}>")
                && message.MentionedUsers.First().Id.Equals(client.CurrentUser.Id);
        }

        private string SanitizeMessageContent(SocketMessage message, TriggerInfo triggerInfo)
            => SanitizeMessageContent(message.Content, triggerInfo);

        private string SanitizeMessageContent(string messageContent, TriggerInfo triggerInfo)
            => messageContent.Remove(0, triggerInfo.TriggerString.Length).Trim();

        private class MessageHandlerModule : IModule
        {
            public string Name => "MessageHandler";
            public IReadOnlyCommandRegistry CommandRegistry => throw new ArgumentException("MessageHandlerModule is not a Command module!");
        }

        private class TriggerInfo
        {
            public string TriggerString { get; }
            public bool IsMention { get; }

            public TriggerInfo(string triggerString, bool isMention)
            {
                this.TriggerString = triggerString;
                this.IsMention = isMention;
            }
        }
    }
}
