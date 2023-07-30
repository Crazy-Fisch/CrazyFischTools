using UnityEditor;
using UnityEngine;

namespace CrazyFischGames.Tools
{
    public class SetMaterialToSelectedWindow : EditorWindow
    {
        [MenuItem("Window/Utils/Set Material to Selected")]
        public static void ShowWindow()
        { 
            GetWindow<SetMaterialToSelectedWindow>("Set Material to Selected GameObjects");
        }

        private Object material;
        private bool includeChildren;

        void OnGUI()
        {
            GUILayout.Label("Set the Material for all selected GameObjects");

            material = EditorGUILayout.ObjectField("Material", material, typeof(Material));
            includeChildren = EditorGUILayout.Toggle("Include Children", includeChildren);

            if (GUILayout.Button("Set!"))
            {
                foreach (var obj in Selection.gameObjects)
                {
                    if(!includeChildren) obj.GetComponent<Renderer>().material = (Material)material;
                    else
                    {
                        var all = obj.GetComponentsInChildren<Renderer>();
                        foreach (var renderer in all)
                            renderer.material = (Material) material;
                    }
                }
            }
        }
    }
}
