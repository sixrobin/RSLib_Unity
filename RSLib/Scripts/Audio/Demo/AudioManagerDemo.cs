namespace RSLib.Audio.Demo
{
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class AudioManagerDemo : MonoBehaviour
    {
        [SerializeField] private AudioPlaylist _playlist = null;

        public void PlayNextPlaylistSound()
        {
            AudioManager.PlayNextPlaylistSound(_playlist);
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
        }

        private void OnEnable()
        {
            _audioManagerDemo = (AudioManagerDemo)target;
        }
    }
#endif
}