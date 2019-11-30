using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core.Responses
{
    public abstract class Response<T> : Response
    {
        public Response.Type ResponseType { get; }
        public T Body { get; }

        protected Response(Response.Type type, T body)
        {
            this.ResponseType = type;
            this.Body = body;
        }
    }

    public abstract class Response
    {
        public string TextBody { get => ((TextResponse)this).Body; }
        public Exception Exception { get => ((ErrorResponse)this).Body; }

        public enum Type : uint
        {
            Unknown = 0x000_0000_0,

            Executed = 0x001_0001_0,

            NotFound = 0x002_0001_0,
            Error = 0x002_0002_0,
        }
    }
}
