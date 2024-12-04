using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class buySkillUI : MonoBehaviour
{
	[Header("Button")]
	[SerializeField]
	Button confirmGacha;
	[SerializeField]
	Button addAmountGachaButton;
	[SerializeField]
	Button removeAmountGachaButton;
	[Header("Image")]
	[SerializeField]
	Image addButtonClickable;
	[SerializeField]
	Image removeButtonClickable;

	[Header("Text")]
	[SerializeField]
	TextMeshProUGUI amountGachaText;
	public TextMeshProUGUI remainText;


	[Header("Other")]
	int _amountGacha;
	int avaliableCoin;
	[SerializeField] private string skillCountPref;
	int AmountGacha
	{
		set
		{
			_amountGacha = value;

			if (avaliableCoin / 300 < _amountGacha)
			{
				_amountGacha--;
				addButtonClickable.gameObject.SetActive(addAmountGachaButton.interactable = false);
			}
			else
			{
				addButtonClickable.gameObject.SetActive(addAmountGachaButton.interactable = true);
			}
			removeButtonClickable.gameObject.SetActive(removeAmountGachaButton.interactable = _amountGacha > 1);
			amountGachaText.text = _amountGacha.ToString();

		}
		get => _amountGacha;
	}
		

	private void Start()
	{
		confirmGacha.onClick.AddListener(ConfirmBuy);
		addAmountGachaButton.onClick.AddListener(AddAmountGacha);
		removeAmountGachaButton.onClick.AddListener(RemoveAmountGacha);
	}

	public void SetUpUI(float coin)
	{
		confirmGacha.interactable = checkCoin();
		avaliableCoin = (int)coin;
		AmountGacha = 1;
		remainText.text = (coin >= 300 ? (avaliableCoin - _amountGacha * 300) : coin).ToString();

		bool checkCoin()
		{
			//hideImage.gameObject.SetActive(coin < 300);
			return coin >= 300;
		}
	}

	void GetVirtualCurrency()
	{
		PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnGetUserInventorySuccess, OnError);
	}

	private void OnError(PlayFabError error)
	{
		throw new NotImplementedException();
	}

	private void OnGetUserInventorySuccess(GetUserInventoryResult result)
	{
		avaliableCoin = result.VirtualCurrency["MC"];
		remainText.text = avaliableCoin.ToString();
		SetUpUI(avaliableCoin);
	}

	private void ConfirmBuy()
	{
		BuyItem(AmountGacha * 300);
		int count = PlayerPrefs.GetInt(skillCountPref, 0);
		PlayerPrefs.SetInt(skillCountPref, count + AmountGacha);
		FindObjectOfType<SkillsSortGameManager>().UpdateAllSkill();
	}
	public void BuyItem(int price)
	{
		var request = new SubtractUserVirtualCurrencyRequest
		{
			VirtualCurrency = "MC",
			Amount = price
		};
		PlayFabClientAPI.SubtractUserVirtualCurrency(request, OnBuySuccessID0, OnError);
		PlayfabMinigame.Instance.GetVirtualCurrencies();
	}
	private void OnBuySuccessID0(ModifyUserVirtualCurrencyResult result)
	{
		GetVirtualCurrency();
	}

	private void OnEnable()
	{
		GetVirtualCurrency();
	}
	void AddAmountGacha()
	{
		AmountGacha++;
		remainText.text = (avaliableCoin - _amountGacha * 300).ToString();
	}
	void RemoveAmountGacha()
	{
		AmountGacha--;
		remainText.text = (avaliableCoin - _amountGacha * 300).ToString();
	}

}
