using System.Collections.Generic;
using HFSM;
using NUnit.Framework;
using UnityEditor.UIElements;

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
        public void TransitionSimple() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateB stateB = new StateB();

            int AtoB = 1;
            game.LoadSubState(stateA);
            game.LoadSubState(stateB);
            game.AddTransition(stateA, stateB, AtoB);
            
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
            StateA2 stateA2 = new StateA2();
            StateB stateB = new StateB();
            StateB1 stateB1 = new StateB1();
            StateB2 stateB2 = new StateB2();

            int AtoB = 1;
            game.LoadSubState(stateA);
            game.LoadSubState(stateB);
            stateA.LoadSubState(stateA1);
            stateB.LoadSubState(stateB1);
            stateA1.LoadSubState(stateA2);
            stateB1.LoadSubState(stateB2);
            
            game.AddTransition(stateA, stateB, AtoB);
            
            game.EnterStateMachine();
            stateA2.SendTrigger(AtoB);
            
            Assert.AreEqual(10, callChain.Count);
            Assert.AreEqual("game_enter", callChain[0]);
            Assert.AreEqual("stateA_enter", callChain[1]);
            Assert.AreEqual("stateA1_enter", callChain[2]);
            Assert.AreEqual("stateA2_enter", callChain[3]);
            Assert.AreEqual("stateA2_exit", callChain[4]);
            Assert.AreEqual("stateA1_exit", callChain[5]);
            Assert.AreEqual("stateA_exit", callChain[6]);
            Assert.AreEqual("stateB_enter", callChain[7]);
            Assert.AreEqual("stateB1_enter", callChain[8]);
            Assert.AreEqual("stateB2_enter", callChain[9]);
        }
        
        [Test]
        public void ThrowsExceptionOnDuplicateStateAdded() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateA1 stateA1 = new StateA1();
            StateA2 stateA2 = new StateA2();
            StateB stateB = new StateB();
            StateB1 stateB1 = new StateB1();
            StateB2 stateB2 = new StateB2();
            StateB2 stateB2Duplicate = new StateB2();

            Assert.Throws<DuplicateSubStateException>(() => {
                game.LoadSubState(stateA);
                game.LoadSubState(stateB);
                stateA.LoadSubState(stateA1);
                stateB.LoadSubState(stateB1);
                stateA1.LoadSubState(stateA2);
                stateB1.LoadSubState(stateB2);       
                stateB1.LoadSubState(stateB2Duplicate);
            });
        }

        [Test]
        public void ThrowsExceptionOnDuplicateTransitionAdded() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateA1 stateA1 = new StateA1();
            StateB stateB = new StateB();
            
            game.LoadSubState(stateA);
            game.LoadSubState(stateB);
            game.LoadSubState(stateA1);

            int TRANSITION = 1;
                
            game.AddTransition(stateA, stateB, TRANSITION);
            
            Assert.Throws<DuplicateTransitionException>(() => {
                game.AddTransition(stateA, stateA1, TRANSITION);
            });
        }
        
        [Test]
        public void ThrowsExceptionOnNeglectedTrigger() {
            Game game = new Game();
            StateA stateA = new StateA();
            StateA1 stateA1 = new StateA1();
            StateB stateB = new StateB();
            
            game.LoadSubState(stateA);
            game.LoadSubState(stateB);
            game.LoadSubState(stateA1);

            int TRIGGER_1 = 1;
            int TRIGGER_2_NEVER_ADDED = 2; 
                
            game.AddTransition(stateA, stateB, TRIGGER_1);
                
            game.EnterStateMachine();
            
            Assert.Throws<NeglectedTriggerException>(() => {
                game.SendTrigger(TRIGGER_2_NEVER_ADDED);
            });
        }
    }
}
