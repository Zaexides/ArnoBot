using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core
{
    [Serializable]
    public abstract class ArnoBotException : Exception
    {
        public abstract string SimpleName { get; }

        public ArnoBotException() { }
        public ArnoBotException(string message) : base(message) { }
        public ArnoBotException(string message, Exception inner) : base(message, inner) { }
        protected ArnoBotException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
