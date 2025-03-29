using System.Reflection;
using CQB.Builders;
using CQB.Components;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using UnityEngine;

namespace CQB.Patches
{
    public class PatchPlayerCreate : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Property(typeof(Player), nameof(Player.PlayerId)).GetSetMethod();
        }

        [PatchPostfix]
        static void Postfix(ref Player __instance)
        {
            // Try to only apply to real players, not bots.
            if (__instance.gameObject.name.Contains("Bot"))
            {
                InteractableInit(ref __instance, true);
                return;
            };
            InteractableInit(ref __instance, false);
        }

        private static void InteractableInit(ref Player __instance, bool bot)
        {
            Transform backTransform = __instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0)
                .GetChild(2).GetChild(4).GetChild(0).GetChild(11);
            Transform leftShoulderTransform = __instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0)
                .GetChild(2).GetChild(4).GetChild(0).GetChild(10);
            Transform rightShoulderTransform = __instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0)
                .GetChild(2).GetChild(4).GetChild(0).GetChild(9);
            Transform leftThighTransform =
                __instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(0);
            Transform leftFootTransform = __instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0);
            Transform rightFootTransform = __instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(0);
            Transform rightThighTransform =
                __instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(1);
            if (bot)
            {
                InteractableBuilder<HostageInteractable>.Build(
                    "CQB -- Back Hostage",
                    new Vector3(0.0f, 0.0f, 0.0f),
                    new Vector3(0.3f, 0.3f, 0.3f),
                    backTransform,
                    false);
                InteractableBuilder<HostageInteractable>.Build(
                    "CQB -- Left Shoulder Hostage",
                    new Vector3(-0.06f, -0.12f, 0.06f),
                    new Vector3(0.4f, 0.4f, 0.4f),
                    leftShoulderTransform,
                    false);
                InteractableBuilder<HostageInteractable>.Build(
                    "CQB -- Right Shoulder Hostage",
                    new Vector3(-0.06f, -0.12f, -0.04f),
                    new Vector3(0.4f, 0.4f, 0.4f),
                    rightShoulderTransform,
                    false);
                InteractableBuilder<TakedownInteractable>.Build(
                    "CQB -- Left Thigh Takedown",
                    new Vector3(-0.2f, -0.15f, 0.0f),
                    new Vector3(0.4f, 0.3f, 0.3f),
                    leftThighTransform,
                    false);
                InteractableBuilder<TakedownInteractable>.Build(
                    "CQB -- Right Thigh Takedown",
                    new Vector3(-0.2f, -0.15f, 0.0f),
                    new Vector3(0.4f, 0.3f, 0.3f),
                    rightThighTransform,
                    false);
            }
            else
            { 
                InteractableBuilder<ShoulderInteractable>.Build(
                "CQB -- Left Shoulder",
                new Vector3(-0.06f, -0.12f, 0.06f),
                new Vector3(0.3f, 0.3f, 0.3f),
                leftShoulderTransform,
                false);
                InteractableBuilder<ShoulderInteractable>.Build(
                "CQB -- Right Shoulder",
                new Vector3(-0.06f, -0.12f, -0.04f),
                new Vector3(0.4f, 0.3f, 0.3f),
                rightShoulderTransform,
                false);
            
                InteractableBuilder<ThighInteractable>.Build(
                "CQB -- Left Thigh",
                new Vector3(-0.2f, -0.15f, 0.0f),
                new Vector3(0.3f, 0.3f, 0.3f),
                leftThighTransform,
                false);
                InteractableBuilder<ThighInteractable>.Build(
                "CQB -- Right Thigh",
                new Vector3(-0.2f, -0.15f, 0.0f),
                new Vector3(0.3f, 0.3f, 0.3f),
                rightThighTransform,
                false);
                        
                GameObject leftFoot = GameObject.CreatePrimitive(PrimitiveType.Cube);
                leftFoot.name = "Left Foot IK Target";
                // Disable collider to stop self interaction, and remove renderer
                leftFoot.GetComponent<BoxCollider>().enabled = false;
                leftFoot.GetComponent<MeshRenderer>().enabled = false;            
                // Set closer to IK target we want
                leftFoot.transform.position = new Vector3(0.9f, -0.45f, 0.3f);
                leftFoot.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                // Get Right Foot IK Target
                leftFoot.transform.SetParent(leftFootTransform, false);
                leftFoot.SetActive(true);
                        
                GameObject rightFoot = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rightFoot.name = "Right Foot IK Target";
                // Disable collider to stop self interaction, and remove renderer
                rightFoot.GetComponent<BoxCollider>().enabled = false;
                rightFoot.GetComponent<MeshRenderer>().enabled = false;            
                // Set closer to IK target we want
                rightFoot.transform.position = new  Vector3(0.9f, -0.45f, -0.3f);
                rightFoot.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                // Get Right Foot IK Target
                rightFoot.transform.SetParent(rightFootTransform, false);
                rightFoot.SetActive(true);
            }
        }
    }
}