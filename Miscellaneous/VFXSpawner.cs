namespace RSLib.Unity
{
    using System.Linq;
    using UnityEngine;

    /// <summary>
    /// Prototype class used to store VFX prefabs that can be instantiated using an ID.
    /// This class should be used to quickly prototype games but is definitely not a solid VFX handler implementation.
    /// </summary>
    public class VFXSpawner : RSLib.Unity.Framework.Singleton<VFXSpawner>
    {
        [System.Serializable]
        public struct VFX
        {
            public string ID;
            public GameObject Prefab;
        }

        [SerializeField]
        private VFX[] _vfxCollection = null;

        private static string[] IDs => Instance._vfxCollection.Select(o => o.ID).ToArray();

        private static bool TryGetVFX(string id, out VFX vfx)
        {
            for (int i = 0; i < Instance._vfxCollection.Length; ++i)
            {
                if (Instance._vfxCollection[i].ID != id)
                {
                    continue;
                }

                vfx = Instance._vfxCollection[i];
                return true;
            }

            vfx = new VFX();
            return false;
        }

        /// <summary>
        /// Instantiates a VFX prefab based on an ID and optional parameters.
        /// </summary>
        /// <param name="id">VFX ID.</param>
        /// <param name="position">VFX world position.</param>
        /// <param name="parent">VFX parent (optional).</param>
        /// <param name="forward">VFX forward vector (optional, cannot work with rotation parameter).</param>
        /// <param name="rotation">VFX rotation (optional, cannot work with forward parameter).</param>
        /// <param name="localOffset">Offset applied to VFX position right after it has been rotated (optional).</param>
        /// <param name="scale">Scale applied to original VFX scale (optional).</param>
        /// <returns>Instantiated VFX.</returns>
        public static GameObject SpawnVFX(string id,
            Vector3 position,
            Transform parent = null,
            Vector3? forward = null,
            Quaternion? rotation = null,
            Vector3? localOffset = null,
            Vector3? scale = null)
        {
            if (!Exists())
            {
                Instance.LogWarning($"Trying to spawn VFX {id} but no instance of {nameof(VFXSpawner)} has been found in the scene!");
                return null;
            }

            if (!TryGetVFX(id, out VFX vfx))
            {
                Instance.LogWarning($"Trying to spawn VFX {id} but no VFX with this ID has been found! Available IDs are: {string.Join(", ", IDs)}.");
                return null;
            }

            if (rotation.HasValue && forward.HasValue)
                Instance.LogWarning($"Trying to spawn VFX {vfx.ID} with both a forward axis and a rotation parameters. Prioritizing rotation.");

            Transform instance = Instantiate(vfx.Prefab, parent).transform;
            instance.position = position;

            if (rotation.HasValue)
                instance.rotation = rotation.Value;
            else if (forward.HasValue)
                instance.forward = forward.Value;

            if (localOffset.HasValue)
                instance.Translate(localOffset.Value, Space.Self);

            if (scale.HasValue)
            {
                Vector3 localScale = instance.localScale;
                localScale.x *= scale.Value.x;
                localScale.y *= scale.Value.y;
                localScale.z *= scale.Value.z;
                instance.localScale = localScale;
            }

            return instance.gameObject;
        }
    }
}