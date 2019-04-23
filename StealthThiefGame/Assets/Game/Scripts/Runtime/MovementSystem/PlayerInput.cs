using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol
{
    public class PlayerInput : InputData
    {
        private Vector2 movement = default;

        public override Vector2 Movement => movement;

        void Update() {
            movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
    } 
}
