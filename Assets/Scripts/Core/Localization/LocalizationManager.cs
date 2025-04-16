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

	TitleManagerSectionOwl,
	TitleManagerSectionHamster,
	TitleManagerSectionGoat,

	QuoestCardInfoTiger,
	QuoestCardInfoDog,
	QuoestCardInfoBear,
	QuoestCardInfoOwl,
	QuoestCardInfoHamster,
	QuoestCardInfoGoat,


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
	TitleInventoryChangeCashierCouter,
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
	TitleMarketCartStaff,
	TitleMarketElevatorStaff,
	TitleMarketMarketGoods,
	TitleMarketImportedGoods,
	TitleMarketCrafts,
	TitleManagerChooseUIHire,
	TitleManagerChooseUIRest,
	TitleBodyFontCute,
	TitleBodyTemplateNumber,
	TitleExchangeUIRandomInterior,
	TitleExchangeUIRandomSkin,
	TitleInventoryChangeBarCounter,

	Hour,
	Minutes,
	Seconds,

	TutorialTitle1,
	TutorialTitle2,
	TutorialTitle3,
	TutorialTitle4,
	TutorialTitle5,
	TutorialTitle6,

	thoigianphache,
	tongsanluong,
	totalShippingVolume,
	transportationStaff,
	collectionSpeed,
	elevatorSpeed,

	upgradeCakeValue,
	upgradeCakeTime,
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
			{ LanguageKeys.TitleManagerSectionOwl, ("owl", "cú") },
			{ LanguageKeys.TitleManagerSectionGoat, ("goat", "dê") },
			{ LanguageKeys.TitleManagerSectionHamster, ("hamster", "hamster") },


			{ LanguageKeys.QuoestCardInfoTiger, ("home>Cardinfo>Quoest>tiger", "\"cậu chỉ sống một lần, nhưng nếu sống đúng cách, một lần là đủ\"") },
			{ LanguageKeys.QuoestCardInfoDog, ("home>Cardinfo>Quoest>dog", "\"một căn phòng không có sách giống như một cơ thể không có linh hồn\"") },
			{ LanguageKeys.QuoestCardInfoBear, ("home>Cardinfo>Quoest>bear", "\"hãy là chính mình, vì tất cả những người khác đã được chọn\"") },
			{ LanguageKeys.QuoestCardInfoOwl, ("home>cardinfo>quoest>owl", "\"trà bong bóng động vật\"") },
			{ LanguageKeys.QuoestCardInfoHamster, ("home>cardinfo>quoest>hamster", "\"trà bong bóng động vật\"") },
			{ LanguageKeys.QuoestCardInfoGoat, ("home>cardinfo>quoest>goat", "\"trà bong bóng động vật\"") },


			{ LanguageKeys.BoostCardInFoCost, ("home>Cardinfo>boost>cost", "chi phí") },
			{ LanguageKeys.BoostCardInFoSpeed, ("home>Cardinfo>boost>speed", "tốc độ di chuyển") },
			{ LanguageKeys.BoostCardInFoEfficiency, ("home>Cardinfo>boost>Efficiency", "tốc độ đỡ hàng") },
			{ LanguageKeys.AuthorPlayList, ("author", "tác giả") },
			{ LanguageKeys.TitleShaft, ("titleShaft", "tầng") },
			{ LanguageKeys.TitleInventoryWallCouter, ("home>inventory>wallcouter", "tường thu ngân") },
			{ LanguageKeys.TitleInventoryShaftSecondBg, ("home>inventory>ShaftSecondBg", "tường 2") },
			{ LanguageKeys.TitleInventoryShaftBg, ("home>inventory>ShaftBg", "tường") },
			{ LanguageKeys.TitleInventoryChangeCartCouter, ("home>inventory>ChangeCartCouter", "Đổi quầy thu tiền") },
			{ LanguageKeys.TitleInventoryChangeCashierCouter, ("home>inventory>ChangeCartCouter", "Đổi quầy thu tiền") },
			{ LanguageKeys.TitleInventoryChangeBackGround, ("home>inventory>changebackground", "đổi background phòng chờ trà sữa") },
			{ LanguageKeys.TitleInventoryChangeBarCounter, ("home>inventory>changebackground", "đổi quầy pha chế") },
			{ LanguageKeys.TitleInventoryChooseSkinElevator, ("home>inventory>chooseSkinElevator", "chọn skin thang máy") },
			{ LanguageKeys.TitleInventoryChangeCartStaff, ("home>inventory>changeCartStaff", "đổi xe đẩy nhân viên") },
			{ LanguageKeys.TitleInventoryChangeTableMilkTea, ("home>inventory>changeTableMilkTea", "đổi bàn để ly trà sữa") },
			{ LanguageKeys.TitleInventoryWaitTable, ("home>Inventory>waittable", "bàn chờ") },
			{ LanguageKeys.TitleInventoryWallElevator, ("home>inventory>WallElevator", "tường thang máy") },
			{ LanguageKeys.TitleInventoryElevator, ("home>inventory>Elevator", "thang máy") },
			{ LanguageKeys.TitleInventoryCart, ("home>Inventory>cart", "xe đẩy") },
			{ LanguageKeys.TitleInventoryHead, ("home>inventory>Head", "đầu") },
			{ LanguageKeys.TitleInventoryBody, ("home>inventory>Body", "thân hình") },
			{ LanguageKeys.TitleMarketCartStaff, ("home>market>cartstaff", "nhân viên xe đẩy") },
			{ LanguageKeys.TitleMarketElevatorStaff, ("home>market>elevatorstaff", "nhân viên thang máy") },
			{ LanguageKeys.TitleMarketMarketGoods, ("home>marketUI>marketgoods", "hàng chợ") },
			{ LanguageKeys.TitleMarketImportedGoods, ("home>marketUI>importedgoods", "hàng nhập khẩu") },
			{ LanguageKeys.TitleMarketCrafts, ("home>marketUI>crafts", "hàng thủ công") },
			{ LanguageKeys.TitleManagerChooseUIHire, ("home>managerchooseUI>hire", "thuê") },
			{ LanguageKeys.TitleManagerChooseUIRest, ("home>managerchooseUI>rest", "nghỉ") },
			{ LanguageKeys.TitleBodyFontCute, ("home>banner>bodyFontCute", "phông chữ dễ thương") },
			{ LanguageKeys.TitleBodyTemplateNumber, ("home>banner>bodyTemplateNumber", "mẫu số") },
			{ LanguageKeys.TitleExchangeUIRandomInterior, ("home>exchangeItem>RandomInterior", "nội thất ngẫu nhiên") },
			{ LanguageKeys.TitleExchangeUIRandomSkin, ("home>exchangeItem>RandomSkin", "trang phục ngẫu nhiên") },

			{ LanguageKeys.Hour, ("common>hours", "giờ") },
			{ LanguageKeys.Minutes, ("common>minutes", "phút") },
			{ LanguageKeys.Seconds, ("common>second", "giây") },


			{ LanguageKeys.TutorialTitle1, ("tutorial>title-1", "Chuyển trà sữa từ tầng xuống quầy để bán đi") },
			{ LanguageKeys.TutorialTitle2, ("tutorial>title-2", "Cho tiền nè thuê nhân viên đi") },
			{ LanguageKeys.TutorialTitle3, ("tutorial>title-3", "Mỗi quản lý sẽ có sức mạnh riêng, càng nhiều sao càng mạnh") },
			{ LanguageKeys.TutorialTitle4, ("tutorial>title-4", "Thoát ra ngoài xem thành quả đi bro") },
			{ LanguageKeys.TutorialTitle5, ("tutorial>title-5", "Thuê quản lý cho cả thang máy và quầy") },
			{ LanguageKeys.TutorialTitle6, ("tutorial>title-6", "Bạn có thể nâng cấp toàn các bộ phận hoặc mở thêm tầng để tăng thu nhập") },

			{ LanguageKeys.thoigianphache, ("information>speed-tea", "tốc độ pha chế") },
			{ LanguageKeys.tongsanluong, ("information>total-product", "tổng sản lượng") },
			{ LanguageKeys.totalShippingVolume, ("home>UpGradeInfo>totalShippingVolume", "tổng khối lượng vận chuyển") },
			{ LanguageKeys.transportationStaff, ("home>UpGradeInfo>transportationStaff", "nhân viên vận chuyển") },
			{ LanguageKeys.collectionSpeed, ("home>UpGradeInfo>collectionSpeed", "tốc độ thu gom") },
			{ LanguageKeys.elevatorSpeed, ("information>elevatorSpeed", "tốc độ thang máy") },

			{ LanguageKeys.upgradeCakeTime, ("upgrade>detail>cake-time", "thời gian sản xuất") },
			{ LanguageKeys.upgradeCakeValue, ("upgrade>detail>cake-value", "giá trị mỗi bánh") },
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
