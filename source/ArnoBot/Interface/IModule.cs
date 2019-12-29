using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Interface
{
    public interface IModule
    {
        string Name { get; }

        IReadOnlyDictionary<string, ICommand> CommandRegistry { get; }
    }
}
