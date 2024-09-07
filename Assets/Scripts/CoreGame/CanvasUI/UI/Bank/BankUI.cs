using System;
using DG.Tweening;
using NOOD.SerializableDictionary;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<int, int> _moneyPackageDictionary = new SerializableDictionary<int, int>();
    [SerializeField] private SerializableDictionary<int, int> _pawPackageDictionary = new SerializableDictionary<int, int>();

    [SerializeField] private MoneyButton _moneyBtnPref;
    [SerializeField] private PawButton _pawBtnPref;
    [SerializeField] private Transform _moneyContentTrans, _pawContentTrans;
    [SerializeField] private RectTransform _content;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Button _closeButton;
    [SerializeField] private float _fadeSpeed = 0.3f;
    private bool _isShowRequest;

    void Start()
    {
        LoadFromDictionary();
        _moneyBtnPref.gameObject.SetActive(false);
        _pawBtnPref.gameObject.SetActive(false);
        _closeButton.onClick.AddListener(Hide);
        if (_isShowRequest == false) // Prevent Hide when press shop button
            Hide();
    }
    private void LoadFromDictionary()
    {
        foreach (var moneyPair in _moneyPackageDictionary.Dictionary)
        {
            MoneyButton moneyBtn = Instantiate<MoneyButton>(_moneyBtnPref, _moneyContentTrans);
            moneyBtn.gameObject.SetActive(true);
            moneyBtn.SetData(moneyPair.Key, moneyPair.Value, this);
        }
        foreach (var pawPair in _pawPackageDictionary.Dictionary)
        {
            PawButton pawBtn = Instantiate<PawButton>(_pawBtnPref, _pawContentTrans);
            pawBtn.gameObject.SetActive(true);
            pawBtn.SetData(pawPair.Key, pawPair.Value, this);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
        }
    }

    // !Only call this function if transaction success
    public void BuyMoney(float amount)
    {
        MoneyManager.Instance.AddMoney(amount);
    }
    public void BuyPaw(float amount, float price)
    {
        if (MoneyManager.Instance.Money >= price)
        {
            MoneyManager.Instance.RemoveMoney(price);
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
