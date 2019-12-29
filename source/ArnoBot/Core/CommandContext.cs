using System;
using System.Collections.Generic;
using System.Text;

using ArnoBot.Interface;

namespace ArnoBot.Core
{
    public class CommandContext
    {
        public string CommandName { get; }
        public object[] Parameters { get; }

        protected CommandContext(string commandName, object[] parameters)
        {
            this.CommandName = commandName;
            this.Parameters = parameters;
        }

        internal static CommandContext Parse(string receivedCommand)
        {
            List<string> queryParts = new List<string>(receivedCommand.Split(' '));
            queryParts.RemoveAll((s) => { return s == null || s.Equals(string.Empty); });

            object[] parameters = new object[queryParts.Count - 1];

            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = ParseParameter(queryParts[i + 1].Trim());

            return new CommandContext(queryParts[0], parameters);
        }

        private static object ParseParameter(string parameter)
        {
            int intValue;
            float floatValue;
            if (int.TryParse(parameter, out intValue))
                return intValue;
            else if (float.TryParse(parameter, out floatValue))
                return floatValue;
            else
                return parameter;
        }
    }
}
