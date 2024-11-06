using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ResourceManagement.AsyncOperations;
public enum LanguageKeys
{
	TitleUpgradeShaft,
	TitleUpgradeElevator,
	TitleUpgradeCounter,
	TitleManagerSectionTiger,
	TitleManagerSectionDog,
	TitleManagerSectionBear,
	QuoestCardInfoTiger,
	QuoestCardInfoDog,
	QuoestCardInfoBear,
	BoostCardInFoCost,
	BoostCardInFoSpeed,
	BoostCardInFoEfficiency,
	AuthorPlayList,
	TitleShaft,
	TitleInventoryWallCouter,
	TitleInventoryShaftSecondBg,
	TitleInventoryShaftBg,
	TitleInventoryShaftCart,
	TitleInventoryChangeCartCouter,
	TitleInventoryChangeBackGround,
	TitleInventoryChooseSkinElevator,
	TitleInventoryChangeCartStaff,
	TitleInventoryChangeTableMilkTea,
	TitleInventoryWaitTable,
	TitleInventoryWallElevator,
	TitleInventoryElevator,
	TitleInventoryCart,
	TitleInventoryHead,
	TitleInventoryBody,
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
			{ LanguageKeys.TitleUpgradeShaft, ("home>UpGradeInfo>ShaftTitle", "hầm pha chế cấp") },
			{ LanguageKeys.TitleUpgradeElevator, ("home>UpGradeInfo>ElevatorTitle", "thang máy cấp") },
			{ LanguageKeys.TitleUpgradeCounter, ("home>UpGradeInfo>CounterTitle", "quầy thu ngân cấp") },
			{ LanguageKeys.TitleManagerSectionTiger, ("Tiger", "hổ") },
			{ LanguageKeys.TitleManagerSectionDog, ("Dog", "chó") },
			{ LanguageKeys.TitleManagerSectionBear, ("Bear", "gấu") },
			{ LanguageKeys.QuoestCardInfoTiger, ("home>Cardinfo>Quoest>tiger", "\"cậu chỉ sống một lần, nhưng nếu sống đúng cách, một lần là đủ\"") },
			{ LanguageKeys.QuoestCardInfoDog, ("home>Cardinfo>Quoest>dog", "\"một căn phòng không có sách giống như một cơ thể không có linh hồn\"") },
			{ LanguageKeys.QuoestCardInfoBear, ("home>Cardinfo>Quoest>bear", "\"hãy là chính mình, vì tất cả những người khác đã được chọn\"") },
			{ LanguageKeys.BoostCardInFoCost, ("home>Cardinfo>boost>cost", "chi phí") },
			{ LanguageKeys.BoostCardInFoSpeed, ("home>Cardinfo>boost>speed", "tốc độ di chuyển") },
			{ LanguageKeys.BoostCardInFoEfficiency, ("home>Cardinfo>boost>Efficiency", "tốc độ đỡ hàng") },
			{ LanguageKeys.AuthorPlayList, ("author", "tác giả") },
			{ LanguageKeys.TitleShaft, ("titleShaft", "tầng") },
			{ LanguageKeys.TitleInventoryWallCouter, ("home>inventory>wallcouter", "tường thu ngân") },
			{ LanguageKeys.TitleInventoryShaftSecondBg, ("home>inventory>ShaftSecondBg", "tường 2") },
			{ LanguageKeys.TitleInventoryShaftBg, ("home>inventory>ShaftBg", "tường") },
			{ LanguageKeys.TitleInventoryChangeCartCouter, ("home>inventory>ChangeCartCouter", "đổi xe đẩy ở quầy") },
			{ LanguageKeys.TitleInventoryChangeBackGround, ("home>inventory>changebackground", "đổi background phòng chờ trà sữa") },
			{ LanguageKeys.TitleInventoryChooseSkinElevator, ("home>inventory>chooseSkinElevator", "chọn skin thang máy") },
			{ LanguageKeys.TitleInventoryChangeCartStaff, ("home>inventory>changeCartStaff", "đổi xe đẩy nhân viên") },
			{ LanguageKeys.TitleInventoryChangeTableMilkTea, ("home>inventory>changeTableMilkTea", "đổi bàn để ly trà sữa") },
			{ LanguageKeys.TitleInventoryWaitTable, ("home>Inventory>waittable", "bàn chờ") },
			{ LanguageKeys.TitleInventoryWallElevator, ("home>inventory>WallElevator", "tường thang máy") },
			{ LanguageKeys.TitleInventoryElevator, ("home>inventory>Elevator", "thang máy") },
			{ LanguageKeys.TitleInventoryCart, ("home>Inventory>cart", "xe đẩy") },
			{ LanguageKeys.TitleInventoryHead, ("home>inventory>Head", "đầu") },
			{ LanguageKeys.TitleInventoryBody, ("home>inventory>Body", "thân hình") },
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
