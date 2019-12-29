using System;
using System.Collections.Generic;
using System.Text;

using ArnoBot.Core;
using ArnoBot.Core.Responses;

namespace ArnoBot.Interface
{
    public interface ICommand
    {
        Response Execute(CommandContext context);
    }
}
