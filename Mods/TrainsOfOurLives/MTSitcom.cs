using BepInEx;
using HarmonyLib;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShinyShoe.Audio;
using UnityEngine.Networking;

namespace TrainsOfOurLives
{
    [BepInPlugin("com.shinyshoe.mtsitcom", "Trains of Our Lives", "1.0.0.0")]
    public class MTSitcom : BaseUnityPlugin
    {
        public static List<SoundReplacement> Replacements;

        void Awake()
        {
            StartCoroutine(PopulateSoundReplacements());

            var harmony = new Harmony("com.shinyshoe.mtsitcom");
            harmony.PatchAll();
        }

        private IEnumerator PopulateSoundReplacements()
        {
            Replacements = new List<SoundReplacement>();
            foreach (KeyValuePair<string, string> kvp in ReplacementData.CueReplacements)
            {
                yield return CreateReplacementDefinition(kvp.Key, kvp.Value);
            }
        }

        private IEnumerator CreateReplacementDefinition(string cueName, string fileName)
        {
            CoreSoundEffectData.SoundCueDefinition definition = new CoreSoundEffectData.SoundCueDefinition();
            definition.Name = cueName;
            definition.VolumeMin = 1f;
            definition.VolumeMax = 1f;
            definition.PitchMin = 1f;
            definition.PitchMax = 1f;
            definition.Loop = false;
            definition.Tags = new string[] { };

            string directory = Path.Combine(Path.GetDirectoryName(Info.Location), fileName);
            directory = Path.Combine("file:///", directory);
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(directory, AudioType.WAV);
            Logger.LogInfo(directory);
            yield return www.SendWebRequest();
            definition.Clips = new AudioClip[] { DownloadHandlerAudioClip.GetContent(www) };

            Replacements.Add(new SoundReplacement(cueName, definition));
        }
    }

    [HarmonyPatch(typeof(CoreAudioSystem))]
    [HarmonyPatch("GetSoundDefinition")]
    public static class Mod_CoreAudioSystem_GetSoundDefinition
    {
        static void Postfix(ref CoreSoundEffectData.SoundCueDefinition __result, string cueName)
        {
            SoundReplacement replacement = FindReplacement(cueName);
            if (replacement != null)
            {
                __result = replacement.replacementDefinition;
            }
        }

        static SoundReplacement FindReplacement(string cueName)
        {
            foreach(SoundReplacement replacement in MTSitcom.Replacements)
            {
                if (replacement.sourceCueName == cueName)
                {
                    return replacement;
                }
            }

            return null;
        }
    }
}
