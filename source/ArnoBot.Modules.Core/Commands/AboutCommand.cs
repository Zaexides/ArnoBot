using System;
using System.Collections.Generic;
using System.Reflection;

using ArnoBot.Interface;
using ArnoBot.Core.Responses;
using ArnoBot.Core;

namespace ArnoBot.Modules.Core.Commands
{
    public class AboutCommand : ICommand
    {
        public Response Execute(CommandContext context)
        {
            return new TextResponse(Response.Type.Executed, "ArnoBot version " + Assembly.GetEntryAssembly().GetName().Version.ToString());
        }
    }
}
