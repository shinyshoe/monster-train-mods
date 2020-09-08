using BepInEx;
using HarmonyLib;
using ShinyShoe;
using UnityEngine;
using System.Collections.Generic;

namespace OnlyStewards
{
    [BepInPlugin("com.shinyshoe.textreplacer", "TextReplacer", "1.0.0.0")]
    public class TextReplacer : BaseUnityPlugin
    {
        void Awake()
        {
            var harmony = new Harmony("com.shinyshoe.textreplacer");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(CharacterData))]
    [HarmonyPatch("GetName")]
    public static class ChangeFriendCharacterName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Little Fade") __result = "Shiny Steward";
        }
    }

    [HarmonyPatch(typeof(CardState))]
    [HarmonyPatch("GetTitle")]
    public static class ChangeFriendCardName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Little Fade") __result = "Shiny Steward";
        }
    }
}
