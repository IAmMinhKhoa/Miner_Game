using NOOD.Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorker : MonoBehaviour
{
	public ShaftUI shaftUI;
	private void OnMouseDown()
	{
		if (TutorialManager.Instance.isTuroialing) return;
		SoundManager.PlaySound(SoundEnum.mobileClickBack);
		shaftUI.AwakeWorker();
	}
}
