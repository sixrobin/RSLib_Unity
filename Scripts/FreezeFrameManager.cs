namespace RSLib
{
    public class FreezeFrameManager : RSLib.Framework.Singleton<FreezeFrameManager>
    {
        private System.Collections.IEnumerator _freezeCoroutine;

        public static bool IsFroze => Exists() && Instance._freezeCoroutine != null;

        public static void Freeze(float duration, float delay = 0f, float targetTimeScale = 0f, bool overrideCurrentFreeze = false, System.Action callback = null)
        {
            if (!Exists() || duration <= 0f)
                return;

            if (Instance._freezeCoroutine != null)
            {
                if (!overrideCurrentFreeze)
                    return;

                Instance.StopCoroutine(Instance._freezeCoroutine);
            }

            Instance.StartCoroutine(Instance._freezeCoroutine = FreezeCoroutine(duration, delay, targetTimeScale, callback));
        }

        private static System.Collections.IEnumerator FreezeCoroutine(float duration, float delay, float targetTimeScale = 0f, System.Action callback = null)
        {
            yield return RSLib.Yield.SharedYields.WaitForSecondsRealtime(delay);
            UnityEngine.Time.timeScale = targetTimeScale;
            yield return RSLib.Yield.SharedYields.WaitForSecondsRealtime(duration);
            UnityEngine.Time.timeScale = 1f;

            Instance._freezeCoroutine = null;
            callback?.Invoke();
        }

        private void Update()
        {
            RSLib.Debug.ValuesDebugger.DebugValue("time_scale", () => UnityEngine.Time.timeScale);
        }
    }
}