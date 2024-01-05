using System.Collections.Generic;
using System.Reflection;

namespace TerminalAPI.Models;

public class TerminalModRegistry
{
    public List<TerminalCommand> Commands { get; } = [];

    public T RegisterFrom<T>() where T : class, new() => RegisterFrom(new T());

    public T RegisterFrom<T>(T instance) where T : class
    {
        foreach (var commandMethod in TerminalRegistry.GetCommandMethods<T>())
        {
            var command = TerminalCommand.FromMethod(commandMethod, instance);
            TerminalRegistry.RegisterCommand(command);
            
            lock (Commands)
                Commands.Add(command);
        }

        StringConverter.RegisterFrom<T>(instance);
        return instance;
    }

    public void Deregister()
    {
        if (Commands is null)
            return;

        for(int i = 0; i < Commands.Count; ++i)
        {
            TerminalRegistry.Deregister(Commands[i]);
        }
    }
}
