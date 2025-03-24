using EFT;
using LiteNetLib.Utils;

namespace ShoulderCQB.Fika;

public struct CQBPacket : INetSerializable
{
    public int SenderID;
    public int ReceiverID;
    public bool TapPlayer;
    public int TapLocation;
    public float SetMovementSpeed;
    public bool SprintEnable;
    
    public void Serialize(NetDataWriter writer)
    {
        writer.Put(SenderID);
        writer.Put(ReceiverID);
        writer.Put(TapPlayer);
        writer.Put(TapLocation);
        writer.Put(SetMovementSpeed);
        writer.Put(SprintEnable);
    }

    public void Deserialize(NetDataReader reader)
    {
        SenderID = reader.GetInt();
        ReceiverID = reader.GetInt();
        TapPlayer = reader.GetBool();
        TapLocation = reader.GetInt();
        SetMovementSpeed = reader.GetFloat();
        SprintEnable = reader.GetBool();
    }
    
}