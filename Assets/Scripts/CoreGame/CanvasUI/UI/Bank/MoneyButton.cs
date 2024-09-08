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
    [SerializeField] private TextMeshProUGUI _moneyText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField][ReadOnly] private int _moneyReward, _price;
    private BankUI _bankUI;

    public void SetData(int money, int price, BankUI bankUI)
    {
        _moneyText.text = money.ToString();
        _priceText.text = price.ToString("C0", CultureInfo.CreateSpecificCulture("vi-VN"));
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
