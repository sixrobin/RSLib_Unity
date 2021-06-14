namespace RSLib.Audio
{
    using UnityEngine;
    using Extensions;

    [System.Serializable]
    public class AudioClipPlayDatas : System.IComparable
    {
        [SerializeField] private AudioClip _clip = null;
        [SerializeField] private Vector2 _volumeRandomRange = Vector2.one;

        public AudioClip Clip => _clip;
        public Vector2 VolumeRandomRange => _volumeRandomRange;

        public float RandomVolume => _volumeRandomRange.Random();

        public int CompareTo(object obj)
        {
            return Clip.name.CompareTo(((AudioClipPlayDatas)obj).Clip.name);
        }
    }
}