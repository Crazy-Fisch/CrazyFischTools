using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrazyFischGames.Editor.Scenes
{
    public class CreateSceneWindow : EditorWindow
    {
        [MenuItem("Scenes/>Create New Scene<", false, 0)]
        public static void ShowWindow()
        {
            GetWindow<CreateSceneWindow>("Create Scene");
        }

        private string _name;
        
        public void OnGUI()
        {
            _name = GUILayout.TextField(_name);
            if (GUILayout.Button("Create"))
            {
                Scene sceneAsset = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
                string path = $"{Path.Combine(Application.dataPath, LoadScene.SCENE_ASSET_PATH)}{_name}.unity";
                EditorSceneManager.SaveScene(sceneAsset, path);
                LoadScene.LoadAllSceneAssets();
            }
        }
    }

    public class LoadScene
    {
        public const string SCENE_ASSET_PATH = "_MyProject/Scenes/";
        private static string SCENE_MENUS_FILE = Path.Combine(Application.dataPath, "_MyProject/Editor/SceneMenus.cs");

        private const string PATTERN = "[^a-zA-Z0-9_]";

        public static void SaveCurrentScene()
        {
            string current = SceneManager.GetActiveScene().path;
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), current);
        }
        
        [MenuItem("Scenes/>Load Scene Assets<", false, 0)]
        public static void LoadAllSceneAssets()
        {
            SaveCurrentScene();
            
            if(File.Exists(SCENE_MENUS_FILE))
                File.Delete(SCENE_MENUS_FILE);

            string assetPath = Path.Combine(Application.dataPath, SCENE_ASSET_PATH);
            string[] sceneAssets = Directory.GetFiles(assetPath, "*.unity");
            
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using UnityEditor;");
            sb.AppendLine("using UnityEditor.SceneManagement;");
            sb.AppendLine("");
            sb.AppendLine("namespace CrazyFischGames.Editor.Scenes");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic class SceneMenus");
            sb.AppendLine("\t{");
            
            foreach (var sceneAsset in sceneAssets)
            {
                int start = assetPath.Length;
                int len = (sceneAsset.Length - ".unity".Length) - start;
                string sceneName = sceneAsset.Substring(start, len);
                string varName = Regex.Replace(sceneName, PATTERN, "").ToUpper();
                string scenePath = "Assets/" + SCENE_ASSET_PATH + sceneName + ".unity";
                
                sb.AppendLine($"\t\tprivate const string SCENE_PATH_{varName} = \"{scenePath}\";");
            }

            foreach (var sceneAsset in sceneAssets)
            {
                int start = assetPath.Length;
                int len = (sceneAsset.Length - ".unity".Length) - start;
                string sceneName = sceneAsset.Substring(start, len);
                string varName = Regex.Replace(sceneName, PATTERN, "").ToUpper();
                
                sb.AppendLine("");
                sb.AppendLine($"\t\t[MenuItem(\"Scenes/{sceneName}\")]");
                sb.AppendLine($"\t\tpublic static void LoadScene{varName}()");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tLoadScene.SaveCurrentScene();");
                sb.AppendLine($"\t\t\tEditorSceneManager.OpenScene(SCENE_PATH_{varName});");
                sb.AppendLine("\t\t}");
            }

            sb.AppendLine("");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            
            try
            {
                StreamWriter writer = new StreamWriter(new FileStream(SCENE_MENUS_FILE, FileMode.CreateNew));
                writer.Write(sb.ToString().Normalize(NormalizationForm.FormC));
                writer.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            
            AssetDatabase.Refresh();
        }
    }
}