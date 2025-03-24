using Comfort.Common;
using EFT;
using EFT.Communications;
using Fika.Core.Coop.Components;
using Fika.Core.Coop.Players;
using Fika.Core.Modding.Events;
using Fika.Core.Networking;
using LiteNetLib;
using ShoulderCQB.Common;
using UnityEngine;

namespace ShoulderCQB.Fika;

public class FikaMethods
{
    public static void OnFikaNetManagerCreated(FikaNetworkManagerCreatedEvent managerCreatedEvent)
    {
        managerCreatedEvent.Manager.RegisterPacket<CQBPacket>(CQBPacketReceived);
    }

    public static void SendCQBPacket(int sender, int receiver, bool tapPlayer, int tapLocation, float setMovementSpeed, bool sprintEnable)
    {
        CQBPacket packet = new()
        {
            SenderID = sender,
            ReceiverID = receiver,
            TapPlayer = tapPlayer,
            TapLocation = tapLocation,
            SetMovementSpeed = setMovementSpeed,
            SprintEnable = sprintEnable,
        };
        
        if (Singleton<FikaServer>.Instantiated)
        {
            Singleton<FikaServer>.Instance.SendDataToAll(ref packet, DeliveryMethod.ReliableOrdered);
        }
    }
    private static void CQBPacketReceived(CQBPacket packet)
    {
        CoopHandler.TryGetCoopHandler(out CoopHandler coopHandler);
        
        if (Utils.MainPlayer == coopHandler.Players[packet.ReceiverID])
        {
            if (packet.SprintEnable == false)
            {
                Utils.MainPlayer.Physical.Sprint(false);
            }

            if (packet.SetMovementSpeed != 0)
            {
                Utils.MainPlayer.MovementContext.SetCharacterMovementSpeed(packet.SetMovementSpeed, true);
            }

            if (packet.TapPlayer)
            {
                if (packet.TapLocation == 0)
                {
                    //Utils.MainPlayer.MovementContext.SetTilt(-0.5f, true);
                    NotificationManagerClass.DisplayMessageNotification("Left Shoulder Tapped",
                        ENotificationDurationType.Default, ENotificationIconType.Alert, Color.green);
                }
                else if (packet.TapLocation == 1)
                {
                    //Utils.MainPlayer.MovementContext.SetTilt(0.5f, true);
                    NotificationManagerClass.DisplayMessageNotification("Right Shoulder Tapped",
                        ENotificationDurationType.Default, ENotificationIconType.Alert, Color.green);
                }
                else if (packet.TapLocation == 2)
                {
                    //Utils.MainPlayer.MovementContext.SetTilt(-1.5f, true);
                    NotificationManagerClass.DisplayMessageNotification("Left Leg Tapped",
                        ENotificationDurationType.Default, ENotificationIconType.Alert, Color.green);
                }
                else if (packet.TapLocation == 3)
                {
                    //Utils.MainPlayer.MovementContext.SetTilt(1.5f, true);
                    NotificationManagerClass.DisplayMessageNotification("Right Leg Tapped",
                        ENotificationDurationType.Default, ENotificationIconType.Alert, Color.green);
                }
            }

            if (Singleton<FikaServer>.Instantiated)
            {
                Singleton<FikaServer>.Instance.SendDataToAll(ref packet, DeliveryMethod.ReliableOrdered);
            }
        }
    }
}