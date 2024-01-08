using BepInEx.Configuration;
using System.Runtime.Serialization;
using Unity.Netcode;

namespace Matrix.TerminalExtensions.Configs;

[DataContract]
public class Config : SyncedInstance<Config>
{
    [DataMember] public bool EnablePlugin { get; private set; }
    [DataMember] public bool EnableCheatCommands { get; private set; }

    public Config(ConfigFile cfg)
    {
        InitInstance(this);

        EnablePlugin = cfg.Bind(GeneratedPluginInfo.Name, "enabled", true, "Enable or disable the plugin globally.").Value;

        EnableCheatCommands = cfg.Bind(GeneratedPluginInfo.Name, "cheat commands enabled", false, "Enable or disable the cheat commands.").Value;
    }

    public static void RequestSync()
    {
        if (!IsClient) return;

        using FastBufferWriter stream = new(IntSize, Unity.Collections.Allocator.Temp);
        SendMessage($"{GeneratedPluginInfo.Name}_OnRequestConfigSync", 0uL, stream);
    }

    public static void OnRequestSync(ulong clientId, FastBufferReader _)
    {
        if (!IsHost) return;

        Plugin.Logger.LogInfo($"Config sync request received from client: {clientId}");

        byte[]? data = SerializeToBytes(Instance);

        if (data is null)
        {
            Plugin.Logger.LogWarning("Config instance is null, unable to sync data!");
            return;
        }

        int value = data.Length;

        using FastBufferWriter stream = new(value + IntSize, Unity.Collections.Allocator.Temp);

        try
        {
            stream.WriteValueSafe(in value, default);
            stream.WriteBytesSafe(data);
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogInfo($"Error occurred when syncing config with client: {clientId}\n{ex}");
        }
    }

    public static void OnReceiveSync(ulong _, FastBufferReader reader)
    {
        if (!reader.TryBeginRead(IntSize))
        {
            Plugin.Logger.LogError("Config sync error: Could not begin reading buffer.");
            return;
        }

        reader.ReadValueSafe(out int value, default);
        if (!reader.TryBeginRead(value))
        {
            Plugin.Logger.LogError("Config sync error: Host could not sync.");
            return;
        }

        byte[] data = new byte[value];
        reader.ReadBytesSafe(ref data, value);

        SyncInstance(data);

        Plugin.Logger.LogInfo("Successfully synced config with host.");
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameNetworkManager), "JoinLobby")]
    public static void InitializeLocalPlayer()
    {
        if (IsHost)
        {
            MessageManager.RegisterNamedMessageHandler($"{GeneratedPluginInfo.Name}_OnRequestConfigSync", OnRequestSync);
            IsSynced = true;

            return;
        }

        IsSynced = false;
        MessageManager.RegisterNamedMessageHandler($"{GeneratedPluginInfo.Name}_OnReceiveConfigSync", OnReceiveSync);
        RequestSync();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(GameNetworkManager), "StartDisconnect")]
    public static void PlayerLeave()
    {
        RevertSync();
    }
}
