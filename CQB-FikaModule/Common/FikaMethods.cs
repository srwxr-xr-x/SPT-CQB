using Comfort.Common;
using CQB.FikaModule.Packets;
using EFT;
using EFT.Communications;
using Fika.Core.Coop.Components;
using Fika.Core.Modding;
using Fika.Core.Modding.Events;
using Fika.Core.Networking;
using LiteNetLib;
using UnityEngine;

namespace CQB.FikaModule.Common;

public class FikaMethods
{
    public static void OnFikaNetManagerCreated(FikaNetworkManagerCreatedEvent managerCreatedEvent)
    {
        managerCreatedEvent.Manager.RegisterPacket<CQBPacket>(CQBPacketReceived);
    }
    public static void InitOnPluginEnabled()
    {
        FikaEventDispatcher.SubscribeEvent<FikaNetworkManagerCreatedEvent>(OnFikaNetManagerCreated);
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
        
        if (Singleton<GameWorld>.Instance.MainPlayer == coopHandler.Players[packet.ReceiverID])
        {
            if (packet.SprintEnable == false)
            {
                Plugin.LogSource.LogInfo(Singleton<GameWorld>.Instance.MainPlayer);
                Singleton<GameWorld>.Instance.MainPlayer.ActiveHealthController.AddFatigue();
                Singleton<GameWorld>.Instance.MainPlayer.ActiveHealthController.SetStaminaCoeff(0f);
            }
            if (packet.SetMovementSpeed != 0)
            {
                Singleton<GameWorld>.Instance.MainPlayer.Physical.WalkSpeedLimit = packet.SetMovementSpeed;
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