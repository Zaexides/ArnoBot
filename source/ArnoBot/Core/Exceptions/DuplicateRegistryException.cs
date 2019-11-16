using System;
using System.Collections.Generic;
using System.Text;

namespace ArnoBot.Core
{

    [Serializable]
    public class DuplicateRegistryException : Exception
    {
        public DuplicateRegistryException() { }
        public DuplicateRegistryException(string message) : base(message) { }
        public DuplicateRegistryException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateRegistryException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
