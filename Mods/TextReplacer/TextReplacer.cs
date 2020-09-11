using BepInEx;
using HarmonyLib;
using I2.Loc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TextReplacer
{
    [BepInPlugin("com.shinyshoe.textreplacer", "Text Replacer", "1.0.0.0")]
    public class TextReplacer : BaseUnityPlugin
    {
        private readonly string ReplacementFileName = "Replacements.csv";

        public static List<TextReplacement> Replacements;

        void Awake()
        {
            var directory = Path.GetDirectoryName(Info.Location);
            var path = Path.Combine(directory, ReplacementFileName);
            Replacements = File.ReadAllLines(path)
                .Select(r => TextReplacement.FromCSV(r))
                .ToList();

            var harmony = new Harmony("com.shinyshoe.textreplacer");
            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(LocalizationManager))]
    [HarmonyPatch("GetTranslation")]
    public static class Mod_LocationlizationManager_GetTranslation
    {
        static void Postfix(ref string __result)
        {
            foreach(TextReplacement replacement in TextReplacer.Replacements)
            {
                __result = __result.Replace(replacement.sourceString, replacement.targetString);
            }
        }
    }
}
