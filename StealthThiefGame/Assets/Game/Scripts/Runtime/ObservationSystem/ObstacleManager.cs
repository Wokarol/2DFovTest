using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObstacleManager
{
    static List<Vector2> corners = new List<Vector2>();
    public static IReadOnlyList<Vector2> Corners => corners;

    public static void ForceObstacleRefresh() {
        var colliders = UnityEngine.Object.FindObjectsOfType<Collider2D>().Where(c => c.gameObject.isStatic).ToArray();
        corners.Clear();

        foreach (var c in colliders) {
            corners.AddRange(GetCorners(c));
        }
    }

    private static Vector2[] GetCorners(Collider2D c) {
        if (c is BoxCollider2D box) {
            Vector2 size = box.size * 0.5f;
            return new Vector2[4] {
                box.transform.TransformPoint(new Vector3( size.x,  size.y)),
                box.transform.TransformPoint(new Vector3(-size.x,  size.y)),
                box.transform.TransformPoint(new Vector3( size.x, -size.y)),
                box.transform.TransformPoint(new Vector3(-size.x, -size.y))
            };
        }

        return new Vector2[0];
    }
}
