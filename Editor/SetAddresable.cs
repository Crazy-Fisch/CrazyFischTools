#if ADDRESSABLES

using UnityEditor;
using UnityEngine;

namespace CrazyFischGames.Tools
{
	public class SetAddresable : EditorWindow
	{
	
		[MenuItem("Window/Utils/Set Addressable")]
		public static void ShowWindow()
		{
			GetWindow<SetAddresable>("Set Addressable from selection");
		}

		private const char SEPERATOR = ';';

		private string _address;
		private bool _addressIsName;
		private string _label;
		private string _groupName;
		private bool _forceNewGroup;

		private void OnGUI()
		{
			GUILayout.Label("Address (Can be empty)");
			_address = GUILayout.TextField(_address);
			
			GUILayout.Label("Address is name");
			_addressIsName = EditorGUILayout.Toggle(_addressIsName);
			
			GUILayout.Label($"Label (Can be empty, split multiple with '{SEPERATOR}')");
			_label = GUILayout.TextField(_label);
			GUILayout.Label("Group Name (Can be empty)");
			_groupName = GUILayout.TextField(_groupName);

			GUILayout.Label("Force new group if it doesnt exits ?");
			_forceNewGroup = EditorGUILayout.Toggle(_forceNewGroup);
			
			if (GUILayout.Button("Set Addressable"))
			{
				var selections = Selection.gameObjects;
				if (selections.Length != 1 && !_addressIsName)
				{
					Debug.Log("Please select exactly one object or choose 'Address is name'");
					return;
				}
				
				foreach(var selection in selections)
				{
					var reference = AddressableEditorUtils.AddAssetToAddressables(selection);
					var entry = AddressableEditorUtils.GetAddressableAssetEntry(reference.AssetGUID);
					var address = _addressIsName ? selection.name : _address;
					if (_label.Contains(SEPERATOR))
					{
						var labels = _label.Split(SEPERATOR);
						AddressableEditorUtils.SetAddressableEntryInfo(entry, address, _groupName, _forceNewGroup, labels);
					}
					else AddressableEditorUtils.SetAddressableEntryInfo(entry, address, _label, _groupName, _forceNewGroup);
				}

				AssetDatabase.SaveAssets();
				EditorUtility.FocusProjectWindow();
			}
		}
	}
}

#endif