namespace RSLib.Unity
{
    #if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
    #endif
    using UnityEngine;
    using UnityEngine.Events;

    public sealed class AnimationEventCallback : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _callback;

        /// <summary>
        /// Method calling a simple UnityEvent.
        /// Designed to be called from an animation clip event.
        /// </summary>
        #if ODIN_INSPECTOR
        [Button]
        #endif
        public void InvokeCallback()
        {
            this._callback?.Invoke();
        }
    }
}