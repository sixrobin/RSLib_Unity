namespace RSLib.Audio
{
    using UnityEngine;

    public class AudioManager : Framework.Singleton<AudioManager>
    {
        [SerializeField] private AudioSource _source = null;

        public static void PlayNextPlaylistSound(AudioPlaylist playlist)
        {
            Instance._source.PlayOneShot(playlist.GetNextClipDatas().Clip);
        }
    }
}