using System;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using NOOD.SerializableDictionary;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PawnItem
{
    public float Amount;
    public float ScaleFactor;
}

public enum IdBundle
{
	currency_sp_1,
	currency_sp_2,
	currency_sp_3,
	currency_sp_4,
	currency_sp_5,
	currency_sp_6
}

public class BankUI : MonoBehaviour
{
	private Vector3 scale_tablet = new Vector3(1.05f, 1.05f, 1.05f);
	[SerializeField] private RectTransform _content;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private List<Button> _closeButtons;
    [SerializeField] private float _fadeSpeed = 0.3f;
    private bool _isShowRequest;
    [SerializeField] private SerializableDictionary<PawButton, PawnItem> _pawStoreButton = new SerializableDictionary<PawButton, PawnItem>();


    void Start()
    {
		if (Common.IsTablet)
		{
			gameObject.transform.localScale = scale_tablet;
		}
		_closeButtons.ForEach(x => x.onClick.AddListener(FadeOutContainer));
        if (_isShowRequest == false) // Prevent Hide when press shop button
            FadeOutContainer();
    }

    void OnEnable()
    {
        UpdateUI();
    }


    /*public void BuyMoney(float amount)
    {
        SuperMoneyManager.Instance.AddMoney(amount);
    }*/
    public void BuyPaw(double amount, float price)
    {
        if (SuperMoneyManager.Instance.SuperMoney >= price)
        {
            SuperMoneyManager.Instance.RemoveMoney(price);
            PawManager.Instance.AddPaw(amount);
        }
        else
        {
            // Announce player don't have enough money
            Debug.Log("Don't have enough money");
        }
    }

    private void UpdateUI()
    {
        foreach (var pair in _pawStoreButton.Dictionary)
        {
            // Update paw button
            pair.Key.SetData((int)pair.Value.Amount, (int)pair.Value.ScaleFactor, this);
        }
        CultureInfo userRegion = CultureInfo.CurrentCulture;

    }

    #region AnimateUI
    [Button]
    public void FadeInContainer()
    {
        gameObject.SetActive(true);
        Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
        gameObject.transform.localPosition = new Vector2(posCam.x - 2000, posCam.y); //Left Screen
        gameObject.transform.DOLocalMoveX(0, 0.4f).SetEase(Ease.OutQuart);
        _isShowRequest = true;
        _canvasGroup.interactable = true;

    }
    [Button]
    public void FadeOutContainer()
    {
        _canvasGroup.interactable = false;
        Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
        gameObject.transform.DOLocalMoveX(posCam.x - 2000f, 0.6f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });

    }
    #endregion
}
