using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObjects/ManagerSpecieData")]
public class ManagerSpecieDataSO : ScriptableObject
{
    public ManagerSpecie managerSpecie;
	public BoostType BoostType;
	public ManagerLocation LocationType;
	public string contentQuoest;
    public Sprite icon; //icon default
    public Sprite icon_Special; //icon special to level 5
    public SkeletonDataAsset spineManager;
    public string viewPath;
}
