using BepInEx.Logging;
using MatrixTermExtensions.Commands;
using TerminalAPI.Models;

namespace MatrixTermExtensions;

[BepInPlugin(GeneratedPluginInfo.Identifier, GeneratedPluginInfo.Name, GeneratedPluginInfo.Version)]
[BepInDependency("Matrix.TerminalAPI", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log = BepInEx.Logging.Logger.CreateLogSource(GeneratedPluginInfo.Name);

    private TerminalModRegistry? _commands;

    public void Awake()
    {
        Log.LogInfo("Registering Commands.");
        _commands = TerminalRegistry.RegisterFromAssembly(typeof(Plugin).Assembly);

        if (_commands is null || _commands.Commands is null || _commands.Commands.Count == 0)
        {
            Log.LogError("Commands are not getting added!");
        }

    }

    //public void OnDestroy()
    //{
    //    if (DoorCommands.Deregister())
    //    {
    //        Log.LogInfo("Deregistered door commands.");
    //    }

    //    if (LaunchCommands.Deregister())
    //    {
    //        Log.LogInfo("Deregistered launch commands.");
    //    }
    //    if (LightsCommands.Deregister())
    //    {
    //        Log.LogInfo("Deregistered lighting commands.");
    //    }

    //    if (TeleporterCommands.Deregister())
    //    {
    //        Log.LogInfo("Deregistered teleporter commands.");
    //    }

    //    if (CheatCommands.Deregister())
    //    {
    //        Log.LogInfo("Deregistered the cheat commands.");
    //    }
    //}
}