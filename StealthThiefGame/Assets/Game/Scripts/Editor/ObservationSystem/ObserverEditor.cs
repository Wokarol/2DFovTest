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
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }

        private void OnSceneGUI() {
            var observer = (Observer)target;
            var serializedObserver = new SerializedObject(observer);

            float angle = serializedObserver.FindProperty("visionAngle").floatValue;
            float radius = serializedObserver.FindProperty("visionDistance").floatValue;
            float resolution = serializedObserver.FindProperty("meshResolution").floatValue;
            LayerMask mask = serializedObserver.FindProperty("visionMask").intValue;

            int edgeResolveIterations = serializedObserver.FindProperty("edgeResolveIterations").intValue;
            float edgeDstThreshold = serializedObserver.FindProperty("edgeDstThreshold").floatValue;

            var points = FOVUtils.GetPointsFromFOV(
                angle,
                radius,
                resolution,
                edgeResolveIterations,
                edgeDstThreshold,
                observer.transform.eulerAngles.z + 90,
                observer.transform.position,
                mask,
                true);

            //Handles.color = Color.blue;
            //foreach (var point in points) {
            //    Handles.DrawLine(observer.transform.position, point);
            //}

            Handles.color = new Color(147 / 256, 188 / 256, 1, 0.3f);
            Handles.DrawAAConvexPolygon(points);
        }
    } 
}
