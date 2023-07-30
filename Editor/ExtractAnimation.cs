using UnityEditor;
using UnityEngine;

namespace CrazyFischGames.Tools
{
	public class ExtractAnimation : EditorWindow
	{
		
		[MenuItem("Window/Utils/Extract Animation Clip")]
		public static void ShowWindow()
		{
			GetWindow<ExtractAnimation>("Extract Animation file from FBX");
		}

		private bool _markHumanoid;
		private bool _loop;
		private bool _bakeRotation;
		private bool _bakePositionY;
		private bool _bakePositionXZ;

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Make sure AnimationType is correctly selected (eg. Humanoid)");
			//_markHumanoid = EditorGUILayout.Toggle(_markHumanoid);
			
			EditorGUILayout.LabelField("Set Loop");
			_loop = EditorGUILayout.Toggle(_loop);
			
			//EditorGUILayout.LabelField("Bake Rotation");
			//_bakeRotation = EditorGUILayout.Toggle(_bakeRotation);
			//
			//EditorGUILayout.LabelField("Bake Position Y");
			//_bakePositionY = EditorGUILayout.Toggle(_bakePositionY);
			//
			//EditorGUILayout.LabelField("Bake Position XZ");
			//_bakePositionXZ = EditorGUILayout.Toggle(_bakePositionXZ);
			
			GUILayout.Space(10f);
			if (GUILayout.Button("Extract"))
			{
				var guids = Selection.assetGUIDs;
				if (guids.Length == 0)
				{
					Debug.Log("No Assets selected !");
					return;
				}

				foreach (var guid in guids)
				{
					var fbxPath = AssetDatabase.GUIDToAssetPath(guid);
					var animPath = fbxPath.Substring(0, fbxPath.Length - 4) + ".anim";
			
					AnimationClip ogClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(fbxPath);
					SerializedObject serializedClip = new SerializedObject(ogClip);

					var clipSettings = AnimationUtility.GetAnimationClipSettings(ogClip);
					clipSettings.loopTime = _loop;
					AnimationUtility.SetAnimationClipSettings(ogClip, clipSettings);

					serializedClip.ApplyModifiedProperties();

					AnimationClip copyClip = new AnimationClip();
					if (!Resources.Load(animPath))
					{
						EditorUtility.CopySerialized(ogClip, copyClip);
						AssetDatabase.CreateAsset(copyClip, animPath);
						AssetDatabase.Refresh();
					}
					else Debug.Log($"{ogClip.name} already exists at: {animPath}");
				}
			}
		}
	}
}
