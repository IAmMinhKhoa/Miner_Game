using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DetailNotification : MonoBehaviour
{
	[SerializeField] Transform container;
	[SerializeField] TMP_Text textNoti;

	public void OpenModal(string content)
	{
		this.gameObject.SetActive(true);
		textNoti.text = content;
		container.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).OnComplete(() =>
		{
		
		});
	}
	public void CloseModal()
	{
		container.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).OnComplete(() =>
		{
			this.gameObject.SetActive(false);
		});
	}
}
