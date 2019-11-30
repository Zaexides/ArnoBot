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

        public Response Query(string command)
        {
            try
            {
                CommandContext context = CommandContext.Parse(command);
                foreach (IModule module in ModuleRegistry.GetModules())
                {
                    if (module.CommandRegistry.ContainsKey(context.CommandName))
                        return module.CommandRegistry[context.CommandName].Execute(context);
                }
                throw new CommandNotFoundException($"Command {context.CommandName} could not be found.");
            }
            catch(CommandNotFoundException notFoundException)
            {
                return new ErrorResponse(Response.Type.NotFound, notFoundException);
            }
            catch(Exception exception)
            {
                return new ErrorResponse(Response.Type.Error, exception);
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
