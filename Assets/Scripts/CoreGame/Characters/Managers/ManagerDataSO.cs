using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/ManagerData")]
public class ManagerDataSO : ScriptableObject
{
    public string managerName;
    public float boostTime;
    public float cooldownTime;
    public Sprite icon;
    public ManagerSpecie managerSpecie;
    public ManagerLevel managerLevel;
    public BoostType boostType;
    public ManagerLocation managerLocation;
    public ManagerName managerNameEnum;
}

public enum ManagerLevel
{
    Intern,
    Junior,
    Senior,
    Lead,
    Executive,
}

public enum BoostType
{
    Costs,
    Speed,
    Efficiency,
}

public enum ManagerLocation
{
    Shaft,
    Elevator,
    Counter,
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
}

public enum ManagerSpecie
{
    Elephant,
    Mouse,
}
