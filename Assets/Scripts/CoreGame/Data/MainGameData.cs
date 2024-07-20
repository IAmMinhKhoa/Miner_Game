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
}
