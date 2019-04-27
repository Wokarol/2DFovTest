using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Wokarol
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Observer))]
    public class ObserverEditor : Editor
    {
        private PropertyInfo angle;
        private PropertyInfo radius;

        private void OnEnable() {
            ObstacleManager.ForceObstacleRefresh();
        }

        private void OnSceneGUI() {
            var observer = (Observer)target;
            var serializedObserver = new SerializedObject(observer);

            float angle = serializedObserver.FindProperty("visionAngle").floatValue;
            float radius = serializedObserver.FindProperty("visionDistance").floatValue;
            float resolution = serializedObserver.FindProperty("resolution").floatValue;
            LayerMask mask = serializedObserver.FindProperty("visionMask").intValue;


            var points = FOVUtils.GetPointsFromFOV(angle, radius, resolution, (observer.transform.eulerAngles.z + 90) % 360, observer.transform.position, mask);

            if (angle != 360) {
                points.Insert(0, observer.transform.position);
                points.Add(observer.transform.position);
            }
            Handles.color = new Color(147 / 256, 188 / 256, 1, 1);
            Handles.DrawAAPolyLine(3, points.ToArray());


            if (angle == 360) {
                points.Add(observer.transform.position); 
            }
            Handles.color = new Color(147 / 256, 188 / 256, 1, 0.2f);
            Handles.DrawAAConvexPolygon(points.ToArray());

        }
    }
}
