﻿namespace RSLib.Editor
{
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public sealed class TilemapUtilitiesMenu
    {
        [MenuItem("RSLib/Tilemap Utilities")]
        public static void LaunchTilesOverrider()
        {
            TilemapUtiliesEditor.LaunchOverrider();
        }
    }

    public sealed class TilemapUtiliesEditor : EditorWindow
    {
        private static bool _firstOpenFrame = true;

        // Override tiles.
        private Tilemap _source;
        private Tilemap _destination;
        private string _destinationName;
        private TileBase _tile;

        // Clear tilemap.
        private Tilemap _tilemapToClear;

        public static void LaunchOverrider()
        {
            _firstOpenFrame = true;

            EditorWindow window = GetWindow<TilemapUtiliesEditor>("Tilemap Utilities");
            window.Show();
        }

        private void OverrideTiles()
        {
            _destination = Instantiate(_source, _source.transform.parent);
            _destination.name = !string.IsNullOrEmpty(_destinationName) ? _destinationName : $"{_source.name} - COPY";
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

            EditorUtilities.SceneManagerUtilities.SetCurrentSceneDirty();
            EditorUtilities.PrefabEditorUtilities.SetCurrentPrefabStageDirty();
        }

        private void ClearTiles(Tilemap tilemap)
        {
            tilemap.ClearAllTiles();
            tilemap.ClearAllEditorPreviewTiles();

            EditorUtilities.SceneManagerUtilities.SetCurrentSceneDirty();
            EditorUtilities.PrefabEditorUtilities.SetCurrentPrefabStageDirty();
        }

        private void OnGUI()
        {
            if (_firstOpenFrame)
            {
                Tilemap selectedTilemap = Selection.activeGameObject?.GetComponent<Tilemap>();

                _source = selectedTilemap;
                _tilemapToClear = selectedTilemap;
                _firstOpenFrame = false;

                if (_source != null)
                    _destinationName = $"{_source.name} - COPY";
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(10f);

            GUILayout.Space(10f);


            // Override tiles.

            EditorGUILayout.LabelField("OVERRIDE TILEMAP TILES", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5f);

            _source = EditorGUILayout.ObjectField(_source, typeof(Tilemap), true, null) as Tilemap;
            _tile = EditorGUILayout.ObjectField(_tile, typeof(TileBase), true, null) as TileBase;

            GUILayout.Space(5f);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("New Tilemap Name");
            _destinationName = EditorGUILayout.TextField(_destinationName);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5f);

            if (GUILayout.Button("Override Tiles", GUILayout.Height(45f), GUILayout.ExpandWidth(true)))
            {
                if (_source == null)
                {
                    EditorUtility.DisplayDialog("Tilemap Utilities Warning", "You must provide a source Tilemap for tiles positions!", "OK");
                    return;
                }

                if (_tile is RuleTile || _tile is RuleOverrideTile)
                {
                    OverrideTiles();
                }
                else if (_tile != null
                    && EditorUtility.DisplayDialog(
                    "Tilemap Utilities Warning",
                    "Referenced tile is not a RuleTile. Are you sure you want to fill a tilemap with a non ruled tile?",
                    "Yes",
                    "No"))
                {
                    OverrideTiles();
                }
                else
                {
                    EditorUtility.DisplayDialog("Tilemap Utilities Warning", "You must provide a tile to create new Tilemap!", "OK");
                    return;
                }
            }

            GUILayout.Space(5f);
            EditorGUILayout.EndVertical();

            GUILayout.Space(10f);


            // Clear tilemap.

            EditorGUILayout.LabelField("CLEAR TILEMAP (NO UNDO)", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(5f);

            _tilemapToClear = EditorGUILayout.ObjectField(_source, typeof(Tilemap), true, null) as Tilemap;

            GUILayout.Space(5f);

            if (GUILayout.Button("Clear Tiles", GUILayout.Height(45f), GUILayout.ExpandWidth(true)))
            {
                if (_tilemapToClear == null)
                {
                    EditorUtility.DisplayDialog("Tilemap Utilities Warning", "You must provide a Tilemap to clear its tiles!", "OK");
                    return;
                }

                ClearTiles(_tilemapToClear);
            }

            GUILayout.Space(5f);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            GUILayout.Space(10f);
            EditorGUILayout.EndHorizontal();

            Repaint();
        }
    }
}