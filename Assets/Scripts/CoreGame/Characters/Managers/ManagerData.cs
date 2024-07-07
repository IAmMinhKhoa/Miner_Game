using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerData
{
    public string managerName;
    public ManagerLevel managerLevel;
    public BoostType boostType;
    public ManagerLocation managerLocation;
    public ManagerName managerNameEnum;

    public static ManagerData CreateManagerData(ManagerLocation location)
    {
        ManagerData managerData = new ManagerData();
        managerData.managerLevel = GetRandomManagerLevel();
        managerData.managerNameEnum = GetRandomManagerName();
        managerData.boostType = GetRandomBoostType();
        managerData.managerLocation = location;

        managerData.managerName = managerData.managerLevel.ToString() + " " + managerData.managerNameEnum.ToString();
        return managerData;
    }

    private static ManagerLevel GetRandomManagerLevel()
    {
        // Define ratios for each level
        float juniorRatio = 0.65f;
        float seniorRatio = 0.25f;

        float randomValue = UnityEngine.Random.value; // Generates a random number between 0.0 and 1.0

        if (randomValue < juniorRatio)
        {
            return ManagerLevel.Junior;
        }
        else if (randomValue < juniorRatio + seniorRatio)
        {
            return ManagerLevel.Senior;
        }
        else
        {
            return ManagerLevel.Excutive;
        }
    }

    private static ManagerName GetRandomManagerName()
    {
        int count = (int)ManagerName.Count;
        int randomValue = UnityEngine.Random.Range(0, count);
        return (ManagerName)randomValue;
    }

    private static BoostType GetRandomBoostType()
    {
        int count = (int)BoostType.Count;
        int randomValue = UnityEngine.Random.Range(0, count);
        return (BoostType)randomValue;
    }
}

public enum ManagerLevel
{
    Intern,
    Junior,
    Senior,
    Lead,
    Excutive,
}

public enum BoostType
{
    Costs,
    Speed,
    Efficiency,
    Count,
}

public enum ManagerLocation
{
    Shaft,
    Elevator,
    Couter,
}

public enum ManagerName
{
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Count,
}
