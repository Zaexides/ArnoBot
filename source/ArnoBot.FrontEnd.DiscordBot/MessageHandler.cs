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
        private readonly string prefix;
        private MessageHandlerModule module = new MessageHandlerModule();

        private Response noNSFWChannelResponse = new TextResponse(Response.Type.NotFound, "You can't use NSFW commands here.");

        public MessageHandler(DiscordSocketClient client, Bot bot, string prefix)
        {
            this.bot = bot;
            this.client = client;
            this.prefix = prefix;
            client.MessageReceived += OnMessageReceived;
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            if (IsDirectedAtBot(message))
                bot.QueryAsync(
                    SanitizeMessageContent(message),
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
            Logger.LogError(module, errorResponse.Exception);
            builder.WithTitle("An error occured!")
                .WithDescription("Oops! Please try again later!");
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

        private bool IsDirectedAtBot(SocketMessage message)
        {
            return
                message.Content.StartsWith(prefix)
                || message.MentionedUsers.First().Id.Equals(client.CurrentUser.Id);
        }

        private string SanitizeMessageContent(SocketMessage message)
            => SanitizeMessageContent(message.Content);

        private string SanitizeMessageContent(string messageContent)
            => messageContent.Remove(0, prefix.Length).Trim();

        private class MessageHandlerModule : IModule
        {
            public string Name => "MessageHandler";
            public IReadOnlyCommandRegistry CommandRegistry => throw new ArgumentException("MessageHandlerModule is not a Command module!");
        }
    }
}
