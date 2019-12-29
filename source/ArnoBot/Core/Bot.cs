using System;
using System.Threading.Tasks;

using ArnoBot.Interface;
using ArnoBot.Core.Responses;

namespace ArnoBot.Core
{
    public class Bot : IDisposable
    {
        private static Bot singleton;

        public ModuleRegistry ModuleRegistry { get; }

        private Bot()
        {
            ModuleRegistry = new ModuleRegistry();
        }

        public static Bot CreateOrGet()
        {
            if(singleton == null)
            {
                singleton = new Bot();
            }
            return singleton;
        }

        public Response Query(string command, Func<ICommand, CommandContext, Response> executeAction)
        {
            CommandContext commandContext = CommandContext.Parse(command);
            ICommand commandObject = FindCommandFromContext(commandContext);
            if (commandObject == null)
                return new ErrorResponse(Response.Type.NotFound, new CommandNotFoundException($"Command \"{commandContext.CommandName}\" could not be found."));
            else
                return ExecuteCommand(commandObject, commandContext);
        }

        public Response Query(string command)
            => Query(command, (commandObject, ctx) => commandObject.Execute(ctx));

        private ICommand FindCommandFromContext(CommandContext commandContext)
        {
            foreach(IModule module in ModuleRegistry.GetModules())
            {
                if (module.CommandRegistry.ContainsKey(commandContext.CommandName))
                    return module.CommandRegistry[commandContext.CommandName];
            }
            return null;
        }

        private Response ExecuteCommand(ICommand command, CommandContext commandContext)
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

        public void QueryAsync(string command, Func<ICommand, CommandContext, Response> executeAction, Action<Response> callback)
        {
            Task task = new Task(() =>
            {
                callback(Query(command));
            });
            task.Start();
        }

        public void QueryAsync(string command, Action<Response> callback)
            => QueryAsync(command, (commandObject, ctx) => commandObject.Execute(ctx), callback);

        public void Dispose()
        {
            singleton = null;
        }
    }
}
