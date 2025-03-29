using Comfort.Common;
using CQB.Fika;
using CQB.FikaModule.Common;
using Fika.Core.Networking;

namespace CQB.FikaModule
{
    internal class Main
    {
        public static void Init()
        {
            FikaBridge.PluginEnableEmitted += PluginEnable;
            FikaBridge.IAmHostEmitted += IAmHost;
            
            FikaBridge.SendCQBPacketEmitted += FikaMethods.SendCQBPacket;
        }
        public static void PluginEnable()
        {
            FikaMethods.InitOnPluginEnabled();
        }
        public static bool IAmHost()
        {
            return Singleton<FikaServer>.Instantiated;
        }
    }
}