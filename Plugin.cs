using BepInEx;
using BepInEx.Logging;
using Comfort.Common;
using Fika.Core.Modding;
using Fika.Core.Modding.Events;
using Fika.Core.Networking;
using LiteNetLib;
using ShoulderCQB.Fika;
using ShoulderCQB.Patches;

namespace ShoulderCQB
{
    [BepInPlugin("com.lillian.cqb",  "CQB", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;

        private void Awake()
        {
            LogSource = Logger;
            LogSource.LogInfo("CQB Plugin loaded!");
            
            // Attach collider on interactive layer to shoulder bone
            new PatchPlayerCreate().Enable();
            new PatchGameWorldStart().Enable();
            new PatchAvailableActions().Enable();

            FikaEventDispatcher.SubscribeEvent<FikaNetworkManagerCreatedEvent>(FikaMethods.OnFikaNetManagerCreated);
        }
    }
}