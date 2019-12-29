using System;
using System.Threading.Tasks;

using ArnoBot.Interface;
using ArnoBot.Core.Responses;
using ArnoBot.Core.Exceptions;

namespace ArnoBot.Core
{
    public class Bot : IDisposable
    {
        private static Bot singleton;

        public FindCommandFromContext FindCommandFromContextDelegate { get; set; }
        public ExecuteCommand ExecuteCommandDelegate { get; set; }
        public ModuleRegistry ModuleRegistry { get; }

        private Bot()
        {
            ModuleRegistry = new ModuleRegistry();
            FindCommandFromContextDelegate = new FindCommandFromContext(FindCommandFromContextInternal);
            ExecuteCommandDelegate = new ExecuteCommand(ExecuteCommandInternal);
        }

        public static Bot CreateOrGet()
        {
            if(singleton == null)
            {
                singleton = new Bot();
            }
            return singleton;
        }

        public Response Query(string command)
        {
            CommandContext commandContext = CommandContext.Parse(command);
            ICommand commandObject = FindCommandFromContextDelegate(commandContext);
            if (commandObject == null)
                return new ErrorResponse(Response.Type.NotFound, new CommandNotFoundException($"Command \"{commandContext.CommandName}\" could not be found."));
            else
                return ExecuteCommandDelegate(commandObject, commandContext);
        }

        public delegate ICommand FindCommandFromContext(CommandContext commandContext);
        public delegate Response ExecuteCommand(ICommand command, CommandContext commandContext);

        private ICommand FindCommandFromContextInternal(CommandContext commandContext)
        {
            foreach(IModule module in ModuleRegistry.GetModules())
            {
                if (module.CommandRegistry.ContainsKey(commandContext.CommandName))
                    return module.CommandRegistry[commandContext.CommandName];
            }
            return null;
        }

        private Response ExecuteCommandInternal(ICommand command, CommandContext commandContext)
        {
            try
            {
                return command.Execute(commandContext);
            }
            catch(Exception ex)
            {
                return new ErrorResponse(Response.Type.Error, ex);
            }
        }

        public void QueryAsync(string command, Action<Response> callback)
        {
            Task task = new Task(() =>
            {
                callback(Query(command));
            });
            task.Start();
        }

        public void Dispose()
        {
            singleton = null;
        }
    }
}
