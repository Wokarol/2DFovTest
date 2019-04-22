using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol
{
    public class Observer : MonoBehaviour
    {
        [SerializeField] float visionAngle = 90;
        [SerializeField] float visionDistance = 5;

        private void OnDrawGizmosSelected() {
        }
    } 
}
