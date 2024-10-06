using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PawButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scaleAmount;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField][ReadOnly] private double _pawReward;
    [SerializeField] private float _price;
    private BankUI _bankUI;
    private Button _button;

    void Awake()
    {
        _button = this.gameObject.GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            _bankUI.BuyPaw(_pawReward, _price);
        });
    }

    public void SetData(int scaleAmount, int price, BankUI bankUI)
    {
        _pawReward = scaleAmount * 3600 * OfflineManager.Instance.GetNSPaw();
        _price = price;
        _scaleAmount.text = Currency.DisplayCurrency(_pawReward);
        _priceText.text = $"<sprite name=heart>{price}";
        _bankUI = bankUI;
    }
}
