using System.Linq;
using System.Reflection;
using Comfort.Common;
using EFT;
using SPT.Reflection.Patching;
using UnityEngine;

namespace CQB.Patches;

public class PatchGameWorldStart : ModulePatch
{
    protected override MethodBase GetTargetMethod() {
        return typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));
    }

    [PatchPrefix]
    public static void PatchPrefix()
    {
        // Find and enable all the Interactive objects after Player.create<T>, since it overrides it.
        foreach (GameObject interact in Resources.FindObjectsOfTypeAll<GameObject>()
                     .Where(obj => obj.name.Contains("CQB")))
        {
            interact.layer = LayerMask.NameToLayer("Interactive");
            interact.GetComponent<BoxCollider>().enabled = true;
        }
        Common.Utils.MainPlayer = Singleton<GameWorld>.Instance.MainPlayer;
    }
}