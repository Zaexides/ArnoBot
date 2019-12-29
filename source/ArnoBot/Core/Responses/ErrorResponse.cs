using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core.Responses
{
    public class ErrorResponse : Response<Exception>
    {
        public ErrorResponse(Response.Type type, Exception body) : base(type, body)
        { }
    }
}
