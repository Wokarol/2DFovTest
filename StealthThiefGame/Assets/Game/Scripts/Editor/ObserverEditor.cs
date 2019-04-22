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
        private Observer[] observers;

        private PropertyInfo angle;
        private PropertyInfo radius;

        private void OnEnable() {
            observers = new Observer[targets.Length];
            for (int i = 0; i < observers.Length; i++) {
                observers[i] = targets[i] as Observer;
            }
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
        }

        private void OnSceneGUI() {
            foreach (var observer in observers) {

                var serializedObserver = new SerializedObject(observer);

                float angle = serializedObserver.FindProperty("visionAngle").floatValue;
                float radius = serializedObserver.FindProperty("visionDistance").floatValue;

                Handles.color = new Color(147 / 256, 188 / 256, 1, 0.1f);
                Handles.DrawSolidArc(
                    observer.transform.position,
                    -observer.transform.forward,
                    Quaternion.Euler(0, 0, angle * 0.5f) * observer.transform.up,
                    angle,
                    radius);
            }
        }
    } 
}
