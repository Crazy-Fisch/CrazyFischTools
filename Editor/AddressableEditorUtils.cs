#if ADDRESSABLES

using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CrazyFischGames.Tools
{
	public static class AddressableEditorUtils
	{
		public static AssetReference AddAssetToAddressables(Object asset) => AddAssetToAddressables(AddressableAssetSettingsDefaultObject.Settings, asset);
		public static AssetReference AddAssetToAddressables(AddressableAssetSettings settings, Object asset)
		{
			string assetPath = AssetDatabase.GetAssetPath(asset);
			string assetGUID = AssetDatabase.AssetPathToGUID(assetPath);
			return settings.CreateAssetReference(assetGUID);
		}
		
		public static AssetReference AddAssetToAddressables(string guid) => AddAssetToAddressables(AddressableAssetSettingsDefaultObject.Settings, guid);
		public static AssetReference AddAssetToAddressables(AddressableAssetSettings settings, string guid) => settings.CreateAssetReference(guid);
		
		public static AddressableAssetEntry GetAddressableAssetEntry(Object o) => GetAddressableAssetEntry(AddressableAssetSettingsDefaultObject.Settings, o);
		public static AddressableAssetEntry GetAddressableAssetEntry(AddressableAssetSettings settings, Object o)
		{
			AddressableAssetEntry entry = null;

			string guid = "";
			long localID = 0;

			bool found = AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o, out guid, out localID);
			string path = AssetDatabase.GUIDToAssetPath(guid);

			if (found && (path.ToLower().Contains("assets")))
				entry = settings.FindAssetEntry(guid);

			return entry;
		}

		public static AddressableAssetEntry GetAddressableAssetEntry(string guid) => GetAddressableAssetEntry(AddressableAssetSettingsDefaultObject.Settings, guid);
		public static AddressableAssetEntry GetAddressableAssetEntry(AddressableAssetSettings settings, string guid) => settings.FindAssetEntry(guid);

		public static void SetAddressableEntryInfo(AddressableAssetEntry entry, [CanBeNull] string address,
			[CanBeNull] string label, [CanBeNull] string groupName, bool forceNewGroup = false) {
			SetAddressableEntryInfo(AddressableAssetSettingsDefaultObject.Settings, entry, address, label, groupName, forceNewGroup);
		}
		
		public static void SetAddressableEntryInfo(AddressableAssetSettings settings, AddressableAssetEntry entry, [CanBeNull] string address,
			[CanBeNull] string label, [CanBeNull] string groupName, bool forceNewGroup = false)
		{
			if(!string.IsNullOrEmpty(address)) entry.address = address;
			if (!string.IsNullOrEmpty(label))
			{
				if(!settings.GetLabels().Contains(label)) settings.AddLabel(label);
				entry.SetLabel(label, true);
			}
			if (!string.IsNullOrEmpty(groupName))
			{
				var group = settings.FindGroup(groupName);
				if (group == null)
				{
					if (forceNewGroup)
						group = settings.CreateGroup(groupName, false, false, true, null);
					if(group == null) return;
				}
				settings.MoveEntry(entry, group);
			}
		}
		
		public static void SetAddressableEntryInfo(AddressableAssetEntry entry, [CanBeNull] string address,
			[CanBeNull] string groupName, bool forceNewGroup, params string[] labels) {
			SetAddressableEntryInfo(AddressableAssetSettingsDefaultObject.Settings, entry, address, groupName, forceNewGroup, labels);
		}
		
		public static void SetAddressableEntryInfo(AddressableAssetSettings settings, AddressableAssetEntry entry, [CanBeNull] string address,
			[CanBeNull] string groupName, bool forceNewGroup, params string[] labels) {
			if(!string.IsNullOrEmpty(address)) entry.address = address;
			if (labels.Length > 0)
			{
				var settingLabels = settings.GetLabels();
				foreach (var label in labels)
				{
					if(!settingLabels.Contains(label)) settings.AddLabel(label);
					entry.SetLabel(label, true);
				}
			}
			if (!string.IsNullOrEmpty(groupName))
			{
				var group = settings.FindGroup(groupName);
				if (group == null)
				{
					if (forceNewGroup)
						group = settings.CreateGroup(groupName, false, false, true, null);
					if(group == null) return;
				}
				settings.MoveEntry(entry, group);
			}
		}

		public static void SetAddressableEntryLabels(AddressableAssetEntry entry, params string[] labels)
		{
			if(entry == null || labels.Length == 0) return;
			var settings = AddressableAssetSettingsDefaultObject.Settings;
			
			var settingLabels = settings.GetLabels();
			foreach (var label in labels)
			{
				if(!settingLabels.Contains(label)) settings.AddLabel(label);
				entry.SetLabel(label, true);
			}
		}
	}
}

#endif