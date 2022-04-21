namespace RSLib.Audio
{
    using RSLib.Maths;
    using UnityEngine;
    using UnityEngine.Audio;

    public class AudioManager : Framework.Singleton<AudioManager>
    {
        [System.Serializable]
        private class SFXPlayer
        {
            [SerializeField, Min(2)] private int _audioSourcesCount = 2;
            [SerializeField] private AudioMixerGroup _mixerGroup = null;

            public int AudioSourcesCount => _audioSourcesCount;
            public AudioMixerGroup MixerGroup => _mixerGroup;
        }

        private class RuntimeSFXPlayer
        {
            private int _nextSourceIndex = 0;
            private AudioSource[] _sources;

            public RuntimeSFXPlayer(int sourcesCount, AudioMixerGroup mixerGroup, Transform sourcesContainer)
            {
                _sources = new AudioSource[sourcesCount];
                for (int i = 0; i < sourcesCount; ++i)
                    _sources[i] = CreateAudioSource($"SFX Source {i}", sourcesContainer, mixerGroup);
            }

            public AudioSource GetNextSource()
            {
                return _sources[_nextSourceIndex++ % _sources.Length];
            }
        }

        [SerializeField] private SFXPlayer[] _sfxPlayers = null;
        [SerializeField] private AudioMixerGroup _musicMixerGroup = null;

        private static System.Collections.Generic.Dictionary<AudioMixerGroup, RuntimeSFXPlayer> s_sfxPlayersDict;
        private static AudioSource[] s_musicSources;
        private static int s_nextMusicIndex;

        private static System.Collections.IEnumerator s_musicFadeCoroutine;

        /// <summary>
        /// Remaps a value from [0.0001f, 1f] to [-80f, 0f], to adjust a percentage to the decibels scaling.
        /// </summary>
        /// <param name="value">Value to convert from percentage to decibels scale.</param>
        /// <returns>Decibels scaled value.</returns>
        public static float RemapToDecibels(float value)
        {
            return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        }

        public static void PlaySound(IClipProvider clipProvider)
        {
            if (!Exists())
            {
                Instance.LogWarning($"Trying to play a sound while no {nameof(AudioManager)} instance exists!");
                return;
            }

            if (clipProvider == null)
            {
                Instance.LogWarning($"Trying to play a sound using a null {nameof(IClipProvider)} reference!");
                return;
            }
            
            AudioSource source = GetSFXSource(clipProvider);
            AudioClipPlayDatas clipData = clipProvider.GetNextClipData();

            source.clip = clipData.Clip;
            source.volume = clipData.RandomVolume * clipProvider.VolumeMultiplier;
            source.pitch = 1f + clipData.PitchVariation + clipProvider.PitchOffset;
            
            source.Play();
        }

        public static void PlayMusic(IClipProvider musicProvider, MusicTransitionsDatas transitionData)
        {
            if (!Exists())
            {
                Instance.LogWarning($"Trying to play a sound while no {nameof(AudioManager)} instance exists!");
                return;
            }
            
            if (musicProvider == null)
            {
                Instance.LogWarning($"Trying to play a music using a null {nameof(IClipProvider)} reference!");
                return;
            }

            if (transitionData == null)
            {
                Instance.LogWarning($"Trying to play a music using a null {nameof(transitionData)} reference, using default Instantaneous transition!");
                transitionData = MusicTransitionsDatas.Instantaneous;
            }

            AudioSource currentMusicSource = GetCurrentMusicSource();
            if (currentMusicSource.isPlaying && currentMusicSource.clip == musicProvider.GetNextClipData().Clip)
            {
                Instance.Log($"Music {nameof(currentMusicSource)} is already playing, aborting.");
                return;
            }
            
            if (s_musicFadeCoroutine != null)
                Instance.StopCoroutine(s_musicFadeCoroutine);

            if (transitionData.CrossFade)
                Instance.StartCoroutine(s_musicFadeCoroutine = CrossFadeMusicCoroutine(musicProvider, transitionData));
            else
                Instance.StartCoroutine(s_musicFadeCoroutine = FadeMusicCoroutine(musicProvider, transitionData));
        }

        private static System.Collections.IEnumerator CrossFadeMusicCoroutine(IClipProvider musicProvider, MusicTransitionsDatas transitionData)
        {
            AudioSource prev = GetCurrentMusicSource();
            AudioSource next = GetNextMusicSource();
            AudioClipPlayDatas clipData = musicProvider.GetNextClipData();

            float prevVol = prev.volume;
            float nextVol = clipData.RandomVolume;

            next.clip = clipData.Clip;
            next.pitch = 1f + clipData.PitchVariation;
            next.Play();

            for (float t = 0f; t <= 1f; t += Time.deltaTime / transitionData.Duration)
            {
                prev.volume = (1f - t).Ease(transitionData.CurveIn) * prevVol;
                next.volume = t.Ease(transitionData.CurveOut) * nextVol;
                yield return null;
            }

            next.volume = nextVol;
            prev.volume = 0f;
            prev.Stop();
        }

        private static System.Collections.IEnumerator FadeMusicCoroutine(IClipProvider musicProvider, MusicTransitionsDatas transitionData)
        {
            AudioSource prev = GetCurrentMusicSource();
            AudioSource next = GetNextMusicSource();
            AudioClipPlayDatas clipData = musicProvider.GetNextClipData();

            float prevVol = prev.volume;
            float nextVol = clipData.RandomVolume;

            for (float t = prevVol; t >= 0f; t -= Time.deltaTime / transitionData.Duration * 2f)
            {
                prev.volume = t.Ease(transitionData.CurveIn);
                yield return null;
            }

            prev.volume = 0f;
            prev.Stop();

            next.clip = clipData.Clip;
            next.pitch = 1f + clipData.PitchVariation;
            next.Play();

            for (float t = 0f; t <= 1f; t += Time.deltaTime / transitionData.Duration * 2f)
            {
                next.volume = t.Ease(transitionData.CurveOut) * nextVol;
                yield return null;
            }

            next.volume = nextVol;
        }

        private static AudioSource GetSFXSource(IClipProvider clipProvider)
        {
            UnityEngine.Assertions.Assert.IsTrue(
                s_sfxPlayersDict.ContainsKey(clipProvider.MixerGroup),
                $"No AudioMixerGroup named {clipProvider.MixerGroup.name} has been registered in {Instance.GetType().Name}.");

            return s_sfxPlayersDict[clipProvider.MixerGroup].GetNextSource();
        }

        private static AudioSource GetCurrentMusicSource()
        {
            return s_musicSources[Helpers.Mod(s_nextMusicIndex - 1, s_musicSources.Length)];
        }

        private static AudioSource GetNextMusicSource()
        {
            return s_musicSources[s_nextMusicIndex++ % s_musicSources.Length];
        }

        private static void InitSFXSources()
        {
            s_sfxPlayersDict = new System.Collections.Generic.Dictionary<AudioMixerGroup, RuntimeSFXPlayer>();
            for (int i = 0; i < Instance._sfxPlayers.Length; ++i)
            {
                GameObject newPlayerParent = new GameObject($"SFX Group Player - {Instance._sfxPlayers[i].MixerGroup.name}");
                newPlayerParent.transform.SetParent(Instance.transform);
                s_sfxPlayersDict.Add(
                    Instance._sfxPlayers[i].MixerGroup,
                    new RuntimeSFXPlayer(Instance._sfxPlayers[i].AudioSourcesCount, Instance._sfxPlayers[i].MixerGroup, newPlayerParent.transform));
            }
        }

        private static void InitMusicSources()
        {
            s_musicSources = new AudioSource[2];
            GameObject newPlayerParent = new GameObject($"Music Group Player - {Instance._musicMixerGroup.name}");
            newPlayerParent.transform.SetParent(Instance.transform);

            for (int i = 0; i < 2; ++i)
                s_musicSources[i] = CreateAudioSource($"Music Source {i}", newPlayerParent.transform, Instance._musicMixerGroup);
        }

        private static AudioSource CreateAudioSource(string gameObjectName, Transform parent, AudioMixerGroup mixerGroup)
        {
            GameObject newSource = new GameObject(gameObjectName);
            newSource.transform.SetParent(parent);
            AudioSource source = newSource.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixerGroup;

            return source;
        }

        protected override void Awake()
        {
            base.Awake();
            InitSFXSources();
            InitMusicSources();
        }
    }
}