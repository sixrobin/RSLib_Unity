namespace RSLib.Yield
{
    using UnityEngine;

    public static class SharedYields
    {
        private static System.Collections.Generic.Dictionary<float, WaitForSeconds> s_waitsForSeconds = new System.Collections.Generic.Dictionary<float, WaitForSeconds>(100, new Framework.Comparers.FloatComparer());
        private static System.Collections.Generic.Dictionary<float, WaitForSecondsRealtime> s_waitForFramesRealtimeCollection = new System.Collections.Generic.Dictionary<float, WaitForSecondsRealtime>(100, new Framework.Comparers.FloatComparer());

        public static WaitForEndOfFrame WaitForEndOfFrame { get; } = new WaitForEndOfFrame();
        public static WaitForFixedUpdate WaitForFixedUpdate { get; } = new WaitForFixedUpdate();

        /// <summary>Returns an existing WaitForFrames if one with the given duration has already been pooled, else a new one and pools it.</summary>
        /// <param name="duration">Frames to wait.</param>
        /// <returns>WaitForFrames instance.</returns>
        public static System.Collections.IEnumerator WaitForFrames(int framesCount)
        {
            while (framesCount-- > 0)
                yield return null;
        }

        /// <summary>Returns an existing WaitForSeconds if one with the given duration has already been pooled, else a new one and pools it.</summary>
        /// <param name="duration">Seconds to wait.</param>
        /// <returns>WaitForSeconds instance.</returns>
        public static WaitForSeconds WaitForSeconds(float duration)
        {
            if (!s_waitsForSeconds.TryGetValue(duration, out WaitForSeconds wait))
                s_waitsForSeconds.Add(duration, wait = new WaitForSeconds(duration));

            return wait;
        }

        /// <summary>Returns an existing WaitForSecondsRealtime if one with the given duration has already been pooled, else a new one and pools it.</summary>
        /// <param name="duration">Seconds to wait.</param>
        /// <returns>WaitForSecondsRealtime instance.</returns>
        public static WaitForSecondsRealtime WaitForSecondsRealtime(float duration)
        {
            if (!s_waitForFramesRealtimeCollection.TryGetValue(duration, out WaitForSecondsRealtime wait))
                s_waitForFramesRealtimeCollection.Add(duration, wait = new WaitForSecondsRealtime(duration));

            return wait;
        }
    }
}