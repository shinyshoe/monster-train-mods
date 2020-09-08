using BepInEx;
using HarmonyLib;

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
            if (__result == "Train Steward") __result = "Only Friend";
        }
    }

    [HarmonyPatch(typeof(CardState))]
    [HarmonyPatch("GetTitle")]
    public static class ChangeFriendCardName
    {
        static void Postfix(ref string __result)
        {
            if (__result == "Train Steward") __result = "Only Friend";
        }
    }
}
