using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class PawButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _pawnText;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField][ReadOnly] private int _pawReward, _price;

    public void SetData(int money, int price)
    {
        _pawnText.text = money.ToString();
        _priceText.text = price.ToString();
        _pawReward = money;
        _price = price;
    }
}
