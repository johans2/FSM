using System.Collections.Generic;
using NUnit.Framework;
using HFSM;

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
            
            game.LoadState(stateA);
            stateA.LoadState(stateA1);
            stateA.LoadState(stateA2);   
            stateA.LoadState(stateA3);
            
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
            
            game.LoadState(stateA);
            stateA.LoadState(stateA1);
            stateA.LoadState(stateA2);   
            stateA.LoadState(stateA3);
            
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
            
            game.LoadState(stateA);
            game.LoadState(stateB);
            stateA.LoadState(stateA1);
            stateA.LoadState(stateA2);   
            stateA.LoadState(stateA3);
            stateB.LoadState(stateB1);
            stateB.LoadState(stateB2);   
            stateB.LoadState(stateB3);            
            
            game.EnterStateMachine();
            callChain.Clear();

            game.GoToState<StateB>();
            Assert.AreEqual(4, callChain.Count);
            Assert.AreEqual("stateA1_exit", callChain[0]);
            Assert.AreEqual("stateA_exit", callChain[1]);
            Assert.AreEqual("stateB_enter", callChain[2]);
            Assert.AreEqual("stateB1_enter", callChain[3]);
        }
        
        

    }
}
