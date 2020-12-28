using System.Collections.Generic;
using NUnit.Framework;
using HFSM;
using UnityEngine;

namespace Tests  {
    
    public class HFSMTests {
        
        static List<string> callChain = new List<string>();

        public static void AddLogCall(string log) {
            callChain.Add(log);
        }

        [SetUp]
        public void Setup() {
            callChain.Clear();
        }

        [Test]
        public void EnterSM() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateA1 stateA1 = new StateA1();
            StateA2 stateA2 = new StateA2();
            StateA3 stateA3 = new StateA3();
            
            game.LoadSubState(stateA);
            stateA.LoadSubState(stateA1);
            stateA.LoadSubState(stateA2);   
            stateA.LoadSubState(stateA3);
            
            game.EnterStateMachine();
            
            Assert.AreEqual(3, callChain.Count);
            Assert.AreEqual("game_enter", callChain[0]);
            Assert.AreEqual("stateA_enter", callChain[1]);
            Assert.AreEqual("stateA1_enter", callChain[2]);
        }

        [Test]
        public void UpdateSM() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateA1 stateA1 = new StateA1();
            StateA2 stateA2 = new StateA2();
            StateA3 stateA3 = new StateA3();
            
            game.LoadSubState(stateA);
            stateA.LoadSubState(stateA1);
            stateA.LoadSubState(stateA2);   
            stateA.LoadSubState(stateA3);
            
            game.EnterStateMachine();
            callChain.Clear();
            
            game.UpdateStateMachine();
            Assert.AreEqual(3, callChain.Count);
            Assert.AreEqual("game_update", callChain[0]);
            Assert.AreEqual("stateA_update", callChain[1]);
            Assert.AreEqual("stateA1_update", callChain[2]);
        }
        
        [Test]
        public void ExitSM() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateA1 stateA1 = new StateA1();
            StateA2 stateA2 = new StateA2();
            StateA3 stateA3 = new StateA3();
            StateB stateB = new StateB();
            StateB1 stateB1 = new StateB1();
            StateB2 stateB2 = new StateB2();
            StateB3 stateB3 = new StateB3();
            
            game.LoadSubState(stateA);
            game.LoadSubState(stateB);
            stateA.LoadSubState(stateA1);
            stateA.LoadSubState(stateA2);   
            stateA.LoadSubState(stateA3);
            stateB.LoadSubState(stateB1);
            stateB.LoadSubState(stateB2);   
            stateB.LoadSubState(stateB3);            
            
            game.EnterStateMachine();
            callChain.Clear();

            game.ChangeSubState<StateB>();
            Assert.AreEqual(4, callChain.Count);
            Assert.AreEqual("stateA1_exit", callChain[0]);
            Assert.AreEqual("stateA_exit", callChain[1]);
            Assert.AreEqual("stateB_enter", callChain[2]);
            Assert.AreEqual("stateB1_enter", callChain[3]);
        }

        [Test]
        public void TransitionSimple() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateB stateB = new StateB();

            int AtoB = 1;
            game.LoadSubState(stateA);
            game.LoadSubState(stateB);
            game.CreateTransition(stateA, stateB, AtoB);
            
            game.EnterStateMachine();
            game.SendTrigger(AtoB);
            
            Assert.AreEqual(4, callChain.Count);
            Assert.AreEqual("game_enter", callChain[0]);
            Assert.AreEqual("stateA_enter", callChain[1]);
            Assert.AreEqual("stateA_exit", callChain[2]);
            Assert.AreEqual("stateB_enter", callChain[3]);
        }
        
        [Test]
        public void TransitionTriggerFromSubState() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateA1 stateA1 = new StateA1();
            StateB stateB = new StateB();
            StateB1 stateB1 = new StateB1();

            int AtoB = 1;
            game.LoadSubState(stateA);
            game.LoadSubState(stateB);
            game.CreateTransition(stateA, stateB, AtoB);
            
            stateA.LoadSubState(stateA1);
            stateB.LoadSubState(stateB1);
            
            game.EnterStateMachine();
            stateA1.SendTrigger(AtoB);
            
            Assert.AreEqual(7, callChain.Count);
            Assert.AreEqual("game_enter", callChain[0]);
            Assert.AreEqual("stateA_enter", callChain[1]);
            Assert.AreEqual("stateA1_enter", callChain[2]);
            Assert.AreEqual("stateA1_exit", callChain[3]);
            Assert.AreEqual("stateA_exit", callChain[4]);
            Assert.AreEqual("stateB_enter", callChain[5]);
            Assert.AreEqual("stateB1_enter", callChain[6]);
        }
        
        [Test]
        public void GetStateHashCode() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateA1 stateA1 = new StateA1();
            StateB stateB = new StateB();
            StateB1 stateB1 = new StateB1();

            List<int> hashCodes = new List<int> {
                game.GetHashCode(),
                stateA.GetHashCode(),
                stateA1.GetHashCode(),
                stateB.GetHashCode(),
                stateB1.GetHashCode()
            };

            int first = hashCodes[0];
            for (int i = 1; i < hashCodes.Count; i++) {
                Assert.AreNotEqual(first, hashCodes[i]);
                Debug.Log(hashCodes[i]);
            }

        }
        
    }
}
