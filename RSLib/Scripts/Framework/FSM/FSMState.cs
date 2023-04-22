namespace RSLib.Framework.FSM
{
    using System.Collections.Generic;

    public abstract class FSMState<TStateID, TTransitionID, TBehaviourContext> where TStateID : System.Enum where TTransitionID : System.Enum
    {
        public FSMState(TStateID id)
        {
            Id = id;
        }

        private readonly Dictionary<TTransitionID, TStateID> _transitionsMap = new Dictionary<TTransitionID, TStateID>();

        public TStateID Id { get; }

        public void AddTransition(TTransitionID transition, TStateID id)
        {
            if (_transitionsMap.ContainsKey(transition))
            {
                UnityEngine.Debug.LogWarning($"A transition with enum ID {transition} already exists in the transitions map!");
                return;
            }
            
            _transitionsMap.Add(transition, id);
        }

        public void RemoveTransition(TTransitionID transition)
        {
            if (!_transitionsMap.ContainsKey(transition))
            {
                UnityEngine.Debug.LogWarning($"No transition with enum ID {transition} exists in the transitions map to remove it!");
                return;
            }
            
            _transitionsMap.Remove(transition);
        }

        public TStateID GetTransitionOutputState(TTransitionID transition)
        {
            if (!_transitionsMap.ContainsKey(transition))
            {
                UnityEngine.Debug.LogWarning($"No transition with enum ID {transition} exists in the transitions map to get its output state! Returning default value.");
                return default;
            }
            
            return _transitionsMap[transition];
        }

        public virtual void OnStateEntered()
        {
        }
        
        public virtual void OnStateExit()
        {
        }

        /// <summary>
        /// State transition if current context requires one.
        /// </summary>
        public abstract TTransitionID Reason(TBehaviourContext context);

        /// <summary>
        /// State behaviour.
        /// </summary>
        public abstract void Act(TBehaviourContext context);
    }
}