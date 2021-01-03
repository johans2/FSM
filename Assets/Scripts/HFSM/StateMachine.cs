using System;
using System.Collections.Generic;

namespace HFSM {
    
    public abstract class State {
        
        private State currentSubState;
        private State defaultSubState;
        private State parent;
        
        private Dictionary<Type, State> subStates = new Dictionary<Type, State>();
        private Dictionary<int, State> transitions = new Dictionary<int, State>();

        public void EnterStateMachine() {
            OnEnter();
            if (currentSubState == null && defaultSubState != null) {
                currentSubState = defaultSubState;
            }
            currentSubState?.EnterStateMachine();
        }

        public void UpdateStateMachine() {
            OnUpdate();
            currentSubState?.UpdateStateMachine();
        }

        public void ExitStateMachine() {
            currentSubState?.ExitStateMachine();
            OnExit();
        }

        protected virtual void OnEnter() { }
        
        protected virtual void OnUpdate() { }
        
        protected virtual void OnExit() { }

        public void LoadSubState(State subState) {
            if (subStates.Count == 0) {
                defaultSubState = subState;
            }

            subState.parent = this;
            try {
                subStates.Add(subState.GetType(), subState);
            }
            catch (ArgumentException) {
                throw new DuplicateSubStateException($"State {GetType()} already contains a substate of type {subState.GetType()}");
            }
            
        }
        
        public void AddTransition(State from, State to, int trigger) {
            try {
                from.transitions.Add(trigger, to);
            }
            catch (ArgumentException) {
                throw new DuplicateTransitionException($"State {from} already has a transition defined for trigger {trigger}");
            }

        }

        public void SendTrigger(int trigger) {
            var root = this;
            while (root?.parent != null) {
                root = root.parent;
            }

            while (root != null) {
                if (root.transitions.TryGetValue(trigger, out State toState)) {
                    root.parent?.ChangeSubState(toState);
                    return;
                }

                root = root.currentSubState;
            }
            
            throw new NeglectedTriggerException($"Trigger {trigger} was not consumed by any transition!");
        }
        
        private void ChangeSubState(State state) {
            currentSubState?.ExitStateMachine();
            var newState = subStates[state.GetType()];
            currentSubState = newState;
            newState.EnterStateMachine();
        }
    }
}