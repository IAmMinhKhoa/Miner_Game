using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ManagerData")]
public class ManagerDataSO : ScriptableObject
{
    public string managerName;
    public float boostValue = 1;
    public ManagerLevel managerLevel;
    public ManagerLocation managerLocation;
    public BoostType boostType;
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
public enum ManagerSpecie
{
    Tiger,
    Bear,
    Dog,
	Owl,
	Hamster,
	Goat,
	Pig,
	Chicken,
	Monkey
}
