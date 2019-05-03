using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class FOVUtils
{
    /// <summary>
    /// Gets global points of FOV
    /// </summary>
    /// <param name="fovAngle">angle of field of view (degrees)</param>
    /// <param name="fovDistance">distance of field of view</param>
    /// <param name="resolution">Resolution in rays per degree</param>
    /// <param name="offsetAngle">rotation of object in degress (0 means facing right, 90 means up)</param>
    /// <param name="origin">origin of field of view</param>
    /// <returns>list of global points</returns>
    public static List<Vector3> GetPointsFromFOV(float fovAngle, float fovDistance, float resolution, float offsetAngle, Vector2 origin, LayerMask mask) {
        var results = new List<Vector3>();
        var arcPoints = GetPointsInArc(fovAngle, fovDistance, offsetAngle, origin, Mathf.RoundToInt(fovAngle * resolution));
        var corners = GetFilteredPoints(fovAngle, fovDistance, offsetAngle, origin);
        var forward = Vector2Utils.FromAngle(offsetAngle);

        bool[] arcHits = new bool[arcPoints.Count];

        // Getting all points along the arc
        for (int i = 0; i < arcPoints.Count; i++) {
            Vector2 p = arcPoints[i];
            results.Add(Raycast(origin, (p - origin).normalized, fovDistance, mask, out bool hitted));
            arcHits[i] = hitted;
        }

        // Getting all points for corners
        foreach (var c in corners) {
            Vector2 dir = (c - origin).normalized;
            AddByAngle(origin, forward, results, Raycast(origin, Quaternion.Euler(0, 0, 0.05f) * dir, fovDistance, mask, out bool hitted));
            AddByAngle(origin, forward, results, Raycast(origin, dir, fovDistance, mask, out hitted));
            AddByAngle(origin, forward, results, Raycast(origin, Quaternion.Euler(0, 0, -0.05f) * dir, fovDistance, mask, out hitted));
        }

        for (int i = 0; i < arcHits.Length - 1; i++) {
            if (arcHits[i] != arcHits[i + 1]) {
                Vector2 edgePoint;
                bool hit;
                if (!arcHits[i] && arcHits[i + 1]) {
                    // Edge to right of arc Point i
                    edgePoint = Raycast(arcPoints[i], arcPoints[i + 1] - arcPoints[i], Vector2.Distance(arcPoints[i], arcPoints[i + 1]), mask, out hit);
                } else {
                    // Edge to left of arc Point i + 1
                    edgePoint = Raycast(arcPoints[i + 1], arcPoints[i] - arcPoints[i + 1], Vector2.Distance(arcPoints[i], arcPoints[i + 1]), mask, out hit);
                }

                if (!hit) continue;

                Vector2 hitPoint = Raycast(origin, edgePoint - origin, Vector2.Distance(origin, edgePoint), mask, out hit);
                if (hit) {
                    AddByAngle(origin, forward, results, hitPoint);
                } else {
                    AddByAngle(origin, forward, results, edgePoint);
                }
            }
        }

        return results;
    }

    private static Vector2 Raycast(Vector2 origin, Vector2 dir, float distance, LayerMask mask, out bool hitted) {
        RaycastHit2D hit = Physics2D.Raycast(origin, dir, distance, mask);
        hitted = hit.transform != null;
        if (hitted) {
            return hit.point;
        } else {
            return dir * distance + origin;
        }

    }

    public static List<Vector2> GetFilteredPoints(float fovAngle, float fovDistance, float offsetAngle, Vector2 origin) {
        var results = new List<Vector2>();
        var forward = Vector2Utils.FromAngle(offsetAngle);
        float distanceSqr = fovDistance * fovDistance;
        float halfAngle = fovAngle * 0.5f;

        foreach (var p in ObstacleManager.Corners) {
            Vector2 direction = (p - origin);
            if (Vector2.Angle(direction, forward) < halfAngle && direction.sqrMagnitude < distanceSqr)
                results.Add(p);
        }

        return results;
    }

    public static List<Vector2> GetPointsInArc(float fovAngle, float fovDistance, float offsetAngle, Vector2 origin, int resolution) {
        var results = new List<Vector2>();
        float startAngle = offsetAngle + fovAngle * 0.5f;

        for (int i = 0; i <= resolution; i++) {
            float angle = startAngle - i * (fovAngle / resolution);
            results.Add(Vector2Utils.FromAngle(angle) * fovDistance + origin);
        }

        return results;
    }

    private static void AddByAngle(Vector3 origin, Vector3 middle, List<Vector3> list, Vector3 v) {
        float myAngle = Vector2.SignedAngle(v - origin, middle);
        for (int i = 0; i < list.Count; i++) {
            float prevAngle = Vector2.SignedAngle(list[i] - origin, middle);
            if (myAngle <= prevAngle) {
                list.Insert(i, v);
                return;
            }
        }
    }
}
