namespace RSLib.Framework.FSM
{
    public class FSM<TState, TTransition, TBehaviourContext> where TState : System.Enum where TTransition : System.Enum
    {
        public FSM()
        {
        }

        public FSM(params FSMState<TState, TTransition, TBehaviourContext>[] states)
        {
            for (int i = 0; i < states.Length; ++i)
                AddState(states[i]);
        }
        
        private readonly System.Collections.Generic.List<FSMState<TState, TTransition, TBehaviourContext>> _states = new System.Collections.Generic.List<FSMState<TState, TTransition, TBehaviourContext>>();

        public TState CurrentStateID { get; private set; }
        
        public FSMState<TState, TTransition, TBehaviourContext> CurrentState { get; private set; }

        public void AddState(FSMState<TState, TTransition, TBehaviourContext> state)
        {
            _states.Add(state);

            // First state is set as current one.
            if (_states.Count == 1)
            {
                CurrentState = state;
                CurrentStateID = state.Id;
            }
        }

        public void RemoveState(TState state)
        {
            for (int i = _states.Count - 1; i >= 0; --i)
            {
                if (System.Collections.Generic.EqualityComparer<TState>.Default.Equals(_states[i].Id, state))
                {
                    _states.RemoveAt(i);
                    return;
                }
            }

            UnityEngine.Debug.LogWarning($"Could not find a {nameof(TState)} with enum ID {state} to delete it.");
        }

        public void PerformTransition(TTransition transition)
        {
            TState state = CurrentState.GetTransitionOutputState(transition);

            for (int i = _states.Count - 1; i >= 0; --i)
            {
                if (!System.Collections.Generic.EqualityComparer<TState>.Default.Equals(_states[i].Id, state))
                {
                    continue;
                }
                
                CurrentStateID = state;
                CurrentState.OnStateExit();
                CurrentState = _states[i];
                CurrentState.OnStateEntered();
                
                return;
            }

            UnityEngine.Debug.LogWarning($"Could not find a {nameof(TState)} with enum ID {state} to perform a state transition {transition}!");
        }
    }
}