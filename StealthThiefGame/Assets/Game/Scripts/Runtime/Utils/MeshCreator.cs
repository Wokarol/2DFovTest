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
    public static void GetIrregularArcFromPoints(ref Mesh mesh, Vector3[] points, Transform parent) {
        mesh.Clear();

        int vertexCount = points.Length + 1;
        Vector3[] verts = new Vector3[vertexCount];
        int[] tris = new int[(vertexCount - 2) * 3];

        verts[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            verts[i + 1] = parent.InverseTransformPoint(points[i]);

            if (i < vertexCount - 2) {
                tris[i * 3] = 0;
                tris[i * 3 + 1] = i + 1;
                tris[i * 3 + 2] = i + 2;
            }
        }

        mesh.vertices = verts;
        mesh.triangles = tris;
    }
}
