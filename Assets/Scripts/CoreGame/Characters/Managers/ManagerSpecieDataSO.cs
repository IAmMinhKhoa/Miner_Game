using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/ManagerSpecieData")]
public class ManagerSpecieDataSO : ScriptableObject
{
    public ManagerSpecie managerSpecie;
    public Sprite icon;
    public string viewPath;
}
