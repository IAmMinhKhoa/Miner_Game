using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PawButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pawnText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField][ReadOnly] private int _pawReward, _price;
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

    public void SetData(int money, int price, BankUI bankUI)
    {
        _pawnText.text = money.ToString();
        _priceText.text = price.ToString();
        _pawReward = money;
        _price = price;
        _bankUI = bankUI;
    }
}
