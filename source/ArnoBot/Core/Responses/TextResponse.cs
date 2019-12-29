using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core.Responses
{
    public class TextResponse : Response<string>
    {
        public TextResponse(Response.Type type, string body) : base(type, body)
        { }
    }
}
