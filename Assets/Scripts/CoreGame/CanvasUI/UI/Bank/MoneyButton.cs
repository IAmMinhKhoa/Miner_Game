using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class MoneyButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _rewardText;
    public IdBundle _idBundle;
    [SerializeField]private float _moneyReward;
    [ReadOnly] private string _priceItem;
    private BankUI _bankUI;

    private async void Start()
    {
	    // Đợi đến khi IAPManager.Instance.IsInitialized() == true
	    while (!IAPManager.Instance.IsInitialized())
	    {
		    await Task.Delay(1000); // đợi 100ms rồi check lại
	    }

	    // Sau khi đã sẵn sàng → tiếp tục
	    InitData();
    }


    public void BuyItem()
    {
	    IAPManager.Instance.BuyProduct(_idBundle.ToString());
    }

    public async void InitData( )
    {

	    _priceItem=await IAPManager.Instance.GetLocalizedPriceAsync(_idBundle.ToString());
	    _priceText.text = _priceItem;
	    _rewardText.text = $"X"+_moneyReward;
    }

}
