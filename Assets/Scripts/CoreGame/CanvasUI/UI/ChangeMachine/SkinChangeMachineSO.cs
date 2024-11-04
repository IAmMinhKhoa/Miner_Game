using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/SkinEvent")]
public class SkinChangeMachineSO : ScriptableObject
{
	public InventoryItemType type;
	public List<SkinGachaInfor> listSkinGacha;
}
[Serializable]
public struct SkinGachaInfor
{
	public string ID;
	public string Name;
	public string Description;
	public MarketPlayItemQuality quality;
	public Vector3 Positon;
	public Vector3 Scale;
}
