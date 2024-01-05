using System;
using TerminalAPI.Models;

namespace TerminalAPI.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AllowedCallerAttribute : AccessControlAttribute
{
    public AllowedCaller Caller { get; }

    public AllowedCallerAttribute(AllowedCaller caller) => Caller = caller;

    public override bool CheckAllowed() => Caller switch
    {
        AllowedCaller.None => false,
        AllowedCaller.Player => true,
        AllowedCaller.Host => !Equals(StartOfRound.Instance, null) && (StartOfRound.Instance).IsHost,
        _ => true,
    };
}
