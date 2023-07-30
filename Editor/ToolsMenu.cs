using UnityEditor;
using static UnityEditor.AssetDatabase;

namespace CrazyFischGames.Tools
{
    public static class ToolsMenu
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        private static void CreateDefaultFolders()
        {
            Folders.CreateDirectories("_MyProject", "Materials", "Animations", "Scenes", "Scripts", "Textures", "Prefabs", "Models", "Sounds", "Sounds/Music", "Sounds/SFX", "Sprites");
            Refresh();
        }

        [MenuItem("Tools/Setup/Load Default VR Manifest")]
        private static async void LoadDefaultVRManifest() => await Packages.ReplacePackagesFromGist("d7dd2dadbf0b70b1aa59b84b46da343f");
        
        [MenuItem("Tools/Setup/Load SteamVR Manifest")]
        private static async void LoadSteamVRManifest() => await Packages.ReplacePackagesFromGist("c060954b17aac81fb5177ab176f87b8e");
        
        [MenuItem("Tools/Setup/Load Default 3D Manifest")]
        private static async void LoadDefault3DManifest() => await Packages.ReplacePackagesFromGist("02e878f787cb2677cd864a3aebefdf37");

        [MenuItem("Tools/Setup/Packages/New Input System")]
        private static void AddNewInputSystem() => Packages.InstallUnityPackage("inputsystem");
        
        [MenuItem("Tools/Setup/Packages/ProBuilder")]
        private static void AddProBuilder() => Packages.InstallUnityPackage("probuilder");
        
        [MenuItem("Tools/Setup/Packages/Cinemachine")]
        private static void AddCinemachine() => Packages.InstallUnityPackage("cinemachine");        
        
        [MenuItem("Tools/Setup/Packages/URP")]
        private static void AddURP()
        {
            Packages.InstallUnityPackage("render-pipelines.universal");
            Folders.CreateDirectories("_MyProject", "Rendering", "Shaders", "Materials/Skybox", "Textures/HDRIs");
        }

        [MenuItem("Tools/Setup/Packages/HDRP")]
        private static void AddHDRP()
        {
            Packages.InstallUnityPackage("render-pipelines.high-definition");
            Folders.CreateDirectories("_MyProject", "Rendering", "Shaders", "Materials/Skybox", "Textures/HDRIs");
        }
    }
}
