namespace RSLib.Editor
{
	using UnityEngine;
	using UnityEditor;
    using Extensions;

    public sealed class LayerRecursiveSetterEditor : EditorWindow
	{
		private GameObject[] _selection;
		private string _layerName;

		[MenuItem("RSLib/Layer Recursive Setter", true)]
		private static bool CheckSelectionCount()
		{
			return Selection.gameObjects.Length > 0;
		}

		[MenuItem("RSLib/Layer Recursive Setter")]
		public static void Open()
		{
			GetWindow<LayerRecursiveSetterEditor>("Layer Recursive Setter").Show();
		}

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Layer Name", EditorStyles.boldLabel);
			_layerName = EditorGUILayout.TextField(_layerName);

			_selection = Selection.gameObjects;

			if (GUILayout.Button("Set objet's children layer recursively", GUILayout.Height(45f), GUILayout.ExpandWidth(true)))
                foreach (GameObject selected in _selection)
					selected.SetChildrenLayers(LayerMask.NameToLayer(_layerName));

			Repaint();
		}
	}
}