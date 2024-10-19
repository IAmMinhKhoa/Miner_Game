using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotEnoughMoneyNotifi : MonoBehaviour
{
	[SerializeField]
	BankUI BankUI;
	[SerializeField]
	ButtonBehavior addMoney;
	[SerializeField]
	SkeletonGraphic spine;
	private void Start()
	{
		addMoney.onClickEvent.AddListener(OpenBankUI);
	}
	private void OpenBankUI()
	{
		CloseUI();
		BankUI.FadeInContainer();
	}
	public void CloseUI()
	{
		gameObject.SetActive(false);
	}
}
