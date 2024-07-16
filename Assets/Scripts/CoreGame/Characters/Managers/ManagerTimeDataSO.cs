using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/ManagerTimeData")]
public class ManagerTimeDataSO : ScriptableObject
{
    public float boostTime;
    public float cooldownTime;
    public ManagerLevel managerLevel;
}
