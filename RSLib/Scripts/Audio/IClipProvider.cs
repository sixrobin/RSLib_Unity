namespace RSLib.Audio
{
    public interface IClipProvider
    {
        UnityEngine.Audio.AudioMixerGroup MixerGroup { get; }
        float VolumeMultiplier { get; }

        AudioClipPlayDatas GetNextClipDatas();
    }
}