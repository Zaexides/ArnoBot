using System;
using System.Collections.Generic;
using System.Text;

using ArnoBot.Core;

namespace ArnoBot.Interface
{
    public interface ICommand
    {
        Response Execute(CommandContext context);
    }
}
