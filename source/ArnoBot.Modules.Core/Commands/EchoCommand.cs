using System;
using System.Collections.Generic;
using System.Text;

using ArnoBot.Interface;
using ArnoBot.Core.Responses;
using ArnoBot.Core;

namespace ArnoBot.Modules.Core.Commands
{
    public class EchoCommand : ICommand
    {
        public Response Execute(CommandContext context)
        {
            return new TextResponse(Response.Type.Executed, string.Join(" ", context.Parameters));
        }
    }
}
