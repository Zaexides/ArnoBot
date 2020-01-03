using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core
{

    [Serializable]
    public class CommandNotFoundException : ArnoBotException
    {
        public override string SimpleName => "Command not found";

        public CommandNotFoundException() { }
        public CommandNotFoundException(string message) : base(message) { }
        public CommandNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected CommandNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
