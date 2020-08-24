using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace HellHornedFriends
{
    [HarmonyPatch(typeof(CharacterData))]
    [HarmonyPatch("GetName")]
    public static class ChangeFriendCharacterName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Alpha Fiend") __result = "Alpha Friend";
            else if (__result == "Demon Fiend") __result = "Demon Friend";
        }
    }

    [HarmonyPatch(typeof(CardState))]
    [HarmonyPatch("GetTitle")]
    public static class ChangeFriendCardName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Alpha Fiend") __result = "Alpha Friend";
            else if (__result == "Demon Fiend") __result = "Demon Friend";
        }
    }

}
