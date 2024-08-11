using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/ManagerSpecieData")]
public class ManagerSpecieDataSO : ScriptableObject
{
    public ManagerSpecie managerSpecie;
    public Sprite icon; //icon default
    public Sprite icon_Special; //icon special to level 5
    public string viewPath;
}
