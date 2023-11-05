namespace RSLib.Editor
{
    using System.Linq;
    using UnityEngine;
    using UnityEditor;
    #if RSLIB_EDITOR_COROUTINES
    using Unity.EditorCoroutines.Editor;
    #endif

    public static class TexturesScanner
    {
        #if RSLIB_EDITOR_COROUTINES
        #region MENU ITEMS
        [MenuItem("RSLib/Texture Scan/Locate Non Multiple of 4")]
        private static void LocateNonMultipleOf4() => EditorCoroutineUtility.StartCoroutineOwnerless(LocateNonMultipleOf4Coroutine());
        
        [MenuItem("RSLib/Texture Scan/Locate Near POT")]
        private static void LocateNearPowerOf2() => EditorCoroutineUtility.StartCoroutineOwnerless(LocateNearPowerOf2Coroutine());
        
        [MenuItem("RSLib/Texture Scan/Locate High Triangles Count")]
        private static void LocateHighTrianglesCount() => EditorCoroutineUtility.StartCoroutineOwnerless(LocateHighTrianglesCountCoroutine());
        #endregion // MENU ITEMS
        #endif // RSLIB_EDITOR_COROUTINES

        #region ASSETS ACCESSOR METHODS
        private static System.Collections.IEnumerator GetTextures(System.Collections.Generic.ICollection<Texture2D> result)
        {
            string[] guids = AssetDatabase.FindAssets("t:texture");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (!path.StartsWith("Assets/")
                    || path.StartsWith("Assets/Plugins/")
                    || path.StartsWith("Assets/TextMesh Pro/"))
                {
                    continue;
                }
                
                Texture2D texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                if (texture2D == null)
                    continue;

                result.Add(texture2D);
                yield return null;
            }
        }
        
        private static System.Collections.IEnumerator GetSprites(System.Collections.Generic.ICollection<Sprite> result)
        {
            string[] guids = AssetDatabase.FindAssets("t:sprite");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (!path.StartsWith("Assets/")
                    || path.StartsWith("Assets/Plugins/")
                    || path.StartsWith("Assets/TextMesh Pro/"))
                {
                    continue;
                }
                
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);

                if (sprite == null)
                    continue;

                result.Add(sprite);
                yield return null;
            }
        }
        #endregion // ASSETS ACCESSOR METHODS

        #if RSLIB_EDITOR_COROUTINES
        #region EDITOR COROUTINES
        private static System.Collections.IEnumerator LocateNonMultipleOf4Coroutine()
        {
            System.Collections.Generic.List<Texture2D> textures = new System.Collections.Generic.List<Texture2D>();
            yield return EditorCoroutineUtility.StartCoroutineOwnerless(GetTextures(textures));

            foreach (Texture2D texture2D in textures)
            {
                float widthBelow = -1f;
                float widthAbove = -1f;
                float heightBelow = -1f;
                float heightAbove = -1f;
                
                if (texture2D.width % 4 != 0f)
                {
                    for (int i = texture2D.width - 3; i < texture2D.width; ++i)
                        if (i % 4 == 0 && i > 0)
                            widthBelow = i;
                    
                    for (int i = texture2D.width; i <= texture2D.width + 3; ++i)
                        if (i % 4 == 0)
                            widthAbove = i;
                }
                
                if (texture2D.height % 4 != 0f)
                {
                    for (int i = texture2D.height - 3; i < texture2D.height; ++i)
                        if (i % 4 == 0 && i > 0)
                            heightBelow = i;
                    
                    for (int i = texture2D.height; i <= texture2D.height + 3; ++i)
                        if (i % 4 == 0)
                            heightAbove = i;
                }
                
                // Both sides are multiple of 4.
                if (widthAbove < 0f && heightAbove < 0f)
                    continue;

                string path = AssetDatabase.GetAssetPath(texture2D);

                if (widthAbove >= 0f && heightAbove < 0f) // Invalid width, valid height.
                    Debug.Log($"{path} has an invalid <b>widths</b> (nearest %4 widths are <b>{widthBelow}</b>/<b>{widthAbove}</b>).", texture2D);
                else if (widthAbove < 0f && heightAbove >= 0f) // Valid width, invalid height.
                    Debug.Log($"{path} has an invalid <b>height</b> (nearest %4 heights are <b>{heightBelow}</b>/<b>{heightAbove}</b>).", texture2D);
                else // Invalid width, invalid height.
                    Debug.Log($"{path} has invalid <b>dimensions</b> (nearest %4 widths are <b>{widthBelow}</b>/<b>{widthAbove}</b>, nearest %4 heights are <b>{heightBelow}</b>/<b>{heightAbove}</b>).", texture2D);

                yield return null;
            }
        }

        private static System.Collections.IEnumerator LocateNearPowerOf2Coroutine()
        {
            int[] maxDiffs =
            {
                1,   // 4
                2,   // 8
                3,   // 16
                4,   // 32
                6,   // 64
                8,   // 128
                12,  // 256
                16,  // 512
                24,  // 1024
                48,  // 2048
                100, // 4096
                200, // 8192
                400, // 16384
            };

            System.Collections.Generic.List<Texture2D> textures = new System.Collections.Generic.List<Texture2D>();
            yield return EditorCoroutineUtility.StartCoroutineOwnerless(GetTextures(textures));

            float widthNearestPowerOf2 = 0f;
            float heightNearestPowerOf2 = 0f;
            
            foreach (Texture2D texture2D in textures)
            {
                int width = texture2D.width;
                int height = texture2D.height;

                for (int i = 2; i < 15; ++i)
                {
                    int power = (int)Mathf.Pow(2, i);
                    int maxDiff = maxDiffs[Mathf.Min(i - 2, maxDiffs.Length - 1)];
                    int widthDiff = Mathf.Abs(width - power);
                    int heightDiff = Mathf.Abs(height - power);

                    if (widthDiff < maxDiff)
                        widthNearestPowerOf2 = power;

                    if (heightDiff < maxDiff)
                        heightNearestPowerOf2 = power;

                    if (widthNearestPowerOf2 != 0f && heightNearestPowerOf2 != 0f)
                        break;
                }

                if (widthNearestPowerOf2 != 0f && heightNearestPowerOf2 != 0f)
                {
                    string path = AssetDatabase.GetAssetPath(texture2D);
                    Debug.Log($"{path} dimensions <b>{width}x{height}</b> are near powers of 2 (nearest POT width is <b>{widthNearestPowerOf2}, nearest POT height is {heightNearestPowerOf2}</b>.", texture2D);
                }

                yield return null;
            }
        }

        private static System.Collections.IEnumerator LocateHighTrianglesCountCoroutine()
        {
            System.Collections.Generic.List<Sprite> sprites = new System.Collections.Generic.List<Sprite>();
            yield return EditorCoroutineUtility.StartCoroutineOwnerless(GetSprites(sprites));
            
            System.Collections.Generic.Dictionary<Sprite, int> spritesTriangles = new System.Collections.Generic.Dictionary<Sprite, int>();

            foreach (Sprite sprite in sprites)
                spritesTriangles.Add(sprite, sprite.triangles.Length);

            IOrderedEnumerable<System.Collections.Generic.KeyValuePair<Sprite, int>> spritesTrianglesOrderer = spritesTriangles
                                                                                                               .Where(o => o.Value > 2)
                                                                                                               .OrderByDescending(o => o.Value);

            foreach (System.Collections.Generic.KeyValuePair<Sprite, int> kvp in spritesTrianglesOrderer)
            {
                string path = AssetDatabase.GetAssetPath(kvp.Key);
                int triangles = kvp.Value / 3; // Triangles array's length is thrice the actual triangles count.
                Debug.Log($"{path} has <b>{triangles}</b> triangles.", kvp.Key);
                yield return null;
            }
        }
        #endregion // EDITOR COROUTINES
        #endif
    }
}