using DG.Tweening;
using NOOD.Sound;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToggleBehaviour : MonoBehaviour
{

	[Header("UI Components")]
	private RectTransform _rectTransform;
	private Vector3 _defaultScale;
	private Vector3 _defaultPos;
	public float bounceScale = 1.2f;
	public float bounceDuration = 0.2f;
	public bool hasManager;

	[Header("Audio")]
	public SoundEnum clickSoundFx = SoundEnum.click;

	void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
		_defaultScale = _rectTransform.localScale;

		if (!hasManager)
		{
			Toggle tg = GetComponent<Toggle>();
			tg.onValueChanged.AddListener(delegate {
				DoAnimate();
			});
		}
	}

	public void DoAnimate()
	{	
		Toggle tg = GetComponent<Toggle>();
		if (tg.isOn) SoundManager.PlaySound(SoundEnum.mobileClickBack);
		tg.graphic.GetComponent<PopupToggle>().OnChoosing(tg.isOn);

	}

	public void DoAnimateToggle(bool isActive)
	{
		SoundManager.PlaySound(SoundEnum.mobileClickBack);
		if (isActive) _rectTransform.DOMoveY(1f, 0.6f).SetEase(Ease.OutQuad);
		else _rectTransform.DOMoveY(_defaultPos.y, 0.6f).SetEase(Ease.OutQuad);
	}


}
