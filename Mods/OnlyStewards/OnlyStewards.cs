using BepInEx;
using HarmonyLib;
using ShinyShoe;
using UnityEngine;
using System.Collections.Generic;

namespace OnlyStewards
{
    [BepInPlugin("com.shinyshoe.onlystewards", "OnlyStewards", "1.0.0.0")]
    public class OnlyStewards : BaseUnityPlugin
    {
        void Awake()
        {
            var harmony = new Harmony("com.shinyshoe.onlystewards");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(CharacterData))]
    [HarmonyPatch("GetName")]
    public static class ChangeFriendCharacterName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Train Steward") __result = "Shiny Steward";
        }
    }

    [HarmonyPatch(typeof(CardState))]
    [HarmonyPatch("GetTitle")]
    public static class ChangeFriendCardName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Train Steward") __result = "Shiny Steward";
        }
    }

    [HarmonyPatch(typeof(CharacterUI))]
    [HarmonyPatch("Setup")]
    public static class Mod_CharacterUI_Setup
    {
        static void Postfix(ref string __debugName, ref CharacterUIMeshBase __characterMesh, ref Transform __googlyEyes, ref CharacterState __characterState, ref Vector3 __googlyEyesOffset, ref int __googlyEyesSortingOrderTweak)
        {
            if (__debugName.StartsWith("Character_TrainSteward"))
            {
                (__characterMesh as CharacterUIMeshSpine).OrNull()?.AttachToBone(__googlyEyes, VfxAtLoc.Location.BoneStatusEffectSlot1);
                foreach (var googlyEye in __googlyEyes.GetComponentsInChildren<GooglyEyes>())
                {
                    googlyEye.SetGravityBias(__characterState.GetTeamType());
                    googlyEye.BringLayersForward(__googlyEyesOffset, __googlyEyesSortingOrderTweak);
                }

                __googlyEyes.gameObject.SetActive(true);
            }
        }
    }
}
