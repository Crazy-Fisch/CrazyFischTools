using UnityEditor;
using UnityEngine;

namespace CrazyFischGames.Tools
{
    public class SetPhysicsMaterialWindow : EditorWindow
    {
        [MenuItem("Window/Utils/Set Physics Materials")]
        public static void ShowWindow()
        { 
            GetWindow<SetPhysicsMaterialWindow>("Set Physics Materials");
        }

        private Object material;
        private bool includeChildren;

        void OnGUI()
        {
            GUILayout.Label("Set Physics Material for all colliders of selected Objects");

            material = EditorGUILayout.ObjectField("Physics Material", material, typeof(PhysicMaterial));
            includeChildren = EditorGUILayout.Toggle("Include Children", includeChildren);

            if (GUILayout.Button("Set!"))
            {
                foreach (var obj in Selection.gameObjects)
                {
                    var colliders = !includeChildren ? obj.GetComponents<Collider>() : obj.GetComponentsInChildren<Collider>();
                    foreach (var collider in colliders)
                        collider.material = (PhysicMaterial)material;
                }
            }
        }
    }
}
    