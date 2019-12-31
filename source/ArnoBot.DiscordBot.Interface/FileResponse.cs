using System;
using System.IO;

using ArnoBot.Core.Responses;

namespace ArnoBot.DiscordBot.Interface
{
    public class FileResponse : Response<FileResponse.Content>
    {
        public FileResponse(Type type, string text, Uri imageUrl) : base(type, new Content(text, imageUrl.AbsoluteUri, "img", false))
        {
        }

        public FileResponse(Type type, string text, FileInfo imageFile) : base(type, new Content(text, imageFile.FullName, imageFile.Name, true))
        {
        }

        public class Content
        {
            public string Text { get; }
            public string ImageURL { get; }
            public string ImageFileName { get; }
            public bool IsAttachment { get; }

            internal Content(string text, string imageUrl, string imageFileName, bool isAttachment)
            {
                this.Text = text;
                this.ImageURL = imageUrl;
                this.ImageFileName = imageFileName;
                this.IsAttachment = isAttachment;
            }
        }
    }
}
