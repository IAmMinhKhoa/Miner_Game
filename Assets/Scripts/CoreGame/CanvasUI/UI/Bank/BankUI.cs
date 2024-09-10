using System;
using DG.Tweening;
using NOOD.SerializableDictionary;
using Sirenix.OdinInspector;
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
        FadeInContainer();
        _canvasGroup.interactable = true;
    }
    public void Hide()
    {
        _canvasGroup.interactable = false;
		FadeOutContainer();
    }


	#region AnimateUI
	[Button]
	public void FadeInContainer()
	{
		gameObject.SetActive(true);
		Vector2 posCam = CustomCamera.Instance.GetCurrentTransform().position;
		Debug.Log("khoaa:" + posCam);
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
