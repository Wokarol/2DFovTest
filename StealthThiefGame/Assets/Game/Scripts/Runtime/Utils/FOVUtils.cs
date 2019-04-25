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
    public static Vector2[] GetPointsFromFOV(float fovAngle, float fovDistance, float resolution, float offsetAngle, Vector2 origin, LayerMask mask) {
        var results = new List<Vector2>();

        int stepCount = Mathf.RoundToInt(fovAngle * resolution);
        float stepAngleSize = fovAngle / stepCount;

        for (int i = 0; i <= stepCount; i++) {
            float angle = offsetAngle - fovAngle / 2 + stepAngleSize * i;
            ViewCastInfo castInfo = ViewCast(angle, origin, fovDistance, mask);
            results.Add(castInfo.Point);
        }

        return results.ToArray();
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

    public struct ViewCastInfo
    {
        public readonly bool Hit;
        /// <summary>
        /// Global hit point
        /// </summary>
        public readonly Vector2 Point;
        public readonly float Distance;
        public readonly float Angle;

        public ViewCastInfo(bool hit, Vector2 point, float distance, float angle) {
            Hit = hit;
            Point = point;
            Distance = distance;
            Angle = angle;
        }
    }
}
