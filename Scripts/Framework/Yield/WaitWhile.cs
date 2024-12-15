namespace RSLib.Unity.Yield
{
    using UnityEngine;

    public class WaitWhile : CustomYieldInstruction
    {
        private readonly System.Func<bool> _predicate;

        public WaitWhile(System.Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public override bool keepWaiting => _predicate();
    }
}