using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CrazyFischGames.Tools
{
    public class SetRigidbody : EditorWindow
    {

        [MenuItem("Window/Utils/Set Rigidbody Properties")]
        public static void ShowWindow()
        {
            GetWindow<SetRigidbody>("Set Rigidbody Properties");
        }

        private bool _kinematic;
        private bool _gravity;
        private bool _children;
        private RigidbodyInterpolation _interpolation;
        private CollisionDetectionMode _collisionDetection;

        private void OnGUI()
        {
            GUILayout.Label("Set Kinematic");
            _kinematic = EditorGUILayout.Toggle(_kinematic);
            
            GUILayout.Label("Use Gravity");
            _gravity = EditorGUILayout.Toggle(_gravity);
            
            GUILayout.Label("Interpolation Mode");
            _interpolation = (RigidbodyInterpolation) EditorGUILayout.EnumPopup(_interpolation, EditorStyles.popup);
            
            GUILayout.Label("Collision Detection Mode");
            _collisionDetection = (CollisionDetectionMode) EditorGUILayout.EnumPopup(_collisionDetection, EditorStyles.popup);
            
            GUILayout.Label("Include Children");
            _children = EditorGUILayout.Toggle(_children);

            if (GUILayout.Button("Set Properties"))
            {
                var selected = Selection.gameObjects.Select(o => o.transform).ToArray();
                if(selected.Length == 0) return;
                
                foreach (var t in selected)
                {
                    Queue<Transform> queue = new ();
                    queue.Enqueue(t);

                    while (queue.Count > 0)
                    {
                        var current = queue.Dequeue();

                        if (_children)
                        {
                            for (int i = 0; i < current.childCount; i++)
                                queue.Enqueue(current.GetChild(i));
                        }
                        
                        if(current.TryGetComponent<Rigidbody>(out var rb))
                            SetProperties(rb);
                    }
                }
            }
        }

        private void SetProperties(Rigidbody rb)
        {
            rb.isKinematic = _kinematic;
            rb.useGravity = _gravity;
            rb.interpolation = _interpolation;
            rb.collisionDetectionMode = _collisionDetection;
        }
    }
}