using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshCreator
{
    /// <summary>
    /// Creates local mesh from global points, useful for FOV and similar
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="points"></param>
    /// <param name="origin"></param>
    public static void GetIrregularArcFromPoints(ref Mesh mesh, Vector2[] points, Vector3 origin) {
        int vertexCount = points.Length + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];
    }
}
