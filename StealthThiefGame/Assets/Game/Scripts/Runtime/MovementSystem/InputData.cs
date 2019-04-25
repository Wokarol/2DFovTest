using UnityEngine;

namespace Wokarol
{
    public abstract class InputData : MonoBehaviour
    {
        public abstract Vector2 Movement { get; }
        public abstract bool Hide { get; }
    } 
}