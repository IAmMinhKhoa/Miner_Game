using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorker : MonoBehaviour
{
	public ShaftUI shaftUI;
	public bool isClickable = true;
	private void OnMouseDown()
	{
		if(!isClickable)  return; 
		SoundManager.PlaySound(SoundEnum.mobileClickBack);
		shaftUI.AwakeWorker();
	}
}
