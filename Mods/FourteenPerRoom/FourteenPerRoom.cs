using BepInEx;
using HarmonyLib;
using TMPro;

namespace FourteenPerRoom
{
    [BepInPlugin("com.shinyshoe.fourteenperroom", "FourteenPerRoom", "1.0.0.0")]
    public class FourteenPerRoom : BaseUnityPlugin
    {
        public const int MaxNumUnitsPerRoom = 14;

        void Awake()
        {
            var harmony = new Harmony("com.shinyshoe.fourteenperroom");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(BalanceData), "GetNumSpawnPointsPerFloor")]
    public static class Mod_BalanceData_GetNumSpawnPointsPerFloor
    {
        static void Postfix(ref int __result)
        {
            __result = FourteenPerRoom.MaxNumUnitsPerRoom;
        }
    }

    [HarmonyPatch(typeof(RoomCapacityIndicator), "Start")]
    public static class Mod_RoomCapacityIndicator_Start
    {
        static void Postfix(TMP_Text ___maxRoomTextElement)
        {
            ___maxRoomTextElement.text = ___maxRoomTextElement.text.Replace("7", FourteenPerRoom.MaxNumUnitsPerRoom.ToString());
        }
    }

    // Adding more units screws up the "rim lighting" (the highlighting around the edges 
    //   of the characters), so turn it off.
    [HarmonyPatch(typeof(RimLight), "IsEnabled")]
    public static class Mod_RimLight_IsEnabled
    {
        static void Postfix(ref bool __result)
        {
            __result = false;
        }
    }
}
