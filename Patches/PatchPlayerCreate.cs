using System.Reflection;
using EFT;
using HarmonyLib;
using ShoulderCQB.Components;
using SPT.Reflection.Patching;
using UnityEngine;

namespace ShoulderCQB.Patches
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
            if (__instance.gameObject.name.Contains("Bot")) return;
            
            GameObject leftShoulder = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftShoulder.name = "CQB -- Left Shoulder";
            leftShoulder.AddComponent<ShoulderInteractable>();
            // Disable collider to stop self interaction, and remove renderer
            leftShoulder.GetComponent<BoxCollider>().enabled = false;
            leftShoulder.GetComponent<MeshRenderer>().enabled = false;            
            // Set closer to center of the shoulder
            leftShoulder.transform.position = new  Vector3(-0.06f, -0.12f, 0.06f);
            leftShoulder.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            // Get Left Shoulder
            leftShoulder.transform.SetParent(__instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(2).GetChild(4).GetChild(0).GetChild(10), false);
            leftShoulder.SetActive(true);
            
            GameObject leftFoot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftFoot.name = "Left Foot IK Target";
            // Disable collider to stop self interaction
            leftFoot.GetComponent<BoxCollider>().enabled = false;
            leftFoot.GetComponent<MeshRenderer>().enabled = false;            
            // Set closer to IK target we want
            leftFoot.transform.position = new  Vector3(0.9f, -0.45f, 0.3f);
            leftFoot.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            // Get Left Foot IK Target
            leftFoot.transform.SetParent(__instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0), false);
            leftFoot.SetActive(true);
            
            GameObject leftThigh = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leftThigh.name = "CQB -- Left Thigh";
            leftThigh.AddComponent<ThighInteractable>();
            // Disable collider to stop self interaction, and remove renderer
            leftThigh.GetComponent<BoxCollider>().enabled = false;
            leftThigh.GetComponent<MeshRenderer>().enabled = true;            
            // Set closer to center of the thigh
            leftThigh.transform.position = new  Vector3(-0.2f, -0.15f, 0.0f);
            leftThigh.transform.localScale = new Vector3(0.4f, 0.3f, 0.3f);
            // Get Left Thigh
            leftThigh.transform.SetParent(__instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(0), false);
            leftThigh.SetActive(true);
            
            GameObject rightShoulder = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightShoulder.name = "CQB -- Right Shoulder";
            rightShoulder.AddComponent<ShoulderInteractable>();
            // Disable collider to stop self interaction, and remove renderer
            rightShoulder.GetComponent<BoxCollider>().enabled = false;
            rightShoulder.GetComponent<MeshRenderer>().enabled = false;            
            // Set closer to center of the shoulder
            rightShoulder.transform.position = new  Vector3(-0.06f, -0.12f, -0.04f);
            rightShoulder.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            // Get Right Shoulder
            rightShoulder.transform.SetParent(__instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(2).GetChild(4).GetChild(0).GetChild(9), false);
            rightShoulder.SetActive(true);
            
            GameObject rightFoot = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightFoot.name = "Right Foot IK Target";
            // Disable collider to stop self interaction, and remove renderer
            rightFoot.GetComponent<BoxCollider>().enabled = false;
            rightFoot.GetComponent<MeshRenderer>().enabled = false;            
            // Set closer to IK target we want
            rightFoot.transform.position = new  Vector3(0.9f, -0.45f, -0.3f);
            rightFoot.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            // Get Right Foot IK Target
            rightFoot.transform.SetParent(__instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0), false);
            rightFoot.SetActive(true);
            
            GameObject rightThigh = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rightThigh.name = "CQB -- Right Thigh";
            rightThigh.AddComponent<ThighInteractable>();
            // Disable collider to stop self interaction, and remove renderer
            rightThigh.GetComponent<BoxCollider>().enabled = false;
            rightThigh.GetComponent<MeshRenderer>().enabled = true;            
            // Set closer to center of the thigh
            rightThigh.transform.position = new  Vector3(-0.2f, -0.15f, 0.0f);
            rightThigh.transform.localScale = new Vector3(0.4f, 0.3f, 0.3f);
            // Get Right Thigh
            rightThigh.transform.SetParent(__instance.gameObject.transform.GetChild(0).GetChild(4).GetChild(0).GetChild(1), false);
            rightThigh.SetActive(true);
        }
    }
}