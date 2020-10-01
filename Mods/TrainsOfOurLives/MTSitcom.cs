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
            foreach (KeyValuePair<string, string> kvp in ReplacementData.GlobalCueReplacements)
            {
                yield return CreateGlobalReplacementDefinition(kvp.Key, kvp.Value);
            }

            foreach (KeyValuePair<CharacterSoundCueInfo, string> kvp in ReplacementData.CharacterCueReplacements)
            {
                yield return CreateCharacterReplacementDefinition(kvp.Key, kvp.Value);
            }
        }

        private IEnumerator CreateGlobalReplacementDefinition(string cueName, string fileName)
        {
            CoreSoundEffectData.SoundCueDefinition definition = GetSoundCueDefinition(cueName);
            yield return GetAudioClip(fileName, definition);

            Replacements.Add(new GlobalSoundReplacement(cueName, definition));
        }

        private IEnumerator CreateCharacterReplacementDefinition(CharacterSoundCueInfo cueInfo, string fileName)
        {
            CoreSoundEffectData.SoundCueDefinition definition = GetSoundCueDefinition(cueInfo.sourceCueName);
            yield return GetAudioClip(fileName, definition);

            Replacements.Add(new CharacterSoundReplacement(cueInfo, definition));
        }

        private CoreSoundEffectData.SoundCueDefinition GetSoundCueDefinition(string cueName)
        {
            CoreSoundEffectData.SoundCueDefinition definition = new CoreSoundEffectData.SoundCueDefinition();
            definition.Name = cueName;
            definition.VolumeMin = 1f;
            definition.VolumeMax = 1f;
            definition.PitchMin = 1f;
            definition.PitchMax = 1f;
            definition.Loop = false;
            definition.Tags = new string[] { };

            return definition;
        }

        private IEnumerator GetAudioClip(string fileName, CoreSoundEffectData.SoundCueDefinition definition)
        {
            string directory = Path.Combine(Path.GetDirectoryName(Info.Location), "audio");
            directory = Path.Combine(directory, fileName);
            directory = Path.Combine("file:///", directory);
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(directory, AudioType.WAV);
            Logger.LogInfo(directory);
            yield return www.SendWebRequest();
            definition.Clips = new AudioClip[] { DownloadHandlerAudioClip.GetContent(www) };
        }
    }

    [HarmonyPatch(typeof(CoreAudioSystem))]
    [HarmonyPatch("GetSoundDefinition")]
    public static class Mod_CoreAudioSystem_GetSoundDefinition
    {
        static void Postfix(ref CoreSoundEffectData.SoundCueDefinition __result, string cueName, CoreSoundEffectData data)
        {
            SoundReplacement replacement = FindReplacement(cueName, data.name);
            if (replacement != null)
            {
                __result = replacement.GetReplacementSoundDefininition();
            }
        }

        static SoundReplacement FindReplacement(string cueName, string soundDataName)
        {
            foreach(SoundReplacement replacement in MTSitcom.Replacements)
            {
                if (replacement.ShouldReplace(cueName, soundDataName))
                {
                    return replacement;
                }
            }

            return null;
        }
    }
}
