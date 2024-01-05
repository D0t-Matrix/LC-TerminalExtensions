using BepInEx.Logging;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;
using TerminalAPI.Attributes;
using UnityEngine;

namespace TerminalAPI.Models;

public class TerminalCommand
{
    private readonly ManualLogSource _logSource = new("Matrix.TerminalAPI");

    public string Name { get; }

    public MethodInfo Method { get; }

    public object Instance { get; }

    public bool ClearConsole { get; }

    public int ArgumentCount { get; }

    public string? Syntax { get; }

    public string? Description { get; }

    public int Priority { get; }

    public TerminalCommand(
      string name,
      MethodInfo method,
      object instance,
      bool clearConsole,
      string? syntax = null,
      string? description = null,
      int priority = 0)
    {
        Name = name;
        Method = method;
        Instance = instance;
        ClearConsole = clearConsole;
        ArgumentCount = method.GetParameters().Length;
        Syntax = syntax;
        Description = description;
        Priority = priority;
    }

    public bool CheckAllowed()
    {
        foreach(var attribute in Method.GetCustomAttributes<AccessControlAttribute>())
        {
            if (!attribute.CheckAllowed())
                return false;
        }

        return true;
    }

    public static TerminalCommand FromMethod(MethodInfo info, object instance, string? overrideName = null)
    {
        bool clearConsole = false;
        string? syntax = null;
        string? description = null;
        string? name = overrideName;
        int priority = 0;
        TerminalCommandAttribute commandAttr = info.GetCustomAttribute<TerminalCommandAttribute>();
        if (commandAttr != null)
        {
            name ??= commandAttr.CommandName;
            clearConsole = commandAttr.ClearText;
        }
        CommandInfoAttribute commandInfoAttr = info.GetCustomAttribute<CommandInfoAttribute>();
        if (commandInfoAttr != null)
        {
            syntax = commandInfoAttr.Syntax;
            description = commandInfoAttr.Description;
        }
        
        CommandPriorityAttribute priorityAttr = info.GetCustomAttribute<CommandPriorityAttribute>();
        if (priorityAttr != null)
            priority = priorityAttr.Priority;

        return new TerminalCommand(name!, info, instance, clearConsole, syntax, description, priority);
    }

    public bool TryCreateInvoker(string[] arguments, Terminal terminal, [NotNullWhen(true)] out Func<TerminalNode>? invoker)
    {
        var parameters = Method.GetParameters();
        var values = new object[parameters.Length];
        var argStream = new ArgumentStream(arguments);
        
        for (int i = 0; i < parameters.Length; i++)
        {
            var element = parameters[i];
            var paramType = element.ParameterType;

            if (paramType == typeof(Terminal))
                values[i] = terminal;
            else if (paramType == typeof(ArgumentStream))
                values[i] = argStream;
            else if (paramType == typeof(string[]))
                values[i] = arguments;
            else if (paramType == typeof(string) && element.GetCustomAttribute<RemainingTextAttribute>() != null)
            {
                if (argStream.TryReadRemaining(out var result))
                {
                    values[i] = result;
                }
                else
                {
                    invoker = null;
                    return false;
                }
            }
            else
            {
                if (argStream.TryReadNext(paramType, out var result))
                {
                    values[i] = result;
                }
                else
                {
                    invoker = null;
                    return false;
                }
            }
        }
        argStream.Reset();

        #pragma warning disable CS8603 // Possible null reference return.
        invoker = () => ExecuteCommand(values);
        #pragma warning restore CS8603 // Possible null reference return.

        return invoker is not null;
    }

    private TerminalNode? ExecuteCommand(object[] arguments)
    {
        object? obj;
        try
        {
            obj = Method.Invoke(Instance, arguments);
        }
        catch (Exception ex)
        {
            _logSource.LogError("Error caught while invoking command handler: " + ex.Message);
            _logSource.LogError(ex.StackTrace);
            return null;
        }
        if (obj is null) 
            return null;
        if (typeof(TerminalNode).IsAssignableFrom(obj.GetType()))
            return (TerminalNode)obj;

        var instance = ScriptableObject.CreateInstance<TerminalNode>();
        instance.displayText = obj.ToString() + "\n";
        instance.clearPreviousText = this.ClearConsole;
        return instance;

    }
}
