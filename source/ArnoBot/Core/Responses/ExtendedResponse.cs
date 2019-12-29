using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core.Responses
{
    public class ExtendedResponse : Response<ExtendedResponse.Content>
    {
        public ExtendedResponse(Response.Type type, Content content) : base(type, content)
        {
        }

        public class Content
        {
            public string Title { get; }
            public string Footer { get; }
            public Paragraph[] Paragraphs { get; }

            private Content(string title, string footer, params Paragraph[] paragraphs)
            {
                this.Title = title;
                this.Footer = footer;
                this.Paragraphs = paragraphs;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("--").Append(Title).AppendLine("--");
                foreach(Paragraph p in Paragraphs)
                {
                    sb.AppendLine(p.Title);
                    sb.Append("\t").AppendLine(p.Body);
                }
                sb.AppendLine("");
                sb.AppendLine(Footer);

                return sb.ToString();
            }

            public class Paragraph
            {
                public string Title { get; }
                public string Body { get; }

                internal Paragraph(string title, string body)
                {
                    this.Title = title;
                    this.Body = body;
                }
            }

            public class Builder
            {
                private string title, footer;
                private List<Paragraph> paragraphs;

                public Builder()
                {
                    this.paragraphs = new List<Paragraph>();
                }

                public Builder SetTitle(string title)
                {
                    this.title = title;
                    return this;
                }

                public Builder SetFooter(string footer)
                {
                    this.footer = footer;
                    return this;
                }

                public Builder AddParagraph(string title, string body)
                {
                    this.paragraphs.Add(new Paragraph(title, body));
                    return this;
                }

                public Content Build()
                {
                    return new Content(this.title, this.footer, this.paragraphs.ToArray());
                }
            }
        }
    }
}
