using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core
{
    public class Response
    {
        public Response.Type ResponseType { get; }
        public string Body { get; }

        private Response(Response.Type type, string body)
        {
            this.ResponseType = type;
            this.Body = body;
        }

        public enum Type : uint
        {
            Unknown         =0x000_0000_0,

            Executed        =0x001_0001_0,

            NotFound        =0x002_0001_0,
        }

        public class Builder
        {
            private Response response;

            public Builder(Response.Type responseType, string body)
            {
                this.response = new Response(responseType, body);
            }

            public Response Build() => response;
        }
    }
}
