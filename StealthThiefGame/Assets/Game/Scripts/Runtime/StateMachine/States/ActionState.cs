using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol.StateMachineSystem
{
    public class ActionState : State
    {
        Func<State> action;

        public ActionState(string name, Func<State> action) {
            Name = name;
            this.action = action ?? throw new ArgumentNullException(nameof(action));
        }

        public ActionState(string name, Action action) {
            Name = name;
            Func<State> nullReturnedAction = () => { action(); return null; };
            this.action = nullReturnedAction ?? throw new ArgumentNullException(nameof(action));
        }

        public override bool CanTransitionToSelf => throw new System.NotImplementedException();

        protected override void EnterProcess(StateMachine stateMachine) {
        }

        protected override void ExitProcess(StateMachine stateMachine) {
        }

        protected override State Process() {
            return action();
        }
    }
}
