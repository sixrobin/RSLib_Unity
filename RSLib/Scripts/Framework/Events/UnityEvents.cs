namespace RSLib.Framework.Events
{
    using UnityEngine.Events;
    
    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [System.Serializable]
    public class StringEvent : UnityEvent<string> { }

    [System.Serializable]
    public class ColliderEvent : UnityEvent<UnityEngine.Collider> { }

    [System.Serializable]
    public class Collider2DEvent : UnityEvent<UnityEngine.Collider2D> { }

    [System.Serializable]
    public class CollisionEvent : UnityEvent<UnityEngine.Collision> { }

    [System.Serializable]
    public class Collision2DEvent : UnityEvent<UnityEngine.Collision2D> { }

    [System.Serializable]
    public class Vector2Event : UnityEvent<UnityEngine.Vector2> { }

    [System.Serializable]
    public class Vector2IntEvent : UnityEvent<UnityEngine.Vector2Int> { }

    [System.Serializable]
    public class Vector3Event : UnityEvent<UnityEngine.Vector3> { }

    [System.Serializable]
    public class Vector3IntEvent : UnityEvent<UnityEngine.Vector3Int> { }

    [System.Serializable]
    public class QuaternionEvent : UnityEvent<UnityEngine.Quaternion> { }

    [System.Serializable]
    public class ColorEvent : UnityEvent<UnityEngine.Color> { }
}