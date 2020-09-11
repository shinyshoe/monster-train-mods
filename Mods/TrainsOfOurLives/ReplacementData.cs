using System;
using System.Collections.Generic;
using ShinyShoe.Audio;

public static class ReplacementData
{
    public static Dictionary<string, string> CueReplacements = new Dictionary<string, string>()
    {
        { SoundCueNames.StartGame, "test.wav" }
    };
}

public class SoundReplacement
{
    public string sourceCueName;
    public CoreSoundEffectData.SoundCueDefinition replacementDefinition;

    public SoundReplacement(string sourceCue, CoreSoundEffectData.SoundCueDefinition soundDefinition)
    {
        this.sourceCueName = sourceCue;
        this.replacementDefinition = soundDefinition;
    }
}
