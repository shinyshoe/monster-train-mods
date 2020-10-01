using System;
using System.Collections.Generic;
using ShinyShoe.Audio;

public static class ReplacementAudioCues
{
    public const string Applause_Big =      "applause_big.wav";
    public const string Applause_Medium =   "applause_medium.wav";
    public const string Applause_Small =    "applause_small.wav";
    public const string AwwClap =           "aww_clap.wav";
    public const string BassSlap =          "bass_slap.wav";
    public const string BassSlap_Kitsch =   "bass_slap_kitsch.wav";
    public const string Victory =           "victory.wav";
    public const string Boo =               "boo.wav";
    public const string Laugh_Big =         "laugh_big.wav";
    public const string Laugh_Medium =      "laugh_medium.wav";
    public const string Laugh_Small =       "laugh_small.wav";
    public const string Ouch =              "ouch.wav";
    public const string Cheer =             "cheer.wav";
}

public static class ReplacementData
{
    public static Dictionary<string, string> GlobalCueReplacements = new Dictionary<string, string>()
    {
        { SoundCueNames.StartGame,          ReplacementAudioCues.BassSlap_Kitsch },
        { SoundCueNames.BattleWinStinger,   ReplacementAudioCues.Victory },
        { SoundCueNames.BattleLoseStinger,  ReplacementAudioCues.BassSlap },
        { SoundCueNames.CombatTrainDamage,  ReplacementAudioCues.Ouch },
        { "Node_Battle",                    ReplacementAudioCues.Applause_Small },
        { "Node_BossBattle",                ReplacementAudioCues.Applause_Medium },
        { "Seraph_Intro",                   ReplacementAudioCues.Laugh_Big },
    };

    public static Dictionary<CharacterSoundCueInfo, string> CharacterCueReplacements = new Dictionary<CharacterSoundCueInfo, string>()
    {
        { new CharacterSoundCueInfo(
            SoundCueNames.CharacterCombatDeath, 
            new List<string>(new string[] {
                "BasicT1SoundEffects",
                "BasicT2SoundEffects",
                "BasicT3SoundEffects"
            })),
                                                                        ReplacementAudioCues.Applause_Small },



        { new CharacterSoundCueInfo(
            SoundCueNames.CharacterCombatSpawn,
            new List<string>(new string[] {
                "Boss_Level3ArmorSoundEffects",
                "Boss_Level3TalosArmorOnKillSoundEffects",
                "Boss_Level6ArchusTrapATrapBSoundEffects",
                "Boss_Level6ArmorSoundEffects",
                "Boss_Level8PurifySoundEffects",
                "Boss_Level1JunkerSoundEffects",
                "Boss_Level1SoundEffects",
                "Boss_Level2SoundEffects",
                "Boss_Level4SoundEffects",
                "Boss_Level5SoundEffects",
                "Boss_Level7SoundEffects",
            })),

                                                                         ReplacementAudioCues.Boo },

        { new CharacterSoundCueInfo(
            SoundCueNames.CharacterCombatDeath,
            new List<string>(new string[] {
                "Boss_Level3ArmorSoundEffects",
                "Boss_Level3TalosArmorOnKillSoundEffects",
                "Boss_Level6ArchusTrapATrapBSoundEffects",
                "Boss_Level6ArmorSoundEffects",
                "Boss_Level8PurifySoundEffects",
                "Boss_Level1JunkerSoundEffects",
                "Boss_Level1SoundEffects",
                "Boss_Level2SoundEffects",
                "Boss_Level4SoundEffects",
                "Boss_Level5SoundEffects",
                "Boss_Level7SoundEffects",
            })),

                                                                         ReplacementAudioCues.Applause_Big },

         { new CharacterSoundCueInfo(
            SoundCueNames.CharacterCombatSpawn,
            new List<string>(new string[] {
                "MonsterAggressiveImpSoundEffects",
                "MonsterEverythingImpSoundEffects",
                "MonsterExpandingImpSoundEffects",
                "MonsterEnlightenedInferySoundEffects",
                "MonsterEmberedInferySoundEffects",
                "MonsterEnsnaringInferySoundEffects",
                "MonsterUnstableInferySoundEffects",
                "MonsterQueensImpSoundEffects",
                "MonsterJunkyMorselSoundEffects",
                "MonsterDivineMorselSoundEffects"
            })),

                                                                         ReplacementAudioCues.AwwClap },

          { new CharacterSoundCueInfo(
            SoundCueNames.CharacterCombatSpawn,
            new List<string>(new string[] {
                "MonsterFirstWaxerSoundEffects",
                "MonsterChampionAwokenRootSoundEffects",
                "MonsterChampionHellhornedShardtailSoundEffects",
                "MonsterChampionUmbraPrimordiumSoundEffects",
                "MonsterStygianChampionMartyrSoundEffects",
                "MonsterDanteSoundEffects",
                "MonsterHollowChampionSoundEffects",
                "MonsterHornbreakerChampionSoundEffects",
                "MonsterInfernusChampionSoundEffects",
                "MonsterStygianChampionSoundEffects",
                "MonsterHarvestAttackSoundEffects"
            })),

                                                                         ReplacementAudioCues.Cheer },

          { new CharacterSoundCueInfo(
            SoundCueNames.CharacterCombatDeath,
            new List<string>(new string[] {
                "MonsterFirstWaxerSoundEffects",
                "MonsterChampionAwokenRootSoundEffects",
                "MonsterChampionHellhornedShardtailSoundEffects",
                "MonsterChampionUmbraPrimordiumSoundEffects",
                "MonsterStygianChampionMartyrSoundEffects",
                "MonsterDanteSoundEffects",
                "MonsterHollowChampionSoundEffects",
                "MonsterHornbreakerChampionSoundEffects",
                "MonsterInfernusChampionSoundEffects",
                "MonsterStygianChampionSoundEffects",
                "MonsterHarvestAttackSoundEffects"
            })),

                                                                         ReplacementAudioCues.Ouch },
    };
}

public class GlobalSoundReplacement : SoundReplacement
{
    public string sourceCueName;
    public CoreSoundEffectData.SoundCueDefinition replacementDefinition;

    public GlobalSoundReplacement(string sourceCue, CoreSoundEffectData.SoundCueDefinition soundDefinition)
    {
        this.sourceCueName = sourceCue;
        this.replacementDefinition = soundDefinition;
    }

    public CoreSoundEffectData.SoundCueDefinition GetReplacementSoundDefininition()
    {
        return replacementDefinition;
    }

    public bool ShouldReplace(string cueName, string soundDataName)
    {
        return cueName == sourceCueName;
    }
}

public class CharacterSoundReplacement : SoundReplacement
{
    public CharacterSoundCueInfo sourceCueInfo;
    public CoreSoundEffectData.SoundCueDefinition replacementDefinition;

    public CharacterSoundReplacement(CharacterSoundCueInfo sourceCueInfo, CoreSoundEffectData.SoundCueDefinition soundDefinition)
    {
        this.sourceCueInfo = sourceCueInfo;
        this.replacementDefinition = soundDefinition;
    }

    public CoreSoundEffectData.SoundCueDefinition GetReplacementSoundDefininition()
    {
        return replacementDefinition;
    }

    public bool ShouldReplace(string cueName, string soundDataName)
    {
        return sourceCueInfo.DoCuesMatch(cueName, soundDataName);
    }
}

public struct CharacterSoundCueInfo
{
    public string sourceCueName;
    public List<string> sourceSoundDataNames;

    public CharacterSoundCueInfo(string sourceCueName, List<string> sourceSoundDataNames)
    {
        this.sourceCueName = sourceCueName;
        this.sourceSoundDataNames = sourceSoundDataNames;
    }

    public bool DoCuesMatch(string cueName, string soundDataName)
    {
        bool cueNamesMatch = cueName == sourceCueName;
        bool dataNamesMatch = sourceSoundDataNames != null && sourceSoundDataNames.Contains(soundDataName);

        return cueNamesMatch && dataNamesMatch;
    }
}
