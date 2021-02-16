using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HFSM;

public class MoveState : StateMachine {
    protected override void OnUpdate() {
        Debug.Log("Updating MoveState");
    }
}

public class RunState : StateMachine {
    protected override void OnEnter() {
        Debug.Log("Enter RunState");
    }

    protected override void OnUpdate() {
        Debug.Log("Updating RunState");
    }
    
    protected override void OnExit() {
        Debug.Log("Exit JumpState");
    }
}

public class JumpState : StateMachine {
    protected override void OnEnter() {
        Debug.Log("Enter JumpState");
    }
    
    protected override void OnUpdate() {
        Debug.Log("Updating JumpState");
    }
    
    protected override void OnExit() {
        Debug.Log("Exit JumpState");
    }
}

public class Triggers {
    public const int JUMP = 1;
    public const int RUN = 2;
}


public class Example : MonoBehaviour
{
    
    StateMachine moveState = new MoveState();
    StateMachine runState = new RunState();
    StateMachine jumpState = new JumpState();
    
    void Start()
    {
        moveState.LoadSubState(runState);
        moveState.LoadSubState(jumpState);
        moveState.AddTransition(runState, jumpState, Triggers.JUMP);
        moveState.AddTransition(jumpState, runState, Triggers.RUN);
        moveState.EnterStateMachine();
    }
    
    void Update()
    {
        moveState.UpdateStateMachine();
        if (Input.GetKeyDown(KeyCode.J)) {
            moveState.SendTrigger(Triggers.JUMP);
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            moveState.SendTrigger(Triggers.RUN);
        }
    }
}
