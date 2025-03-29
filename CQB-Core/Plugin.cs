using System;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using CQB.Fika;
using CQB.Patches;

namespace CQB
{
    [BepInDependency("com.fika.core", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("com.lillian.cqb",  "CQB", "1.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static ManualLogSource LogSource;
        public static bool FikaInstalled { get; private set; }
        
        private void Awake()
        {
            FikaInstalled = Chainloader.PluginInfos.ContainsKey("com.fika.core");
            LogSource = Logger;
            LogSource.LogInfo("CQB Plugin loaded!");
            
            // Attach collider on interactive layer to shoulder bone
            new PatchPlayerCreate().Enable();
            new PatchGameWorldStart().Enable();
            new PatchAvailableActions().Enable();
            
            TryInitFikaAssembly();
        }

        private void OnEnable()
        {
            FikaBridge.PluginEnable();
        }

        void TryInitFikaAssembly()
        {
            if (!FikaInstalled) return;
            
            Assembly fikaModuleAssembly = Assembly.Load("CQB-FikaModule");
            Type main = fikaModuleAssembly.GetType("CQB.FikaModule.Main");
            MethodInfo init = main.GetMethod("Init");
            
            init.Invoke(main, null);
        }
    }
}