using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : Patterns.Singleton<SkinManager>
{
	public static SkinDataSO SkinDataSO = null;
	public void FindSkinData()
	{
		SkinDataSO[] soundDataSOs = Resources.LoadAll<SkinDataSO>("");
		if (soundDataSOs.Length > 0)
			SkinDataSO = Resources.FindObjectsOfTypeAll<SkinDataSO>()[0];
		if (SkinDataSO == null)
			Debug.LogError("Can't find SkinDataSO, please create one in Resources folder using Create -> SkinDataSO");
		else
			Debug.Log("Load SkinDataSO success");
	}


	public Sprite GetBrShaft(SkinShaftBg enumBr)
	{
		if (SkinDataSO.skinBgShaft.ContainsKey(enumBr))
		{
			DataSkinImage dataSkinImage = SkinDataSO.skinBgShaft.Dictionary[enumBr];
				
			return dataSkinImage.img;
		}
		else
		{
			Debug.LogError($"Key {enumBr} not found in skinBgShaft dictionary.");
			return null; // Or return a default Sprite
		}
	}



}
