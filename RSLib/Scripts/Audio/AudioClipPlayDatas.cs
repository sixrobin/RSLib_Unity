namespace RSLib.Audio
{
    using UnityEngine;
    using Extensions;

    [System.Serializable]
    public class AudioClipPlayDatas : System.IComparable
    {
        [SerializeField] private AudioClip _clip = null;
        [SerializeField] private Vector2 _volumeRandomRange = Vector2.one;
        [SerializeField, Range(0f, 1f)] private float _pitchVariation = 0f;

        public AudioClip Clip => _clip;
        public float RandomVolume => _volumeRandomRange.Random();
        public float PitchVariation => new Vector2(-_pitchVariation, _pitchVariation).Random();

        public int CompareTo(object obj)
        {
            return Clip.name.CompareTo(((AudioClipPlayDatas)obj).Clip.name);
        }
    }
}