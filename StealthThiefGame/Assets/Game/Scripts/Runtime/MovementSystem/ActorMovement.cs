using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ActorMovement : MonoBehaviour
    {
        [SerializeField] InputData input = default;
        [Space]
        [SerializeField] float speed = 5;

        new Rigidbody2D rigidbody;
        Rigidbody2D Rigidbody {
            get {
                if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
                return rigidbody;
            }
        }

        private void OnValidate() {
            if (input == null) input.GetComponent<InputData>();
        }

        private void FixedUpdate() {
            Rigidbody.velocity = input.Movement * speed;
        }
    }
}
