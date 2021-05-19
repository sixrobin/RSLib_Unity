#if UNITY_EDITOR
namespace RSLib.EditorUtilities
{
    using UnityEditor.Experimental.SceneManagement;
    using UnityEditor.SceneManagement;

    public static class PrefabEditorUtilities
    {
        public static void SetCurrentPrefabStageDirty()
        {
            PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
        }
    }
}
#endif