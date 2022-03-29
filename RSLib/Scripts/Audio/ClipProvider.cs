namespace RSLib.Audio
{
    public abstract class ClipProvider : UnityEngine.ScriptableObject, IClipProvider
    {
        [UnityEngine.SerializeField] private UnityEngine.Audio.AudioMixerGroup _mixerGroup = null;
        [UnityEngine.SerializeField, UnityEngine.Range(0f, 1f)] private float _volumeMultiplier = 1f;

        public UnityEngine.Audio.AudioMixerGroup MixerGroup => _mixerGroup;
        public float VolumeMultiplier => _volumeMultiplier;

        public virtual void Init() { }
        public abstract AudioClipPlayDatas GetNextClipDatas();
    }
}