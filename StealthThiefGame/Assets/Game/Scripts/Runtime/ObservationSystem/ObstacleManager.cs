﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public static class ObstacleManager
{
    static long lastSession = 0;

    static List<Vector2> corners = new List<Vector2>();
    public static IReadOnlyList<Vector2> Corners {
        get {
            if ((Application.isPlaying && lastSession != AnalyticsSessionInfo.sessionId)) {
                lastSession = AnalyticsSessionInfo.sessionId;
                ForceObstacleRefresh();
            }
            return corners;
        }
    }



    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void ForceObstacleRefresh() {
        Debug.Log($"Refreshed obstacles for session {lastSession} and scene {SceneManager.GetActiveScene().buildIndex}");

        var colliders = UnityEngine.Object.FindObjectsOfType<Collider2D>().Where(c => c.gameObject.isStatic).ToArray();
        corners.Clear();

        foreach (var c in colliders) {
            Debug.Log($"Adding for {c.name}");
            corners.AddRange(GetCorners(c));
        }

        Debug.Log(colliders.Length);
        Debug.Log(UnityEngine.Object.FindObjectsOfType<Collider2D>().Length);
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
        if(c is CircleCollider2D circle) {
            float radius = circle.radius;
            var results = new List<Vector2>();
            for (int i = 0; i < 360; i += 10) {
                results.Add(circle.transform.TransformPoint(Vector2Utils.FromAngle(i) * radius));
                //Debug.DrawLine(Vector3.zero, Vector2Utils.FromAngle(i) * radius, Color.white, 20f);
            }
            return results.ToArray();
        }

        return new Vector2[0];
    }
}
