using BepInEx;
using HarmonyLib;

namespace RestartBattle
{
    [BepInPlugin("com.shinyshoe.restartbattle", "RestartBattle", "1.0.0.0")]
    public class RestartBattle : BaseUnityPlugin
    {
        private void Awake()
        {
            var harmony = new Harmony("com.shinyshoe.restartbattle");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(BattleHud), "Start")]
    public static class Mod_BattleHud_Start
    {
        // When the BattleHud starts, create game objects under it that make up the "restart battle" button
        private static void Postfix(BattleHud __instance)
        {
            RestartBattleButton.Create(__instance);
        }
    }
}
