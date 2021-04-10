#if UNITY_EDITOR
namespace RSLib.EditorUtilities
{
    public static class SceneManagerUtilities
    {
        public static void SetCurrentSceneDirty()
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }
}
#endif