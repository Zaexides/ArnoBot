using System;

using ArnoBot.Core.Responses;

namespace ArnoBot.DiscordBot.Interface
{
    public class FileResponse : Response<FileResponse.Content>
    {
        public FileResponse(Type type, string text, string imageUrl) : base(type, new Content(text, imageUrl))
        {
        }

        public class Content
        {
            public string Text { get; }
            public string ImageURL { get; }

            internal Content(string text, string imageUrl)
            {
                this.Text = text;
                this.ImageURL = imageUrl;
            }
        }
    }
}
