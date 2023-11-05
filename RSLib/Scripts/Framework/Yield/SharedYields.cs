namespace RSLib.Yield
{
    using UnityEngine;

    public static class SharedYields
    {
        private static readonly System.Collections.Generic.Dictionary<float, WaitForSeconds> WAITS_FOR_SECONDS = new System.Collections.Generic.Dictionary<float, WaitForSeconds>(100, new Framework.Comparers.FloatComparer());
        private static readonly System.Collections.Generic.Dictionary<float, WaitForSecondsRealtime> WAIT_FOR_SECONDS_REALTIME_COLLECTION = new System.Collections.Generic.Dictionary<float, WaitForSecondsRealtime>(100, new Framework.Comparers.FloatComparer());

        public static WaitForEndOfFrame WaitForEndOfFrame { get; } = new WaitForEndOfFrame();
        public static WaitForFixedUpdate WaitForFixedUpdate { get; } = new WaitForFixedUpdate();

        /// <summary>
        /// Waits for a given amount of frames.
        /// </summary>
        /// <param name="framesCount">Frames to wait.</param>
        /// <returns>IEnumerator instance.</returns>
        public static System.Collections.IEnumerator WaitForFrames(int framesCount)
        {
            while (framesCount-- > 0)
                yield return null;
        }

        /// <summary>
        /// Returns an existing WaitForSeconds if one with the given duration has already been pooled, else a new one and pools it.
        /// </summary>
        /// <param name="duration">Seconds to wait.</param>
        /// <returns>WaitForSeconds instance.</returns>
        public static WaitForSeconds WaitForSeconds(float duration)
        {
            if (!WAITS_FOR_SECONDS.TryGetValue(duration, out WaitForSeconds wait))
                WAITS_FOR_SECONDS.Add(duration, wait = new WaitForSeconds(duration));

            return wait;
        }

        /// <summary>
        /// Returns an existing WaitForSecondsRealtime if one with the given duration has already been pooled, else a new one and pools it.
        /// </summary>
        /// <param name="duration">Seconds to wait.</param>
        /// <returns>WaitForSecondsRealtime instance.</returns>
        public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
        {
            if (!WAIT_FOR_SECONDS_REALTIME_COLLECTION.TryGetValue(duration, out WaitForSecondsRealtime wait))
                WAIT_FOR_SECONDS_REALTIME_COLLECTION.Add(duration, wait = new WaitForSecondsRealtime(duration));

            return wait;
        }

        /// <summary>
        /// Waits while the given scene is not the active scene.
        /// This coroutine will not load the scene, it just handles the waiting.
        /// </summary>
        /// <param name="sceneName">Loaded scene name.</param>
        /// <returns>IEnumerator instance.</returns>
        public static System.Collections.IEnumerator WaitForSceneLoad(string sceneName)
        {
            while (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != sceneName)
                yield return null;
        }
    }
}