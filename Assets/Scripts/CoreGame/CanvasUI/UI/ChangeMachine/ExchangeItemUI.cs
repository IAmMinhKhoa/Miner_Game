using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExchangeItemUI : MonoBehaviour
{
	[Header("Button")]
	[SerializeField]
	Button closeButton;
	[SerializeField]
	Button confirmGacha;
	[SerializeField]
	Button addAmountGachaButton;
	[SerializeField]
	Button removeAmountGachaButton;
	[Header("Image")]
	[SerializeField]
	Image hideImage;
	[SerializeField]
	Image addButtonClickable;
	[SerializeField]
	Image removeButtonClickable;

	[Header("Text")]
	[SerializeField]
	TextMeshProUGUI amountGachaText;
	[SerializeField]
	TextMeshProUGUI cointRemaining;

	public event Action<int> OnGachaButtonClick;
	int _amountGacha;
	int avaliableCoin;
	int AmountGacha
	{
		set {
			_amountGacha = value;
			
			if(avaliableCoin / 300 < _amountGacha)
			{
				_amountGacha--;
				addButtonClickable.gameObject.SetActive(addAmountGachaButton.interactable = false);
			} else
			{
				addButtonClickable.gameObject.SetActive(addAmountGachaButton.interactable = true);
			}
			removeButtonClickable.gameObject.SetActive(removeAmountGachaButton.interactable = _amountGacha > 1);
			amountGachaText.text = _amountGacha.ToString();
			
		}
		get => _amountGacha;
	}
    void Start()
    {
		closeButton.onClick.AddListener(OnCloseButtonClick);
		confirmGacha.onClick.AddListener(GachaItem);
		addAmountGachaButton.onClick.AddListener(AddAmountGacha);
		removeAmountGachaButton.onClick.AddListener(RemoveAmountGacha);
    }

	void OnCloseButtonClick()
	{
		gameObject.gameObject.SetActive(false);
		OnGachaButtonClick = null;
	}
	void GachaItem()
	{
		OnGachaButtonClick?.Invoke(_amountGacha);
		OnCloseButtonClick();
	}
	public void SetUpUI(int  coin)
	{
		confirmGacha.interactable = checkCoin();
		avaliableCoin = coin;
		AmountGacha = 1;
		cointRemaining.text = (coin >= 300 ? (avaliableCoin - _amountGacha * 300) : coin).ToString();

		bool checkCoin() {
			hideImage.gameObject.SetActive(coin < 300);
			return coin >= 300;
		}
	}
	void AddAmountGacha()
	{
		AmountGacha++;
		cointRemaining.text = (avaliableCoin - _amountGacha * 300).ToString();
	}
	void RemoveAmountGacha()
	{
		AmountGacha--;
		cointRemaining.text = (avaliableCoin - _amountGacha * 300).ToString();
	}
}
