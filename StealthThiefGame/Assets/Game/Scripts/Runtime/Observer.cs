using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wokarol
{
    public class Observer : MonoBehaviour
    {
        [SerializeField] float visionAngle = 90;
        [SerializeField] float visionDistance = 5;
        [SerializeField] LayerMask visionMask;

        private void Update() {
            CheckSurrounding();
        }

        /// <summary>
        /// Test if any observable is in view
        /// </summary>
        private void CheckSurrounding() {
            var colliders = Physics2D.OverlapCircleAll(transform.position, visionDistance);
            foreach (var c in colliders) {
                var detectables = c.GetComponents<IDetectable>();
                foreach (var detectable in detectables) {
                    if (CheckIfDetectableIsInFOV(detectable)) {

                        // TODO: Logic
                        Debug.DrawLine(transform.position, detectable.Position);

                    }
                }
            }
        }

        /// <summary>
        /// Checks if given detectable is in Field of View of the observer, takes angle and line of sight into account
        /// </summary>
        /// <param name="detectable"></param>
        /// <returns></returns>
        private bool CheckIfDetectableIsInFOV(IDetectable detectable) {
            // Positions
            Vector2 pos = transform.position;
            Vector2 detectableCentre = detectable.Position;

            // Vectors
            Vector2 difference = detectableCentre - pos;
            Vector2 direction = difference.normalized;

            // Checks if centre of detectable is in view
            if (PointInView(pos, direction, difference.sqrMagnitude)) {
                return true;
            }

            // Chekcs if any point on the detectable's edge is view
            foreach (var p in detectable.GetEdgePoints(-direction)) {
                Vector2 vectorToPoint = (p - pos);
                if (PointInView(pos, vectorToPoint.normalized, vectorToPoint.sqrMagnitude)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if point is in view, takes angle and line of sight into account
        /// </summary>
        /// <param name="source">Point used as view centre</param>
        /// <param name="direction">direction to point</param>
        /// <param name="sqrDistanceToTarget">squared distance to a point</param>
        /// <returns></returns>
        private bool PointInView(Vector2 source, Vector2 direction, float sqrDistanceToTarget) {
            if (Vector2.Angle(direction, transform.up) < visionAngle * 0.5f) {
                var hit = Physics2D.Raycast(source, direction, sqrDistanceToTarget, visionMask);
                if (hit.transform == null ||
                    hit.distance * hit.distance > sqrDistanceToTarget) {
                    return true;
                }
            }
            return false;
        }

        private void OnDrawGizmosSelected() {
        }
    }
}
