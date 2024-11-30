using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorker : MonoBehaviour
{
	public ShaftUI shaftUI;
	private void OnMouseDown()
	{
		SoundManager.PlaySound(SoundEnum.mobileClickBack);
		shaftUI.AwakeWorker();
	}
}
