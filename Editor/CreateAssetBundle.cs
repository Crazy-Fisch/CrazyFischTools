using System.IO;
using UnityEditor;
using UnityEngine;
#if ADDRESSABLES
using UnityEditor.AddressableAssets.Settings;
#endif

namespace CrazyFischGames.Tools
{
	public class CreateAssetBundle
	{
		[MenuItem("Assets/Build AssetBundles")]
		public static void BuildAllAssetBundles()
		{
			string directory = "Assets/StreamingAssets/AssetBundles";
			if (!Directory.Exists(Application.streamingAssetsPath)) Directory.CreateDirectory(directory);

			BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.UncompressedAssetBundle, EditorUserBuildSettings.activeBuildTarget);
			//BuildPipeline.BuildAssetBundles(directory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget); //For real builds
		}

		//Add "ADDRESSABLES" as a Scripting Define Symbol
		#if ADDRESSABLES
		[MenuItem("Assets/Build Addressables")]
		public static void BuildAllAddressables()
		{
			AddressableAssetSettings.BuildPlayerContent();
		}
		#endif
	}
}
