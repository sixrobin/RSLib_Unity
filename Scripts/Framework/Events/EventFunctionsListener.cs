namespace RSLib.Unity.Framework.Events
{
    using UnityEngine;
    using UnityEngine.Events;

    [DisallowMultipleComponent]
    public sealed class EventFunctionsListener : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _onAwake = null;
        [SerializeField]
        private UnityEvent _onEnable = null;
        [SerializeField]
        private UnityEvent _onStart = null;
        [SerializeField]
        private UnityEvent _onDisable = null;

        private void Awake()
        {
            _onAwake?.Invoke();
        }

        private void OnEnable()
        {
            _onEnable?.Invoke();
        }

        private void Start()
        {
            _onStart?.Invoke();
        }
        
        private void OnDisable()
        {
            _onDisable?.Invoke();
        }
    }
}
