using System;
using System.Collections.Generic;

namespace HFSM {
    
    public abstract class StateMachine {
        
        private State currentSubState;
        private State defaultSubState;
        private StateMachine parent;
        
        private Dictionary<Type, State> subStates = new Dictionary<Type, State>();
        private Dictionary<int, State> transitions = new Dictionary<int, State>();

        public void EnterStateMachine() {
            Enter();
            if (currentSubState == null && defaultSubState != null) {
                currentSubState = defaultSubState;
            }
            currentSubState?.EnterStateMachine();
        }

        public void UpdateStateMachine() {
            Update();
            currentSubState?.UpdateStateMachine();
        }

        public void ExitStateMachine() {
            currentSubState?.ExitStateMachine();
            Exit();
        }

        protected virtual void Enter() { }
        
        protected virtual void Update() { }
        
        protected virtual void Exit() { }

        public void ChangeSubState<T>() where T : State {
            currentSubState?.ExitStateMachine();
            var newState = subStates[typeof(T)];
            currentSubState = newState;
            newState.EnterStateMachine();
        }

        public void ChangeSubState(State state) {
            currentSubState?.ExitStateMachine();
            var newState = subStates[state.GetType()];
            currentSubState = newState;
            newState.EnterStateMachine();
        }

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
    }

    public abstract class State : StateMachine {
        public override int GetHashCode() {
            return GetType().ToString().GetHashCode();
        }
    }
    
    
    public class FSMException : Exception {
        public FSMException(string msg) : base(msg) { }
    }

}