namespace RSLib.Audio.Demo
{
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class AudioManagerDemo : MonoBehaviour
    {
        [SerializeField] private AudioPlaylist _playlist = null;

        [SerializeField] private AudioPlaylist[] _musicProviders = null;

        private int _nextMusicIndex;

        public void PlayNextPlaylistSound()
        {
            AudioManager.PlayNextPlaylistSound(_playlist);
        }

        public void PlayNextMusic()
        {
            AudioManager.PlayMusic(_musicProviders[_nextMusicIndex++ % _musicProviders.Length], MusicTransitionsDatas.Default);
        }

        public void PlayNextMusicInstantaneous()
        {
            AudioManager.PlayMusic(_musicProviders[_nextMusicIndex++ % _musicProviders.Length], MusicTransitionsDatas.Instantaneous);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AudioManagerDemo))]
    public class AudioManagerDemoEditor : Editor
    {
        private AudioManagerDemo _audioManagerDemo;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(5);

            if (GUILayout.Button("Play Next Playlist Sound"))
                _audioManagerDemo.PlayNextPlaylistSound();

            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("Play Next Track"))
                    _audioManagerDemo.PlayNextMusic();

                if (GUILayout.Button("Play Next Track Instantaneous"))
                    _audioManagerDemo.PlayNextMusicInstantaneous();
            }
        }

        private void OnEnable()
        {
            _audioManagerDemo = (AudioManagerDemo)target;
        }
    }
#endif
}