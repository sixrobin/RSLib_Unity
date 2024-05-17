namespace RSLib.Yield
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Custom yield instruction used to wait for multiple coroutines to all be over.
    /// </summary>
    public class WaitForParallelCoroutines : CustomYieldInstruction
    {
        public WaitForParallelCoroutines(MonoBehaviour runner, params IEnumerator[] coroutines)
        {
            foreach (IEnumerator coroutine in coroutines)
                runner.StartCoroutine(this.StartCoroutineWithCounter(runner, coroutine));
        }
        
        private int _counter;

        public override bool keepWaiting => _counter > 0;

        private IEnumerator StartCoroutineWithCounter(MonoBehaviour runner, IEnumerator coroutine)
        {
            _counter++;
            yield return runner.StartCoroutine(coroutine);
            _counter--;
        }
    }
}
