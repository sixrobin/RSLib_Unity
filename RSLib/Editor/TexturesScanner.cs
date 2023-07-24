namespace RSLib.Editor
{
    using System.Linq;
    using UnityEngine;
    using UnityEditor;

    public static class TexturesScanner
    {
        private static Texture2D[] GetTextures()
        {
            string[] guids = AssetDatabase.FindAssets("t:texture");
            System.Collections.Generic.List<Texture2D> textures = new System.Collections.Generic.List<Texture2D>();

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

                textures.Add(texture2D);
            }

            return textures.ToArray();
        }
        
        private static Sprite[] GetSprites()
        {
            string[] guids = AssetDatabase.FindAssets("t:sprite");
            System.Collections.Generic.List<Sprite> sprites = new System.Collections.Generic.List<Sprite>();

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

                sprites.Add(sprite);
            }

            return sprites.ToArray();
        }
        
        [MenuItem("RSLib/Texture Scan/Locate non multiple of 4")]
        private static void LocateNonMultipleOf4()
        {
            Texture2D[] textures = GetTextures();

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
                    Debug.Log($"Texture {path} has an invalid <b>widths</b> (nearest %4 widths are <b>{widthBelow}</b>/<b>{widthAbove}</b>).", texture2D);
                else if (widthAbove < 0f && heightAbove >= 0f) // Valid width, invalid height.
                    Debug.Log($"Texture {path} has an invalid <b>height</b> (nearest %4 heights are <b>{heightBelow}</b>/<b>{heightAbove}</b>).", texture2D);
                else // Invalid width, invalid height.
                    Debug.Log($"Texture {path} has invalid <b>dimensions</b> (nearest %4 widths are <b>{widthBelow}</b>/<b>{widthAbove}</b>, nearest %4 heights are <b>{heightBelow}</b>/<b>{heightAbove}</b>).", texture2D);
            }
        }

        [MenuItem("RSLib/Texture Scan/Locate near power of 2")]
        private static void LocateNearPowerOf2()
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

            Texture2D[] textures = GetTextures();

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
                    Debug.Log($"Texture {path} dimensions <b>{width}x{height}</b> are near powers of 2 (nearest POT width is <b>{widthNearestPowerOf2}, nearest POT height is {heightNearestPowerOf2}</b>.", texture2D);
                }
            }
        }

        [MenuItem("RSLib/Texture Scan/Locate high triangles count (100 higher)")]
        private static void LocateHighTrianglesCount()
        {
            Sprite[] sprites = GetSprites();
            System.Collections.Generic.Dictionary<Sprite, int> spritesTriangles = new System.Collections.Generic.Dictionary<Sprite, int>();

            foreach (Sprite sprite in sprites)
                spritesTriangles.Add(sprite, sprite.triangles.Length);

            IOrderedEnumerable<System.Collections.Generic.KeyValuePair<Sprite, int>> spritesTrianglesOrderer = spritesTriangles
                                                                                                               .Where(o => o.Value > 2)
                                                                                                               .OrderByDescending(o => o.Value);

            int i = 0;
            foreach (System.Collections.Generic.KeyValuePair<Sprite, int> kvp in spritesTrianglesOrderer)
            {
                string path = AssetDatabase.GetAssetPath(kvp.Key);
                int triangles = kvp.Value / 3; // Triangles array's length is thrice the actual triangles count.
                Debug.Log($"Sprite {path} has <b>{triangles}</b> triangles.", kvp.Key);
                
                if (++i == 100)
                    break;
            }
        }
    }
}