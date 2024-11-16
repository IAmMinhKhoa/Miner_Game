using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/SkinEvent")]
public class SkinChangeMachineSO : ScriptableObject
{
	public InventoryItemType type;
	public bool isTheSameValue;
	public List<SkinGachaInfor> listSkinGacha;
	
	private void OnValidate()
	{
		for (int i = 0; i < listSkinGacha.Count; i++)
		{
			listSkinGacha[i].ValidateID();
			if (isTheSameValue && i > 0)
			{
				listSkinGacha[i].Positon = listSkinGacha[i - 1].Positon;
				listSkinGacha[i].Scale = listSkinGacha[i - 1].Scale;
				listSkinGacha[i].PositonSingle = listSkinGacha[i - 1].PositonSingle;
				listSkinGacha[i].ScaleSingle = listSkinGacha[i - 1].ScaleSingle;
			}
		}
	}
}
[Serializable]
public class SkinGachaInfor
{
	[Tooltip("Unique identifier for this skin item. This field is required.")]
	public string ID;
	public string Name;
	[TextArea]
	public string Description;
	public MarketPlayItemQuality quality;
	public Vector3 Positon;
	public Vector3 Scale;
	public Vector3 PositonSingle;
	public Vector3 ScaleSingle;
	public void ValidateID()
	{
		if (string.IsNullOrEmpty(ID))
		{
			Debug.LogWarning("ID is required and cannot be empty.");
		}
	}
}
