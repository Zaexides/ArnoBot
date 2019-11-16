using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core
{
    public class Response
    {
        public Response.Type ResponseType { get; }

        public Response(Response.Type type)
        {
            this.ResponseType = type;
        }

        public enum Type : uint
        {
            Unknown         =0x000_0000_0,

            Executed        =0x001_0001_0,
        }
    }
}
