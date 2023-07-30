#if ADDRESSABLES

using UnityEditor;
using UnityEngine;

namespace CrazyFischGames.Tools
{
    public class SetAddresableLabel : EditorWindow
    {
	
        [MenuItem("Window/Utils/Set Addressable Labels")]
        public static void ShowWindow()
        {
            GetWindow<SetAddresableLabel>("Set labels for selection");
        }

        private const char SEPERATOR = ';';
        private string _labels;
        
        private void OnGUI()
        {
            GUILayout.Label("This operation does not replace other labels");
            GUILayout.Label($"Label(s), split with '{SEPERATOR}'");
            _labels = GUILayout.TextField(_labels);

            var selections = Selection.gameObjects;
            
            if (GUILayout.Button("Set Label" + (selections.Length > 1 ? "s" : "")))
            {
                if (selections.Length == 0)
                {
                    Debug.Log("No object selected !");
                    return;
                }
				
                foreach(var selection in selections)
                {
                    var entry = AddressableEditorUtils.GetAddressableAssetEntry(selection);
                    var labels = _labels.Split(SEPERATOR);
                    AddressableEditorUtils.SetAddressableEntryLabels(entry, labels);
                }

                AssetDatabase.SaveAssets();
                EditorUtility.FocusProjectWindow();
            }
        }
    }
}

#endif