using System.Runtime.Serialization;
using Unity.Netcode;

namespace Matrix.TerminalExtensions.Configs;

[Serializable]
public class SyncedInstance<T>
{
    public static CustomMessagingManager MessageManager => NetworkManager.Singleton.CustomMessagingManager;
    public static bool IsClient => NetworkManager.Singleton.IsClient;
    public static bool IsHost => NetworkManager.Singleton.IsHost;

    [NonSerialized] protected static int IntSize = 4;
    [NonSerialized] static readonly DataContractSerializer serializer = new(typeof(T));

    internal static T? Default { get; private set; }
    internal static T? Instance { get; private set; }

    internal static bool IsSynced;

    protected void InitInstance(T instance)
    {
        Default = instance;
        Instance = instance;

        // Ensures the size of an integer is correct for the current system.
        IntSize = 4;
    }

    internal static void SyncInstance(byte[] data)
    {
        Instance = DeserializeFromBytes(data);
        IsSynced = true;
    }

    internal static void RevertSync()
    {
        Instance = Default;
        IsSynced = false;
    }

    public static byte[]? SerializeToBytes(T? val)
    {
        if (val is null) return null;

        using MemoryStream stream = new();

        try
        {
            serializer.WriteObject(stream, val);
            return stream.ToArray();
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogError($"Error serializing instance: {ex}");
            return null;
        }
    }

    public static T? DeserializeFromBytes(byte[] data)
    {
        using MemoryStream stream = new(data);

        try
        {
            return (T)serializer.ReadObject(stream);
        }
        catch (Exception ex)
        {
            Plugin.Logger.LogDebug($"Error deserializing instance: {ex}");
            return default;
        }
    }

    internal static void SendMessage(string label, ulong clientId, FastBufferWriter stream)
    {
        bool fragment = stream.Capacity >= stream.MaxCapacity;
        NetworkDelivery delivery = fragment
            ? NetworkDelivery.ReliableFragmentedSequenced
            : NetworkDelivery.Reliable;

        if (fragment)
        {
            Plugin.Logger.LogDebug(
                $"Size of stream ({stream.Capacity}) was past the max buffer size.\n" +
                "Config instance will be sent in fragments to avoid overflowing the buffer.");
        }

        MessageManager.SendNamedMessage(label, clientId, stream, delivery);
    }
}
