using System;
using System.Collections.Generic;

namespace SM {
    
    
    public abstract class StateMachine {

        private State currentState;
        private State defaultState;
        
        private Dictionary<Type, State> states = new Dictionary<Type, State>();

        public void EnterStateMachine() {
            Enter();
            if (currentState == null && defaultState != null) {
                currentState = defaultState;
            }
            currentState?.EnterStateMachine();
        }

        public void UpdateStateMachine() {
            Update();
            currentState?.UpdateStateMachine();
        }

        public void ExitStateMachine() {
            currentState?.ExitStateMachine();
            Exit();
        }

        protected virtual void Enter() {
        }
        
        protected virtual void Update() {
        }
        
        protected virtual void Exit() {
        }

        public void GoToState<T>() where T : State {
            currentState?.ExitStateMachine();
            var newState = states[typeof(T)];
            currentState = newState;
            newState.EnterStateMachine();
        }

        public void LoadState(State state) {
            if (states.Count == 0) {
                defaultState = state;
            }
            states.Add(state.GetType(), state);
        }
    }

    public abstract class State : StateMachine {
    }

    public class Game : StateMachine {
        
    }
}