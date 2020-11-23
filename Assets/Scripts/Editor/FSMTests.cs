using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FSMTests
    {
        
        public List<FSM.State> enteredList = new List<FSM.State>();
        public List<FSM.State> exitedList = new List<FSM.State>();
        
        public class StateA : FSM.State {
            public List<FSM.State> enteredList;
            public List<FSM.State> exitedList;
            
            public StateA(List<FSM.State> enteredList, List<FSM.State> exitedList) {
                this.enteredList = enteredList;
                this.exitedList = exitedList;
            }
            public override void Enter() { enteredList.Add(this); }
            public override void Exit() { exitedList.Add(this); }
        }
        
        public class StateB : FSM.State {
            public List<FSM.State> enteredList;
            public List<FSM.State> exitedList;
            
            public StateB(List<FSM.State> enteredList, List<FSM.State> exitedList) {
                this.enteredList = enteredList;
                this.exitedList = exitedList;
            }
            public override void Enter() { enteredList.Add(this); }
            public override void Exit() { exitedList.Add(this); }
        }
        
        public class SubStateX : FSM.State {
            public List<FSM.State> enteredList;
            public List<FSM.State> exitedList;
            
            public SubStateX(List<FSM.State> enteredList, List<FSM.State> exitedList) {
                this.enteredList = enteredList;
                this.exitedList = exitedList;
            }
            public override void Enter() { enteredList.Add(this); }
            public override void Exit() { exitedList.Add(this); }
        }
        
        public class SubStateY : FSM.State {
            public List<FSM.State> enteredList;
            public List<FSM.State> exitedList;
            
            public SubStateY(List<FSM.State> enteredList, List<FSM.State> exitedList) {
                this.enteredList = enteredList;
                this.exitedList = exitedList;
            }
            public override void Enter() { enteredList.Add(this); }
            public override void Exit() { exitedList.Add(this); }
        }
        
        public class SubStateZ : FSM.State {
            public List<FSM.State> enteredList;
            public List<FSM.State> exitedList;
            
            public SubStateZ(List<FSM.State> enteredList, List<FSM.State> exitedList) {
                this.enteredList = enteredList;
                this.exitedList = exitedList;
            }
            public override void Enter() { enteredList.Add(this); }
            public override void Exit() { exitedList.Add(this); }
        }
        
        public class SubSubStateV : FSM.State {
            public List<FSM.State> enteredList;
            public List<FSM.State> exitedList;
            
            public SubSubStateV(List<FSM.State> enteredList, List<FSM.State> exitedList) {
                this.enteredList = enteredList;
                this.exitedList = exitedList;
            }
            public override void Enter() { enteredList.Add(this); }
            public override void Exit() { exitedList.Add(this); }
        }

        public class SubSubStateU : FSM.State {
            public List<FSM.State> enteredList;
            public List<FSM.State> exitedList;
            
            public SubSubStateU(List<FSM.State> enteredList, List<FSM.State> exitedList) {
                this.enteredList = enteredList;
                this.exitedList = exitedList;
            }
            public override void Enter() { enteredList.Add(this); }
            public override void Exit() { exitedList.Add(this); }
        }

        
        [SetUp]
        public void Setup() {
            FSM.Clear();
            enteredList.Clear();
            exitedList.Clear();
        }
        
        [Test]
        public void SetupStateHirarchy()
        {
            var stateA = new StateA(enteredList, exitedList);
            var subStateX = new SubStateX(enteredList, exitedList);
            var subStateY = new SubStateY(enteredList, exitedList);
            var subSubStateU = new SubSubStateU(enteredList, exitedList);
            
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
            var stateA = new StateA(enteredList, exitedList);
            var subStateX = new SubStateX(enteredList, exitedList);
            var subStateY = new SubStateY(enteredList, exitedList);
            var subSubStateU = new SubSubStateU(enteredList, exitedList);
            
            FSM.LoadState(stateA);
            FSM.LoadState(subStateX);
            FSM.LoadState(subStateY);
            FSM.LoadState(subSubStateU);
            
            FSM.SetSubState<StateA, SubStateX>();
            FSM.SetSubState<StateA, SubStateY>();
            FSM.SetSubState<SubStateX, SubSubStateU>();
            
            FSM.GoToState<StateA>();
            
            Assert.AreEqual(stateA, enteredList[0]);
            Assert.AreEqual(subStateX, enteredList[1]);
            Assert.AreEqual(subSubStateU, enteredList[2]);
        }
        
        [Test]
        public void ExitStateHirarchy()
        {
            var stateA = new StateA(enteredList, exitedList);
            var subStateX = new SubStateX(enteredList, exitedList);
            var subStateY = new SubStateY(enteredList, exitedList);
            var subSubStateU = new SubSubStateU(enteredList, exitedList);
            
            var stateB = new StateB(enteredList, exitedList);
            var subStateZ = new SubStateZ(enteredList, exitedList);
            var subSubStateV = new SubSubStateV(enteredList, exitedList);

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
            Assert.IsTrue(exitedList[0] == subSubStateU);
            Assert.IsTrue(exitedList[1] == subStateX);
            Assert.IsTrue(exitedList[2] == stateA);
        }
        
        public class SuperState : FSM.State {
            public List<FSM.State> enteredList;
            public List<FSM.State> exitedList;
            
            public SuperState(List<FSM.State> enteredList, List<FSM.State> exitedList) {
                this.enteredList = enteredList;
                this.exitedList = exitedList;
            }
            public override void Enter() { enteredList.Add(this); }
            public override void Exit() { exitedList.Add(this); }
        }
        
        [Test]
        public void SwitchStateTreeWithCommonParent()
        {
            var superState = new SuperState(enteredList, exitedList);
            var stateA = new StateA(enteredList, exitedList);
            var subStateX = new SubStateX(enteredList, exitedList);
            var subSubStateU = new SubSubStateU(enteredList, exitedList);
            
            var stateB = new StateB(enteredList, exitedList);
            var subStateZ = new SubStateZ(enteredList, exitedList);
            var subSubStateV = new SubSubStateV(enteredList, exitedList);

            FSM.LoadState(superState);
            
            FSM.LoadState(stateA);
            FSM.LoadState(subStateX);
            FSM.LoadState(subSubStateU);
            
            FSM.LoadState(stateB);
            FSM.LoadState(subStateZ);
            FSM.LoadState(subSubStateV);
            
            // Branch A
            FSM.SetSubState<SuperState, StateA>();
            FSM.SetSubState<StateA, SubStateX>();
            FSM.SetSubState<SubStateX, SubSubStateU>();
            
            //Branch B
            FSM.SetSubState<SuperState, StateB>();
            FSM.SetSubState<StateB, SubStateZ>();
            FSM.SetSubState<SubStateZ, SubSubStateV>();
            
            FSM.GoToState<SuperState>();
            
            Assert.IsTrue(FSM.CurrentState == subSubStateU);
            
            enteredList.Clear();
            
            FSM.GoToState<SubSubStateV>();
            // Make sure all states has been exited, except the superstate which is the common parent.
            Assert.AreEqual(3, exitedList.Count);
            Assert.AreEqual(subSubStateU, exitedList[0]);
            Assert.AreEqual(subStateX ,exitedList[1]);
            Assert.AreEqual(stateA, exitedList[2]);
            
            // Make sure only the subbranch has been entered
            Assert.AreEqual(3, enteredList.Count);
            Assert.AreEqual(stateB, enteredList[0]);
            Assert.AreEqual(subStateZ, enteredList[1]);
            Assert.AreEqual(subSubStateV, enteredList[2]);
            
        }        


    }
}
