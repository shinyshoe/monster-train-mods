using System;
using ShinyShoe.Audio;

public interface SoundReplacement
{
    bool ShouldReplace(string cueName, string soundDataName);
    CoreSoundEffectData.SoundCueDefinition GetReplacementSoundDefininition();
}
