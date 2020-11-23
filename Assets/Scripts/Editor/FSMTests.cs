using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FSMTests
    {
        public class StateA : FSM.State {
            public bool enterCalled = false;
            public bool exitCalled = false;
            public override void Enter() { enterCalled = true; }
            public override void Exit() { exitCalled = true; }
        }
        
        public class StateB : FSM.State {
            public bool enterCalled = false;
            public bool exitCalled = false;
            public override void Enter() { enterCalled = true; }
            public override void Exit() { exitCalled = true; }
        }
        
        public class SubStateX : FSM.State {
            public bool enterCalled = false;
            public bool exitCalled = false;
            public override void Enter() { enterCalled = true; }
            public override void Exit() { exitCalled = true; }
        }
        
        public class SubStateY : FSM.State {
            public bool enterCalled = false;
            public bool exitCalled = false;
            public override void Enter() { enterCalled = true; }
            public override void Exit() { exitCalled = true; }
        }
        
        public class SubStateZ : FSM.State {
            public bool enterCalled = false;
            public bool exitCalled = false;
            public override void Enter() { enterCalled = true; }
            public override void Exit() { exitCalled = true; }
        }
        
        public class SubSubStateV : FSM.State {
            public bool enterCalled = false;
            public bool exitCalled = false;
            public override void Enter() { enterCalled = true; }
            public override void Exit() { exitCalled = true; }
        }

        public class SubSubStateU : FSM.State {
            public bool enterCalled = false;
            public bool exitCalled = false;
            public override void Enter() { enterCalled = true; }
            public override void Exit() { exitCalled = true; }
        }

        
        [SetUp]
        public void Setup() {
            FSM.Clear();
        }
        
        [Test]
        public void SetupStateHirarchy()
        {
            var stateA = new StateA();
            var subStateX = new SubStateX();
            var subStateY = new SubStateY();
            var subSubStateU = new SubSubStateU();
            
            FSM.LoadState(stateA);
            FSM.LoadState(subStateX);
            
            FSM.LoadState(subStateY);
            FSM.LoadState(subSubStateU);
            
            FSM.SetSubState<StateA, SubStateX>();
            FSM.SetSubState<StateA, SubStateY>();
            FSM.SetSubState<SubStateX, SubSubStateU>();
            
            Assert.IsNull(stateA.parent);
            Assert.AreEqual(2, stateA.subStates.Count);
            Assert.IsTrue(stateA.subStates[0] == subStateX);
            Assert.IsTrue(stateA.subStates[1] == subStateY);
            
            Assert.IsTrue(subStateX.parent == stateA);
            Assert.IsTrue(subStateY.parent == stateA);
            Assert.AreEqual(0, subStateY.subStates.Count);
            
            Assert.AreEqual(1, subStateX.subStates.Count);
            Assert.IsTrue(subStateX.subStates[0] == subSubStateU);
            
            Assert.IsTrue(subSubStateU.parent == subStateX);
            Assert.AreEqual(0, subSubStateU.subStates.Count);            
        }
        
        [Test]
        public void EnterStateHirarchy()
        {
            var stateA = new StateA();
            var subStateX = new SubStateX();
            var subStateY = new SubStateY();
            var subSubStateU = new SubSubStateU();
            
            FSM.LoadState(stateA);
            FSM.LoadState(subStateX);
            FSM.LoadState(subStateY);
            FSM.LoadState(subSubStateU);
            
            FSM.SetSubState<StateA, SubStateX>();
            FSM.SetSubState<StateA, SubStateY>();
            FSM.SetSubState<SubStateX, SubSubStateU>();
            
            FSM.GoToState<StateA>();
            
            Assert.IsTrue(stateA.enterCalled);
            Assert.IsTrue(subStateX.enterCalled);
            Assert.IsTrue(subSubStateU.enterCalled);
        }
        
        [Test]
        public void ExitStateHirarchy()
        {
            var stateA = new StateA();
            var subStateX = new SubStateX();
            var subStateY = new SubStateY();
            var subSubStateU = new SubSubStateU();
            
            var stateB = new StateB();
            var subStateZ = new SubStateZ();
            var subSubStateV = new SubSubStateV();

            FSM.LoadState(stateA);
            FSM.LoadState(subStateX);
            FSM.LoadState(subStateY);
            FSM.LoadState(subSubStateU);
            
            FSM.LoadState(stateB);
            FSM.LoadState(subStateZ);
            FSM.LoadState(subSubStateV);
            
            // State tree A
            FSM.SetSubState<StateA, SubStateX>();
            FSM.SetSubState<StateA, SubStateY>();
            FSM.SetSubState<SubStateX, SubSubStateU>();
            
            // State tree B
            FSM.SetSubState<StateB, SubStateZ>();
            FSM.SetSubState<SubStateZ, SubSubStateV>();
            
            FSM.GoToState<StateA>();
            
            FSM.GoToState<StateB>();
            Assert.IsTrue(subSubStateU.exitCalled);
            Assert.IsTrue(subStateX.exitCalled);
            Assert.IsTrue(stateA.exitCalled);
            
            Assert.IsTrue(stateB.enterCalled);
            Assert.IsTrue(subStateZ.enterCalled);
            Assert.IsTrue(subSubStateV.enterCalled);
        }
        
        


    }
}
