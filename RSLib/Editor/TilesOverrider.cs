namespace RSLib.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public sealed class TilesOverriderMenu
    {
        [MenuItem("RSLib/Tilemap Tiles Overrider")]
        public static void LaunchTilesOverrider()
        {
            TilesOverriderEditor.LaunchOverrider();
        }
    }

    /// <summary>
    /// Allows to instantiates a new Tilemap gameObject based on a given source, and to override every tile of the source
    /// Tilemap with a given tile. This should be used with a rule tile to change an overall Tilemap at once.
    /// </summary>
    public sealed class TilesOverriderEditor : EditorWindow
    {
        private static bool _firstOpenFrame = true;

        private Tilemap _source;
        private Tilemap _destination;
        private TileBase _tile;

        public static void LaunchOverrider()
        {
            _firstOpenFrame = true;

            EditorWindow window = GetWindow<TilesOverriderEditor>("Override tilemap tiles");
            window.Show();
        }

        private void OverrideTiles()
        {
            _destination = Instantiate(_source, _source.transform.parent);
            _destination.name = $"{_source.name} [COPY]";
            _destination.ClearAllTiles();

            int tilesCounter = 0;

            foreach (Vector3Int pos in _source.cellBounds.allPositionsWithin)
            {
                if (_source.HasTile(pos))
                {
                    _destination.SetTile(pos, _tile);
                    tilesCounter++;
                }
            }

            if (tilesCounter == 0)
            {
                Debug.LogWarning("No tile has been found on source Tilemap to create a new Tilemap.");
                DestroyImmediate(_destination.gameObject);
                return;
            }

            Debug.Log($"Created new Tilemap {_destination.name} with {tilesCounter} overridden tiles.");
            Selection.activeGameObject = _destination.gameObject;
        }

        private void OnGUI()
        {
            if (_firstOpenFrame)
            {
                _source = Selection.activeGameObject?.GetComponent<Tilemap>();
                _firstOpenFrame = false;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10f);

            GUILayout.Space(10f);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5f);

            _source = EditorGUILayout.ObjectField(_source, typeof(Tilemap), true, null) as Tilemap;
            _tile = EditorGUILayout.ObjectField(_tile, typeof(TileBase), true, null) as TileBase;

            GUILayout.Space(5);
            EditorGUILayout.EndVertical();
            GUILayout.Space(10);

            if (GUILayout.Button("Override Tiles", GUILayout.Height(45f), GUILayout.ExpandWidth(true)))
            {
                if (_source == null)
                {
                    EditorUtility.DisplayDialog("Tiles Overrider warning", "You must provide a source Tilemap for tiles positions !", "OK");
                    return;
                }

                if (_tile is RuleTile || _tile is RuleOverrideTile)
                {
                    OverrideTiles();
                }
                else if (EditorUtility.DisplayDialog(
                    "Tiles Overrider warning",
                    "Referenced tile is not a RuleTile. Are you sure you want to fill a tilemap with a non ruled tile ?",
                    "Yes",
                    "No"))
                {
                    OverrideTiles();
                }
            }

            EditorGUILayout.EndVertical();
            GUILayout.Space(10f);
            EditorGUILayout.EndHorizontal();

            Repaint();
        }
    }
}