using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wokarol.StateMachineSystem;

namespace Wokarol.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ActorMovement : MonoBehaviour
    {
        [SerializeField] InputData input = default;
        [Space]
        [SerializeField] float speed = 5;

        StateMachine movementMachine;
        public DebugBlock DebugBlock { get; } = new DebugBlock("Movement");

        new Rigidbody2D rigidbody;
        Rigidbody2D Rigidbody {
            get {
                if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
                return rigidbody;
            }
        }

        private void Start() {
            var movement = new ActionState("Walking", () => Rigidbody.velocity = input.Movement * speed);
            var hiding = new ActionState("Hiding", () => Rigidbody.velocity = input.Movement * speed * .5f);

            movement.AddTransition(() => input.Hide, hiding);
            hiding.AddTransition(() => !input.Hide, movement);

            movementMachine = new StateMachine(movement, DebugBlock);
        }

        private void OnValidate() {
            if (input == null) input.GetComponent<InputData>();
        }

        private void FixedUpdate() {
            movementMachine.Tick();
            //Rigidbody.velocity = input.Movement * speed;
        }
    }
}
