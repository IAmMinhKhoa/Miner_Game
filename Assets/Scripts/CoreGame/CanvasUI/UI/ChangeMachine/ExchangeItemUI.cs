using DG.Tweening;
using Sirenix.OdinInspector;
using Spine.Unity;
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
	[SerializeField]
	TextMeshProUGUI itemGacha;

	[Header("Spine")]
	[SerializeField]
	SkeletonGraphic interiorSpine;
	[SerializeField]
	SkeletonGraphic staffSpine;

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

	private void OnEnable()
	{
		FadeInContainer();
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
		FadeOutContainer();
		gameObject.gameObject.SetActive(false);
		OnGachaButtonClick = null;
	}
	void GachaItem()
	{
		OnGachaButtonClick?.Invoke(_amountGacha);
		OnCloseButtonClick();
	}
	public void SetUpUI(float coin, bool isInterior)
	{
		
		confirmGacha.interactable = checkCoin();
		avaliableCoin = (int)coin;
		AmountGacha = 1;
		cointRemaining.text = (coin >= 300 ? (avaliableCoin - _amountGacha * 300) : coin).ToString();
		interiorSpine.gameObject.SetActive(isInterior);
		staffSpine.gameObject.SetActive(!isInterior);
		itemGacha.text = isInterior ? "Nội thất ngẫu nhiên" : "Trang phục nhân viên ngẫu nhiên";

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

	#region AnimateUI
	[Button]
	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
		gameObject.transform.DOLocalMoveX(0, 0.6f).SetEase(Ease.OutQuart);

	}
	[Button]
	public void FadeOutContainer()
	{
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
		{
			gameObject.SetActive(false);
		});

	}
	#endregion
}
