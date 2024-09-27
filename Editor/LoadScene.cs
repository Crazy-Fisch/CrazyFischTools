using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrazyFischGames.Editor
{
    public class LoadScene
    {
        private const string SCENE_ASSET_PATH = "_MyProject/Scenes/";
        private const string SCENE_MENU_PATH = "_MyProject/Editor/SceneMenus";

        public static void SaveCurrentScene()
        {
            string current = SceneManager.GetActiveScene().path;
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene(), current);
        }
        
        [MenuItem("Scenes/>Load Scene Assets<")]
        public static void LoadAllSceneAssets()
        {
            SaveCurrentScene();
            
            string path = Path.Combine(Application.dataPath, SCENE_MENU_PATH);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            string[] sceneMenuFiles = Directory.GetFiles(path);
            
            foreach (var sceneMenuFile in sceneMenuFiles)
            {
                File.Delete(sceneMenuFile);
            }

            string assetPath = Path.Combine(Application.dataPath, SCENE_ASSET_PATH);
            string[] sceneAssets = Directory.GetFiles(assetPath, "*.unity");
            
            foreach (var sceneAsset in sceneAssets)
            {
                int start = assetPath.Length;
                int len = (sceneAsset.Length - ".unity".Length) - start;
                string sceneName = sceneAsset.Substring(start, len);

                string scenePath = "Assets/" + SCENE_ASSET_PATH + sceneName + ".unity";
                
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("using UnityEditor;");
                sb.AppendLine("using UnityEditor.SceneManagement;");
                sb.AppendLine("");
                sb.AppendLine("namespace CrazyFischGames.Editor.SceneMenus");
                sb.AppendLine("{");
                sb.AppendLine($"\tpublic class LoadScene{sceneName}");
                sb.AppendLine("\t{");
                sb.AppendLine($"\t\tprivate const string SCENE_PATH = \"{scenePath}\";");
                sb.AppendLine($"\t\t[MenuItem(\"Scenes/{sceneName}\")]");
                sb.AppendLine("\t\tpublic static void LoadSceneInEditor()");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tLoadScene.SaveCurrentScene();");
                sb.AppendLine("\t\t\tEditorSceneManager.OpenScene(SCENE_PATH);");
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");
                sb.AppendLine("}");

                try
                {
                    StreamWriter writer = new StreamWriter(Path.Combine(path, $"LoadScene{sceneName}.cs"));
                    writer.Write(sb.ToString().Normalize(NormalizationForm.FormC));
                    writer.Close();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Could not create SceneMenu for {sceneName} !");
                    Debug.Log(e.Message);
                }
            }
            
            AssetDatabase.Refresh();
        }
    }
}