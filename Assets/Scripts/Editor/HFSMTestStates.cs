using HFSM;

namespace Tests {
    
    public class Game : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("game_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("game_update");
        }
    }
    
    public class StateA : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("stateA_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("stateA_update");
        }

        protected override void OnExit() {
            HFSMTests.AddLogCall("stateA_exit");
        }
    }
    
    public class StateA1 : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("stateA1_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("stateA1_update");
        }

        protected override void OnExit() {
            HFSMTests.AddLogCall("stateA1_exit");
        }
    }

    public class StateA2 : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("stateA2_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("stateA2_update");
        }

        protected override void OnExit() {
            HFSMTests.AddLogCall("stateA2_exit");
        }
    }

    public class StateA3 : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("stateA3_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("stateA3_update");
        }

        protected override void OnExit() {
            HFSMTests.AddLogCall("stateA3_exit");
        }
    }
    
    public class StateB : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("stateB_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("stateB_update");
        }

        protected override void OnExit() {
            HFSMTests.AddLogCall("stateB_exit");
        }
    }
    
    public class StateB1 : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("stateB1_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("stateB1_update");
        }

        protected override void OnExit() {
            HFSMTests.AddLogCall("stateB1_exit");
        }
    }

    public class StateB2 : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("stateB2_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("stateB2_update");
        }

        protected override void OnExit() {
            HFSMTests.AddLogCall("stateB2_exit");
        }
    }

    public class StateB3 : State {
        protected override void OnEnter() {
            HFSMTests.AddLogCall("stateB3_enter");
        }

        protected override void OnUpdate() {
            HFSMTests.AddLogCall("stateB3_update");
        }

        protected override void OnExit() {
            HFSMTests.AddLogCall("stateB3_exit");
        }
    }
}