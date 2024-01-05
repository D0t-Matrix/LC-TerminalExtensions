using System;

namespace TerminalAPI.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public abstract class AccessControlAttribute : Attribute
{
    public abstract bool CheckAllowed();
}
