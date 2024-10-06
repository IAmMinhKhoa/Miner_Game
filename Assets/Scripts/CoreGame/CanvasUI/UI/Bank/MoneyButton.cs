using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class MoneyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField][ReadOnly] private float _moneyReward, _price;
    private BankUI _bankUI;

    public void SetData(float money, float price, CultureInfo cultureInfo, BankUI bankUI)
    {
        if (cultureInfo.Name == "vi-VN")
        {
            _priceText.text = $"{price:N0}đ";
        }
        else
        {
            _priceText.text = $"${price:F2}";
        }
        _moneyReward = money;
        _price = price;
        _bankUI = bankUI;
    }

    public void OnTransactionComplete()
    {
        // Player paid money to buy in game money    
        _bankUI.BuyMoney(_moneyReward);
    }
}
