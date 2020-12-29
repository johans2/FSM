using System;
using System.Collections.Generic;

namespace HFSM {
    
    public abstract class StateMachine {
        
        private StateMachine currentSubState;
        private StateMachine defaultSubState;
        private StateMachine parent;
        
        private Dictionary<Type, State> subStates = new Dictionary<Type, State>();
        private Dictionary<int, State> transitions = new Dictionary<int, State>();

        public void Enter() {
            OnEnter();
            if (currentSubState == null && defaultSubState != null) {
                currentSubState = defaultSubState;
            }
            currentSubState?.Enter();
        }

        public void Update() {
            OnUpdate();
            currentSubState?.Update();
        }

        public void Exit() {
            currentSubState?.Exit();
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
            subStates.Add(subState.GetType(), subState);
        }
        
        public void AddTransition(State from, State to, int trigger) {
            from.transitions.Add(trigger, to);
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
            
            throw new FSMException($"Trigger {trigger} was not consumed by any transition!");
        }
        
        private void ChangeSubState(State state) {
            currentSubState?.Exit();
            var newState = subStates[state.GetType()];
            currentSubState = newState;
            newState.Enter();
        }

    }

    public abstract class State : StateMachine {
        private new void Update() { }
        private new void Enter() { }
        private new void Exit() { }
    }

    public class FSMException : Exception {
        public FSMException(string msg) : base(msg) { }
    }

}