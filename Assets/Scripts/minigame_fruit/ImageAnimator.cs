using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ImageAnimator : MonoBehaviour
{
	public float duration=0.9f;
	public RectTransform rectTransform;
	private void Start()
	{
		ComboUI.Instance.OnImageAnimation.AddListener(OnImageAnimation);
	}

	private void OnImageAnimation()
	{
		rectTransform.localScale = Vector3.zero;
		rectTransform.DOScale(Vector3.one, duration).SetEase(Ease.OutBounce);
	}
}
