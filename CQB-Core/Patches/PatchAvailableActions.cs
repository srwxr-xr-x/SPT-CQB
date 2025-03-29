using System.Reflection;
using EFT;
using HarmonyLib;
using CQB.Components;
using SPT.Reflection.Patching;

namespace CQB.Patches;

public class PatchAvailableActions : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.FirstMethod(typeof(GetActionsClass), method => method.Name == nameof(GetActionsClass.GetAvailableActions) && method.GetParameters()[0].Name == "owner");
    }

    [PatchPrefix]
    static bool PatchPrefix(GamePlayerOwner owner, object interactive, ref ActionsReturnClass __result)
    {
        // Add the interactions to the list. 
        if (interactive is ShoulderInteractable)
        {
            ShoulderInteractable shoulder = interactive as ShoulderInteractable;
            ActionsReturnClass newResult = shoulder.GetActions();
            __result = newResult;
            return false;
        }
        if (interactive is ThighInteractable)
        {
            ThighInteractable thigh = interactive as ThighInteractable;
            ActionsReturnClass newResult = thigh.GetActions();
            __result = newResult;
            return false;
        }
        if (interactive is HostageInteractable)
        {
            HostageInteractable hostage = interactive as HostageInteractable;
            ActionsReturnClass newResult = hostage.GetActions();
            __result = newResult;
            return false;
        }
        if (interactive is TakedownInteractable)
        {
            TakedownInteractable takedown = interactive as TakedownInteractable;
            ActionsReturnClass newResult = takedown.GetActions();
            __result = newResult;
            return false;
        }
        
        return true;
    }
}