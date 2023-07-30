using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class HandPoseSaver : EditorWindow
{
	[MenuItem("Window/Utils/Serialize Poses")]
	public static void ShowWindow()
	{
		GetWindow<HandPoseSaver>("Hand Pose Saver/Loader");
	}

	private string handPoseName = "HandPose";
	private GameObject selection;
	private bool useRoot;
	
	private void OnGUI()
	{
		handPoseName = GUILayout.TextArea(handPoseName, 50);
		if (GUILayout.Button("Serialize HandPose"))
		{
			if (Selection.gameObjects.Length > 1)
			{
				Debug.LogWarning("Please select only the root !");
				return;
			}

			if (Selection.activeGameObject == null)
			{
				Debug.LogWarning("No object selected ! you dummy >.<");
				return;
			}
			selection = useRoot ? Selection.activeGameObject.transform.root.gameObject : Selection.activeGameObject;
			SerializeTree(handPoseName);
		}
		if (GUILayout.Button("Deserialize HandPose"))
		{
			if (Selection.gameObjects.Length > 1)
			{
				Debug.LogWarning("Please select only the root !");
				return;
			}

			if (Selection.activeGameObject == null)
			{
				Debug.LogWarning("No object selected ! you dummy >.<");
				return;
			}
			selection = useRoot ? Selection.activeGameObject.transform.root.gameObject : Selection.activeGameObject;
			DeserializeTree(handPoseName);
		}
		
		useRoot = GUILayout.Toggle(useRoot, "Root as selection");
		
		string selectionName;
		if (Selection.activeGameObject == null) selectionName = "No selection";
		else if (useRoot) selectionName = Selection.activeGameObject.transform.root.name;
		else selectionName = Selection.activeGameObject.name;
		
		var display = new StringBuilder("Selection: ").Append(selectionName);
		if (useRoot) display.Append(" (ROOT)");
		GUILayout.Label(display.ToString());
	}

	private const string SEPERATOR = ";";
	
	private void SerializeTree(string name)
	{
		string home = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

		try
		{
			StreamWriter sw = new StreamWriter(home + "\\" + name + ".txt", false);

			var allBones = selection.GetComponentsInChildren<Transform>();

			for(int i = 0; i < allBones.Length; i++)
			{
				var currentBone = allBones[i];

				var pos = currentBone.localPosition;
				
				sw.WriteLine($"{pos.x}{SEPERATOR}{pos.y}{SEPERATOR}{pos.z}");

				var rot = currentBone.localRotation;
				
				sw.WriteLine($"{rot.x}{SEPERATOR}{rot.y}{SEPERATOR}{rot.z}{SEPERATOR}{rot.w}");

				var scale = currentBone.localScale;
				
				sw.WriteLine($"{scale.x}{SEPERATOR}{scale.y}{SEPERATOR}{scale.z}");
			}
			sw.Close();
			Debug.Log("Finished !");
		}
		catch (Exception e)
		{
			Debug.LogWarning("Error Occured !");
			Debug.Log(e.Message);
		}
	}
	
	private void DeserializeTree(string name)
	{
		string home = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

		if (!File.Exists(home + "\\" + name + ".txt"))
		{
			Debug.LogWarning($"Pose of name: '{name}' does not exist on your Desktop !");
			return;
		}

		try
		{
			StreamReader sr = new StreamReader(home + "\\" + name + ".txt");

			var allBones = selection.GetComponentsInChildren<Transform>();
			
			for(int i = 0; i < allBones.Length; i++)
			{
				var currentBone = allBones[i];
				
				var pos = new Vector3();
				string[] parts = sr.ReadLine().Split(SEPERATOR[0]);
				pos.x = float.Parse(parts[0]);
				pos.y = float.Parse(parts[1]);
				pos.z = float.Parse(parts[2]);

				currentBone.localPosition = pos;
				
				var rot = new Quaternion();
				string[] partsRot = sr.ReadLine().Split(SEPERATOR[0]);
				rot.x = float.Parse(partsRot[0]);
				rot.y = float.Parse(partsRot[1]);
				rot.z = float.Parse(partsRot[2]);
				rot.w = float.Parse(partsRot[3]);
				
				currentBone.localRotation = rot;
				
				var scale = new Vector3();
				string[] partsScale = sr.ReadLine().Split(SEPERATOR[0]);
				scale.x = float.Parse(partsScale[0]);
				scale.y = float.Parse(partsScale[1]);
				scale.z = float.Parse(partsScale[2]);

				currentBone.localScale = scale;
			}
			sr.Close();
			Debug.Log("Finished !");
		}
		catch (Exception e)
		{
			Debug.LogWarning("Error Occured !");
			Debug.Log(e.Message);
		}
	}
}
