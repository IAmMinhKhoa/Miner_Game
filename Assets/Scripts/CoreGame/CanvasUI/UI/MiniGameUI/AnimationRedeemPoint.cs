using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class AnimationRedeemPoint : MonoBehaviour
{
	private float moveDistance = 0.1f;
	private float moveDuration = 0.5f;

	void Start()
	{
		Vector3 startPosition = transform.position;
		Vector3 upPosition = startPosition + Vector3.up * moveDistance;
		Vector3 downPosition = startPosition;
		transform.DOMove(upPosition, moveDuration)
			.SetEase(Ease.InOutSine) 
			.OnComplete(() =>
			{
				transform.DOMove(downPosition, moveDuration)
					.SetEase(Ease.InOutSine)
					.SetLoops(-1, LoopType.Yoyo); 
			});
	}
}
