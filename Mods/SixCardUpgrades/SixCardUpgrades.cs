using BepInEx;
using HarmonyLib;

namespace SixCardUpgrades
{
    [BepInPlugin("com.shinyshoe.sixcardupgrades", "SixCardUpgrades", "1.0.0.0")]
    public class SixCardUpgrades : BaseUnityPlugin
    {
        public const int NumCardUpgrades = 6;

        void Awake()
        {
            var harmony = new Harmony("com.shinyshoe.sixcardupgrades");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(BalanceData), "GetUpgradeSlots")]
    public static class Mod_BalanceData_GetUpgradeSlots
    {
        static void Prefix(ref int ___unitUpgradeSlots, ref int ___spellUpgradeSlots)
        {
            ___unitUpgradeSlots = SixCardUpgrades.NumCardUpgrades;
            ___spellUpgradeSlots = SixCardUpgrades.NumCardUpgrades;
        }
    }
}
