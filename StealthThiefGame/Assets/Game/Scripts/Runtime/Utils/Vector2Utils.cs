using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Utils
{
    /// <summary>
    /// Gets direction from angle
    /// </summary>
    /// <param name="angle">Angle (in degrees)</param>
    /// <returns></returns>
    public static Vector2 FromAngle(float angle) {
        angle = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }
}
