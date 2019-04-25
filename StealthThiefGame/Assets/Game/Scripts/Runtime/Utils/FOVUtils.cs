using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FOVUtils
{
    /// <summary>
    /// Gets local points of FOV
    /// </summary>
    /// <param name="fovAngle">angle of field of view (degrees)</param>
    /// <param name="fovDistance">distance of field of view</param>
    /// <param name="resolution">resolution of field of view</param>
    /// <param name="offsetAngle">rotation of object in degress (0 means facing right, 90 means up)</param>
    /// <param name="origin">origin of field of view</param>
    /// <returns>list of local points</returns>
    public static Vector3[] GetPointsFromFOV
        (float fovAngle, float fovDistance, float resolution, int edgeResolveIterations, float edgeDstThreshold, 
        float offsetAngle, Vector2 origin, LayerMask mask, bool addOriginInFront = false) 
    {
        var results = new List<Vector3>();
        if (addOriginInFront) results.Add(origin);

        int stepCount = Mathf.RoundToInt(fovAngle * resolution);
        float stepAngleSize = fovAngle / stepCount;

        ViewCastInfo oldCast = new ViewCastInfo();

        for (int i = 0; i <= stepCount; i++) {
            float angle = offsetAngle - fovAngle / 2 + stepAngleSize * i;
            ViewCastInfo castInfo = ViewCast(angle, origin, fovDistance, mask);

            if(i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs(castInfo.Distance - oldCast.Distance) > edgeDstThreshold;

                if (oldCast.Hit != castInfo.Hit || (oldCast.Hit && castInfo.Hit && edgeDstThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldCast, castInfo, edgeResolveIterations, edgeDstThreshold, origin, fovDistance, mask);
                    if (edge.PointA != Vector3.zero) results.Add(edge.PointA);
                    if (edge.PointB != Vector3.zero) results.Add(edge.PointB);
                }
            }

            results.Add(castInfo.Point);
            oldCast = castInfo;
        }

        return results.ToArray();
    }

    static EdgeInfo FindEdge(ViewCastInfo min, ViewCastInfo max, int edgeResolveIterations, float edgeDstThreshold, Vector2 origin, float fovRadius, LayerMask mask) {
        float minAngle = min.Angle; // in degrees
        float maxAngle = max.Angle; // in degrees

        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) * .5f;
            ViewCastInfo castInfo = ViewCast(angle, origin, fovRadius, mask);

            bool edgeDstThresholdExceeded = Mathf.Abs(castInfo.Distance - min.Distance) > edgeDstThreshold;
            if (castInfo.Hit == min.Hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = castInfo.Point;
            } else {
                maxAngle = angle;
                maxPoint = castInfo.Point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="angle">global angle (in degrees)</param>
    /// <returns></returns>
    static ViewCastInfo ViewCast(float angle, Vector2 origin, float fovRadius, LayerMask mask) {
        Vector2 dir = Vector2Utils.FromAngle(angle * Mathf.Deg2Rad);
        var hit = Physics2D.Raycast(origin, dir, fovRadius, mask);

        if(hit.transform != null) {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        } else {
            return new ViewCastInfo(false, origin + dir * fovRadius, fovRadius, angle);
        }
    }

    public struct EdgeInfo
    {
        public readonly Vector3 PointA;
        public readonly Vector3 PointB;

        public EdgeInfo(Vector3 pointA, Vector3 pointB) {
            PointA = pointA;
            PointB = pointB;
        }
    }

    public struct ViewCastInfo
    {
        public readonly bool Hit;
        /// <summary>
        /// Global hit point
        /// </summary>
        public readonly Vector2 Point;
        public readonly float Distance;
        /// <summary>
        /// (in degrees)
        /// </summary>
        public readonly float Angle;

        public ViewCastInfo(bool hit, Vector2 point, float distance, float angle) {
            Hit = hit;
            Point = point;
            Distance = distance;
            Angle = angle;
        }
    }
}
