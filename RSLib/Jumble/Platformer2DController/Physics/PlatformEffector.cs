namespace RSLib.Jumble.Platformer2D
{
    using UnityEngine;

    public class PlatformEffector : MonoBehaviour
    {
        [SerializeField] private bool _blockDown = false;

        public bool BlockDown => _blockDown;
    }
}