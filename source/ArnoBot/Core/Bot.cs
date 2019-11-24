using System;
using System.Threading.Tasks;

using ArnoBot.Interface;

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
            CommandContext context = CommandContext.Parse(command);
            foreach(IModule module in ModuleRegistry.GetModules())
            {
                if (module.CommandRegistry.ContainsKey(context.CommandName))
                    return module.CommandRegistry[context.CommandName].Execute(context);
            }
            return new Response.Builder(Response.Type.NotFound, $"Command {context.CommandName} could not be found.").Build();
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
