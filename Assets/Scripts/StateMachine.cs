using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace HFSM {
    
    public abstract class StateMachine {
        
        private class Transition{
            public readonly State from;
            public readonly State to;
            private readonly int trigger;

            public Transition(State from, State to, int trigger) {
                this.from = from;
                this.to = to;
                this.trigger = trigger;
            }

            public override int GetHashCode() {
                return (from.GetHashCode(), to.GetHashCode(), trigger.GetHashCode()).GetHashCode();
            }

            public override string ToString() {
                return $"[{from} -> {to}]";
            }
        }
        
        private State currentSubState;
        private State defaultSubState;
        private StateMachine parent;
        
        private Dictionary<Type, State> subStates = new Dictionary<Type, State>();
        private Dictionary<int, Transition> transitions = new Dictionary<int, Transition>(); // TODO: key should not be the trigger, but a hash from key, from and to
        
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
        
        public void CreateTransition(State from, State to, int trigger) {
            Transition transion = new Transition(from, to, trigger);
            int transitionKey = transion.GetHashCode();
            if (transitions.ContainsKey(transitionKey)) {
                throw new FSMException($"StateMachine {this} already has a trigger defined from state {from} to state {to} with trigger {trigger}");
            }
            transitions.Add(transitionKey, transion);
        }

        public void SendTrigger(int trigger) {
            var root = this;
            while (root?.parent != null) {
                root = root.parent;
            }

            while (root != null) {
                // TODO: this lookup failes atm. Need a better way to identify transitions.
                if (root.transitions.TryGetValue(trigger, out Transition transition)) {
                    ValidateTransition(root, transition);
                    root.ChangeSubState(transition.to);
                    return;
                }

                root = root.currentSubState;
            }
            
            throw new FSMException($"Trigger {trigger} was not consumed by any transition!");
        }

        private static void ValidateTransition(StateMachine root, Transition transition) {
            if (!root.subStates.ContainsKey(transition.@from.GetType())) {
                throw new FSMException(
                    $"StateMachine {root} has transition defined {transition} but has no substate {transition.@from}");
            }

            if (!root.subStates.ContainsKey(transition.to.GetType())) {
                throw new FSMException(
                    $"StateMachine {root} has transition defined {transition} but has no substate {transition.to}");
            }
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