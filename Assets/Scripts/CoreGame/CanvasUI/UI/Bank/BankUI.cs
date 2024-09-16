using System;
using System.Collections.Generic;
using DG.Tweening;
using NOOD.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private List<Button> _closeButtons;
    [SerializeField] private float _fadeSpeed = 0.3f;
    private bool _isShowRequest;

    void Start()
    {
        _closeButtons.ForEach(x => x.onClick.AddListener(Hide));
        if (_isShowRequest == false) // Prevent Hide when press shop button
            Hide();
    }

    // !Only call this function if transaction success
    public void BuyMoney(float amount)
    {
        SuperMoneyManager.Instance.AddMoney(amount);
    }
    public void BuyPaw(float amount, float price)
    {
        if (SuperMoneyManager.Instance.SuperMoney >= price)
        {
            SuperMoneyManager.Instance.RemoveMoney(price);
            PawManager.Instance.AddPaw(amount);
        }
        else
        {
            // Announce player don't have enough money
        }
    }

    public void Show()
    {
        _isShowRequest = true;
        this.gameObject.SetActive(true);
        _canvasGroup.DOFade(1, _fadeSpeed).SetEase(Ease.Flash);
        _canvasGroup.interactable = true;
    }
    public void Hide()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.DOFade(0, _fadeSpeed).SetEase(Ease.Flash).OnComplete(() => this.gameObject.SetActive(false));
    }
}
