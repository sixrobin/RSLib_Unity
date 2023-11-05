namespace RSLib.Editor
{
	using UnityEngine;
	using UnityEditor;

	/// <summary>
	/// Select all gameObjects in a scene based on their static flags state.
	/// For instance, this can be used to select all gameObject flagged as Occludee Static.
	/// </summary>
	public sealed class FilterStaticObjectEditor : EditorWindow
	{
		private StaticEditorFlags _flag;
		private bool _include;

		[MenuItem("RSLib/Filter Static Objects")]
		public static void Open()
		{
			GetWindow<FilterStaticObjectEditor>("Filter Static Objects").Show();
		}

        private void FilterSelection(bool include)
		{
            Object[] gameObjects = FindObjectsOfType(typeof(GameObject));
            Object[] gameObjectsArray = new Object[gameObjects.Length];
			int i = 0;

			foreach (Object obj in gameObjects)
			{
				GameObject gameObject = (GameObject)obj;
				StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(gameObject);

				if (((flags & this._flag) != 0) == include)
					gameObjectsArray[i++] = gameObject;
			}

			Selection.objects = gameObjectsArray;
		}

		private void OnGUI()
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(10f);
			EditorGUILayout.BeginVertical();
			GUILayout.Space(10f);

			EditorGUILayout.LabelField("Flag to filter:", EditorStyles.boldLabel);
            _flag = (StaticEditorFlags)EditorGUILayout.EnumPopup(_flag);
            _include = EditorGUILayout.Toggle("Include", _include);

			GUILayout.Space(10f);

			if (GUILayout.Button("Filter Selection", GUILayout.Height(45f), GUILayout.ExpandWidth(true)))
				FilterSelection(_include);

			EditorGUILayout.EndVertical();
			GUILayout.Space(10f);
			EditorGUILayout.EndHorizontal();

			Repaint();
		}
	}
}