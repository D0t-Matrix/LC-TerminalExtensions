using BepInEx.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TerminalAPI.Attributes;

namespace TerminalAPI.Models;

public class TerminalRegistry
{
    private static readonly ConcurrentDictionary<string, List<TerminalCommand>> _registeredCommands = new(StringComparer.InvariantCultureIgnoreCase);

    private static readonly ManualLogSource _logSource = Logger.CreateLogSource("Matrix.TerminalAPI.TerminalRegistry");

    public static TerminalModRegistry RegisterFromAssembly(Assembly assembly)
    {
        _logSource.LogInfo($"Registering commands in assembly: {assembly.FullName}.");
        var terminalModRegistry = new TerminalModRegistry();
        var types = AccessTools.GetTypesFromAssembly(assembly).Where(type => type.Namespace.Contains(assembly.FullName.Split(',').First()));
        _logSource.LogInfo($"Types identified:\n");
        
        foreach(var type in types)
        {
            _logSource.LogInfo(type.Name);
        }

        foreach(var assemblyType in types )
        {
            _logSource.LogInfo($"Registering commands in type: {assemblyType}.");
            try
            {
                var instance = Activator.CreateInstance(assemblyType);
                foreach (var commandMethod in GetCommandMethods(assemblyType))
                {
                    _logSource.LogInfo($"Attempting to register {commandMethod}.");
                    _logSource.LogInfo($"{commandMethod.Name}\n\n");
                    var command = TerminalCommand.FromMethod(commandMethod, instance);
                    _logSource.LogInfo($"Command {command.Name} registered.");
                    RegisterCommand(command);
                    terminalModRegistry.Commands.Add(command);
                }

                StringConverter.RegisterFrom(instance);
            }
            catch (MissingMethodException ex)
            {
                _logSource.LogError(ex.Message);
            }
        }

        return terminalModRegistry;
    }

    public static TerminalModRegistry RegisterFromType(Type type)
    {
        _logSource.LogInfo($"Registering commands in type: {type}.");
        var terminalModRegistry = new TerminalModRegistry();
        var instance = Activator.CreateInstance(type);
        foreach (var commandMethod in GetCommandMethods(type))
        {
            var command = TerminalCommand.FromMethod(commandMethod, instance);
            RegisterCommand(command);
            terminalModRegistry.Commands.Add(command);
        }

        StringConverter.RegisterFrom(instance);
        return terminalModRegistry;
    }

    public static TerminalModRegistry RegisterFrom<T>(T instance) where T : class
    {
        _logSource.LogInfo($"Registering commands in instance: {instance}.");
        var terminalModRegistry = new TerminalModRegistry();
        foreach(var commandMethod in GetCommandMethods<T>())
        {
            var command = TerminalCommand.FromMethod(commandMethod, instance);
            RegisterCommand(command);
            terminalModRegistry.Commands.Add(command);
        }

        StringConverter.RegisterFrom<T>(instance);
        return terminalModRegistry;
    }

    public static TerminalModRegistry CreateTerminalRegistry() => new();

    public static void RegisterCommand(TerminalCommand command)
    {
        if (!_registeredCommands.TryGetValue(command.Name, out List<TerminalCommand> terminalCommandList))
        {
            terminalCommandList = [];
            _registeredCommands[command.Name] = terminalCommandList;
        }
        lock (terminalCommandList)
        {
            terminalCommandList.Add(command);
        }
    }

    public static void Deregister(TerminalCommand command)
    {
        if (_registeredCommands.TryGetValue(command.Name, out List<TerminalCommand>? commandList))
        {
            lock (commandList)
            {
                commandList.Remove(command);
            }
        }

        return;
    }

    public static IReadOnlyList<TerminalCommand> GetCommands(string commandName) 
        => _registeredCommands.TryGetValue(commandName, out List<TerminalCommand>? terminalCommands)
            ? (IReadOnlyList<TerminalCommand>)terminalCommands
            : new List<TerminalCommand>();

    public static IEnumerable<TerminalCommand> EnumerateCommands(string name) 
        => !_registeredCommands.TryGetValue(name, out List<TerminalCommand> terminalCommandList)
            ? Enumerable.Empty<TerminalCommand>()
            : terminalCommandList;

    public static IEnumerable<TerminalCommand> EnumerateCommands()
    {
        foreach(var key in _registeredCommands.Keys)
        {
            foreach (var overloads in _registeredCommands[key])
            {
                yield return overloads;
            }
        }
    }

    public static IEnumerable<MethodInfo> GetCommandMethods<T>()
    {
        foreach(var element in typeof(T).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (element.GetCustomAttribute<TerminalCommandAttribute>() != null) 
                yield return element;
        }
    }
    public static IEnumerable<MethodInfo> GetCommandMethods(Type type)
    {
        foreach(var element in type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
        {
            if (element.GetCustomAttribute<TerminalCommandAttribute>() != null) 
                yield return element;
        }
    }
}
