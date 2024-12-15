namespace RSLib.Unity
{
    using UnityEngine;

    /// <summary>
    /// Attach this on a gameObject with an Animator so that animation events can call its methods.
    /// </summary>
    public sealed class SharedAnimationEvents : MonoBehaviour
    {
        [ContextMenu("Disable")]
        public void Disable()
        {
            gameObject.SetActive(false);
        }

        [ContextMenu("Destroy")]
        public void Destroy()
        {
            Destroy(gameObject);
        }

        [ContextMenu("Destroy Immediate")]
        public void DestroyImmediate()
        {
            DestroyImmediate(gameObject);
        }

        public void PlayAudioClip(RSLib.Unity.Audio.ClipProvider clipProvider)
        {
            RSLib.Unity.Audio.AudioManager.PlaySound(clipProvider);
        }
        
        public void Log(string msg)
        {
            UnityEngine.Debug.Log(msg, gameObject);
        }
    }
}
