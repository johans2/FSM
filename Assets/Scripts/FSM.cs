using System.Collections.Generic;
using UnityEngine;
using System;
/*
public static class FSM {

    public abstract class State {
        public State parent;
        public List<State> subStates = new List<State>();
        public State DefaultSubState => subStates.Count > 0 ? subStates[0] : null; 
        
        public virtual void Enter() { }
        
        public virtual void Update() { }
        
        public virtual void Exit() { }
        
    }

    public static State CurrentState { get; private set; }

    private static Dictionary<Type, State> states = new Dictionary<Type, State>();

    public static void LoadState(State state) {
        states.Add(state.GetType(), state);
    }

    public static void Tick() {
        CurrentState?.Update();
    }

    public static void GoToState<T>() where T : State  {
        State newState = states[typeof(T)];

        
        State targetParent = newState.parent;
        if (CurrentState != null) {

            
            CurrentState.Exit();
            State parentState = CurrentState.parent;
            //State targetParent = newState.parent;
            
            while (parentState != targetParent) {
                if(parentState == targetParent) {
                    break;
                }

                if(targetParent?.parent != null) {
                    targetParent = targetParent.parent;
                }

                parentState.Exit();
                parentState = parentState.parent;
            }            
        }

        CurrentState = newState;
        Stack<State> enterStack = new Stack<State>();
        while (CurrentState != targetParent) {
            enterStack.Push(CurrentState);
            CurrentState = CurrentState.parent;
        }

        while (enterStack.Count > 0) {
            CurrentState = enterStack.Pop();
            CurrentState.Enter();
        }
        
        //CurrentState.Enter();
        
        while (CurrentState.DefaultSubState != null) {
            CurrentState.DefaultSubState.Enter();
            CurrentState = CurrentState.DefaultSubState;
        }
    }

    public static void Clear() {
        states.Clear();
        CurrentState = null;
    }

    public static void SetSubState<TParentState, TSubState>() where TParentState : State where TSubState : State {
        if (!states.TryGetValue(typeof(TParentState), out State parent)) {
            throw new FSMException($"Cannot set substate to parent {typeof(TParentState)}. Parent not loaded.");
        }
        
        if (!states.TryGetValue(typeof(TSubState), out State subState)) {
            throw new FSMException($"Cannot set substate {typeof(TSubState)}. Substate not loaded.");
        }

        for (int i = 0; i < parent.subStates.Count; i++) {
            if (parent.subStates[i].GetType() == subState.GetType()) {
                throw new FSMException($"State {parent.GetType()} already has a substate {subState.GetType()}");
            }
        }

        if (subState.parent != null) {
            throw new FSMException($"State {subState.GetType()} is already a substate of {subState.parent.GetType()}");
        }
        
        parent.subStates.Add(subState);
        subState.parent = parent;
    }
}

public class FSMException : Exception {
    public FSMException(string msg) : base(msg) { }
}
*/

