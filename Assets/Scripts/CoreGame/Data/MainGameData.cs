using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MainGameData
{
    public static List<ManagerDataSO> managerDataSOList = new List<ManagerDataSO>();
    public static List<ManagerSpecieDataSO> managerSpecieDataSOList = new List<ManagerSpecieDataSO>();
    public static List<ManagerTimeDataSO> managerTimeDataSOList = new List<ManagerTimeDataSO>();
    public static bool isDone = false;

    public readonly static Dictionary<BoostType, List<string>> BoostButtonUIs = new Dictionary<BoostType, List<string>>
    {
        {BoostType.Costs, new List<string>
        {
            "UI/Icon Buff/Cost_1",
            "UI/Icon Buff/Cost_2",
            "UI/Icon Buff/Cost_3"
        }},
        {BoostType.Efficiency, new List<string>
        {
            "UI/Icon Buff/Efficiency_1",
            "UI/Icon Buff/Efficiency_2",
            "UI/Icon Buff/Efficiency_3"
        }},
        {BoostType.Speed, new List<string>
        {
            "UI/Icon Buff/Speed_1",
            "UI/Icon Buff/Speed_2",
            "UI/Icon Buff/Speed_3"
        }}
    };
    public readonly static Dictionary<ManagerLocation, List<string>> FrameLevelButton = new Dictionary<ManagerLocation, List<string>>
    {
        {ManagerLocation.Shaft, new List<string>
        {
            "UI/Frame lvl small/LV 1-200",
            "UI/Frame lvl small/LV 201-400",
            "UI/Frame lvl small/LV 401-600",
            "UI/Frame lvl small/LV 601-800"

        }},
        {ManagerLocation.Counter, new List<string>
        {
			"UI/Frame lvl small/LV 1-200",
			"UI/Frame lvl small/LV 201-400",
			"UI/Frame lvl small/LV 401-600",
			"UI/Frame lvl small/LV 601-800"
		}},
        {ManagerLocation.Elevator, new List<string>
        {
		   "UI/Frame lvl small/LV 1-200",
			"UI/Frame lvl small/LV 201-400",
			"UI/Frame lvl small/LV 401-600",
			"UI/Frame lvl small/LV 601-800"
		}}
    };

    public readonly static List<string> IconLevelNumber = new List<string>
    {
		"UI/Icon level/Level-1 1",
        "UI/Icon level/Level-2 1",
        "UI/Icon level/Level-3 1",
        "UI/Icon level/Level-4 1",
        "UI/Icon level/Level-5 1"
    };
    public readonly static List<string> FrameLevelAvatar = new List<string>
    {
        "UI/Frame Avatar/Frame-lvl-1 1",
        "UI/Frame Avatar/Frame-lvl-2 1",
        "UI/Frame Avatar/Frame-lvl-3 1",
        "UI/Frame Avatar/Frame-lvl-4 1",
        "UI/Frame Avatar/Frame-lvl-5 1"


    };
    public readonly static List<string> CanUpgradeButton = new List<string>
    {
        "UI/Icon-CanUpgrade/IconUpgrade-1",
        "UI/Icon-CanUpgrade/IconUpgrade-2",
        "UI/Icon-CanUpgrade/IconUpgrade-3",
    };
	public readonly static List<string> BannerLevels = new List<string>
	{
		"UI/Frame Information Manager/L-1 1",
		"UI/Frame Information Manager/L-2 1",
		"UI/Frame Information Manager/L-3 1",
		"UI/Frame Information Manager/L-4 1",
		"UI/Frame Information Manager/L-5 1",


	};
	public readonly static List<string> PanelFrontCardManager= new List<string>
	{
		"UI/Frame Information Manager/PI-1 1",
		"UI/Frame Information Manager/PI-2 1",
		"UI/Frame Information Manager/PI-3 1",
		"UI/Frame Information Manager/PI-4 1",
		"UI/Frame Information Manager/PI-5 1",
	};
	public readonly static List<string> PanelBehindCardManager = new List<string>
	{
		"UI/Frame Information Manager/BH-1 1",
		"UI/Frame Information Manager/BH-2 1",
		"UI/Frame Information Manager/BH-3 1"

	};

	public readonly static Dictionary<ManagerLocation, List<string>> UpgradeDetailInfo = new Dictionary<ManagerLocation, List<string>>
    {
        {ManagerLocation.Shaft, new List<string>
        {
            "hầm pha chế cấp ",
            "tốc độ pha chế",
            "nhân viên \nvận chuyển",
            "tổng sản lượng",
        }},
        {ManagerLocation.Elevator, new List<string>
        {
            "thang máy cấp ",
            "tốc độ thu gom",
            "tốc độ thang máy",
            "tổng khối lượng \nvận chuyển",
        }},
        {ManagerLocation.Counter, new List<string>
        {
            "quầy bán hàng cấp ",
            "tốc độ thu gom",
            "nhân viên \nvận chuyển",
            "tổng khối lượng \nvận chuyển",
        }}
    };
}
