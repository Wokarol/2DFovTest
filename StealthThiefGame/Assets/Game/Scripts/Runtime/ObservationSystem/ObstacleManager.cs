using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class ObstacleManager
{
    static string Filename => SceneManager.GetActiveScene().name + "_corners";

    static long lastSession = 0;

    static List<Vector2> corners = new List<Vector2>();
    public static IReadOnlyList<Vector2> Corners {
        get {
            if ((Application.isPlaying && lastSession != AnalyticsSessionInfo.sessionId)) {
                lastSession = AnalyticsSessionInfo.sessionId;
                GetCornersFromFile();
            }
            return corners;
        }
    }

#if UNITY_EDITOR
    public static void RebuildCorners() {
        ForceObstacleRefresh();
        SaveCornersToFile();
    }

    private static void SaveCornersToFile() {
        var filename = Filename;
        var file = new SavedCorners(corners);
        var json = JsonUtility.ToJson(file);

        string directory = Path.Combine(Application.dataPath, "Resources", "Corners");
        string path = Path.Combine(directory, filename + ".txt");

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        File.WriteAllText(path, json);

        AssetDatabase.Refresh();
    } 
#endif

    private static void GetCornersFromFile() {
        corners.Clear();
        var filename = "Corners/" + Filename;
        TextAsset asset = Resources.Load<TextAsset>(filename);

        var json = asset.text;
        corners = JsonUtility.FromJson<SavedCorners>(json).Corners;
    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
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

    private struct SavedCorners
    {
        public List<Vector2> Corners;

        public SavedCorners(List<Vector2> corners) {
            Corners = corners;
        }
    }
}
