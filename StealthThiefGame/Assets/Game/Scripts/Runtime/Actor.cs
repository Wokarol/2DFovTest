using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol
{
    /// <summary>
    /// This class represents a thinking entity inside a gameworld (NPC, Enemies, Player)
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public class Actor : MonoBehaviour, IDetectable
    {
        public Vector2 Position => transform.position;

        new CircleCollider2D collider = default;

        private void Start() {
            collider = GetComponent<CircleCollider2D>();
        }

        public Vector2[] GetEdgePoints (Vector2 direction) {
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            return new Vector2[2] {
                transform.TransformPoint(perpendicular * collider.radius),
                transform.TransformPoint(-perpendicular * collider.radius)
            };
        }
    } 
}
