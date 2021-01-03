using System;
using System.Collections.Generic;

namespace HFSM {
    
    public abstract class StateMachine {
        
        private StateMachine currentSubState;
        private StateMachine defaultSubState;
        private StateMachine parent;
        
        private Dictionary<Type, StateMachine> subStates = new Dictionary<Type, StateMachine>();
        private Dictionary<int, StateMachine> transitions = new Dictionary<int, StateMachine>();

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

        public void LoadSubState(StateMachine subState) {
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
        
        public void AddTransition(StateMachine from, StateMachine to, int trigger) {
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
                if (root.transitions.TryGetValue(trigger, out StateMachine toState)) {
                    root.parent?.ChangeSubState(toState);
                    return;
                }

                root = root.currentSubState;
            }
            
            throw new NeglectedTriggerException($"Trigger {trigger} was not consumed by any transition!");
        }
        
        private void ChangeSubState(StateMachine state) {
            currentSubState?.ExitStateMachine();
            var newState = subStates[state.GetType()];
            currentSubState = newState;
            newState.EnterStateMachine();
        }
    }
}