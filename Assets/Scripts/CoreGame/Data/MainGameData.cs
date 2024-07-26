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
            "UI/Frame lvl big/LV 1-600",
            "UI/Frame lvl big/LV 601-1200",
            "UI/Frame lvl big/LV 1201-1800",
            "UI/Frame lvl big/LV 1801-2400",
        }},
        {ManagerLocation.Elevator, new List<string>
        {
           "UI/Frame lvl big/LV 1-600",
            "UI/Frame lvl big/LV 601-1200",
            "UI/Frame lvl big/LV 1201-1800",
            "UI/Frame lvl big/LV 1801-2400",
        }}
    };

    public readonly static List<string> CanUpgradeButton = new List<string>
    {
        "UI/Icon-CanUpgrade/IconUpgrade-1",
        "UI/Icon-CanUpgrade/IconUpgrade-2",
        "UI/Icon-CanUpgrade/IconUpgrade-3",
    };
}
