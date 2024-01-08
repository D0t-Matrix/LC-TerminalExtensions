using UnityEngine;
using LethalAPI.LibTerminal.Attributes;

namespace Matrix.TerminalExtensions.Commands;

public class LightsCommands
{
    [TerminalCommand("Lights", false)]
    [CommandInfo("Turns the lights on or off.", "Lights [on/off]")]
    public string SwitchLightsCommand([RemainingText] string subcommand)
        => subcommand.ToLowerInvariant() switch
        {
            "on" or "up" => LightsOnCommand(),
            "off" or "out" => LightsOffCommand(),
            _ => "Invalid parameter!",
        };

    [TerminalCommand("Lightup", false)]
    [CommandInfo("Turns the lights on, if not already on")]
    public string LightsOnCommand()
    {
        if (TrySwitchLights(switchOn: true))
            return "Lights turned on.";

        return "Lights alredy on!";
    }

    [TerminalCommand("lightout", false)]
    [CommandInfo("Turns the lights off, if not already off")]
    public string LightsOffCommand()
    {
        if (TrySwitchLights(switchOn: false))
            return "Lights turned off.";

        return "Lights already off!";
    }

    [TerminalCommand("Lightsout", false)]
    [CommandInfo("Turns the lights off, if not already off")]
    public string LightsOutCommand() => LightsOffCommand();

    private static bool TrySwitchLights(bool switchOn)
    {
        GameObject lightSwtich = GameObject.Find("LightSwitch");

        InteractTrigger lightSwitchTrigger = lightSwtich.GetComponent<InteractTrigger>();

        if (StartOfRound.Instance.shipRoomLights.enabled == switchOn)
        {
            lightSwitchTrigger.onInteract.Invoke(GameNetworkManager.Instance.localPlayerController);

            return true;
        }

        return false;
    }
}
