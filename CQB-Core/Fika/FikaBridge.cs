namespace CQB.Fika;

public class FikaBridge
{
    public delegate void SimpleEvent();
    public delegate bool SimpleBoolReturnEvent();
    
    public static event SimpleEvent PluginEnableEmitted;
    public static void PluginEnable() { PluginEnableEmitted?.Invoke(); }
    
    public static event SimpleBoolReturnEvent IAmHostEmitted;
    public static bool IAmHost()
    {
        bool? eventResponse = IAmHostEmitted?.Invoke();

        if (eventResponse == null)
        {
            return true;
        }
        else
        {
            return eventResponse.Value;
        }
    }
    
    public delegate void SendCQBPacketEvent(int sender, int receiver, bool tapPlayer, int tapLocation, float setMovementSpeed, bool sprintEnable);
    public static event SendCQBPacketEvent SendCQBPacketEmitted;

    public static void SendCQBPacket(int sender, int receiver, bool tapPlayer, int tapLocation, float setMovementSpeed,
        bool sprintEnable)
    {
        SendCQBPacketEmitted?.Invoke(sender, receiver, tapPlayer, tapLocation, setMovementSpeed, sprintEnable);
    }
    
    
}