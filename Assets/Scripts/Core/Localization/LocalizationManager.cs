using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
public enum LanguageKeys
{
	TitleUpgradeCounter,

}

public static class LocalizationManager
{
	private static Dictionary<LanguageKeys, (string entryKey, string defaultValue)> keyMappings;

	private static Dictionary<LanguageKeys, (string entryKey, string defaultValue)> KeyMappings
	{
		get
		{
			if (keyMappings == null)
			{
				keyMappings = InitializeKeyMappings();
			}
			return keyMappings;
		}
	}

	private static Dictionary<LanguageKeys, (string entryKey, string defaultValue)> InitializeKeyMappings()
	{
		var mappings = new Dictionary<LanguageKeys, (string, string)>
		{
			//add more key - value
			{ LanguageKeys.TitleUpgradeCounter, ("home>UpGradeInfo>Title", "") },

		};
		return mappings;
	}

	public static string GetLocalizedString(LanguageKeys key, string tableReference = "Language", object[] parameters = null)
	{
		if (!KeyMappings.TryGetValue(key, out var mapping)) return string.Empty;

		var stringRef = new LocalizedString() { TableReference = tableReference, TableEntryReference = mapping.entryKey };
		if (parameters != null && parameters.Length > 0)
		{
			stringRef.Arguments = parameters;
			stringRef.RefreshString();
		}
		var stringOperation = stringRef.GetLocalizedStringAsync();
		return stringOperation is { IsDone: true, Status: AsyncOperationStatus.Succeeded } ? stringOperation.Result.Replace("\\n", "\n") : mapping.defaultValue;
	}
}
