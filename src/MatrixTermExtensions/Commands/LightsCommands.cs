using TerminalAPI.Attributes;
using TerminalAPI.Models;
using MatrixTermExtensions.Extensions;
using UnityEngine;

namespace MatrixTermExtensions.Commands;

public class LightsCommands
{
    [TerminalCommand("Lightup")]
    [CommandInfo("Turns the lights on.")]
    public string LightsOnCommand()
    {
        if (StartOfRound.Instance.shipRoomLights.enabled) return "Lights already on.";

        var trigger = GameObject.Find("LightSwitch").GetComponent<InteractTrigger>();
        trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

        return "Lights turned on.";
    }

    [TerminalCommand("Lumos")]
    [CommandInfo("Turns the lights on.")]
    public string LumosCommand() => LightsOnCommand();

    [TerminalCommand("Lightsout")]
    [CommandInfo("Turns the lights off.")]
    public string LightsOutCommand()
    {
        if (!StartOfRound.Instance.shipRoomLights.enabled) return "Lights already off.";

        var trigger = GameObject.Find("LightSwitch").GetComponent<InteractTrigger>();
        trigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

        return "Lights turned off.";
    }

    [TerminalCommand("Nox")]
    [CommandInfo("Turns the lights off.")]
    public string NoxCommand() => LightsOutCommand();

    [TerminalCommand("lights")]
    [CommandInfo("Turns the lights on or off.")]
    public string LightsToggleCommand([RemainingText] string? subcommand)
    {
        if (subcommand.IsNullOrEmpty()) return "Invalid Command";

        return subcommand.ToLowerInvariant() switch
        {
            "on" => LightsOnCommand(),
            "off" or "out" => LightsOutCommand(),
            _ => "Invalid Command.",
        };
    }
}
