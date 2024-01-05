using System;

namespace TerminalAPI.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class TerminalCommandAttribute : Attribute
{
    public string CommandName { get; }

    public bool ClearText { get; }

    public TerminalCommandAttribute(string name, bool clearText = false)
    {
        CommandName = name;
        ClearText = clearText;
    }
}
