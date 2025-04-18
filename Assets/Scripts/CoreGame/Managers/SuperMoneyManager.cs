using System;
using System.Collections;
using NOOD;
using Sirenix.OdinInspector;
using UnityEngine;

public class SuperMoneyManager : MonoBehaviorInstance<SuperMoneyManager>
{
    private const string MONEY_KEY = "Money";
    private const string DEFAULT_MONEY = "1000";

    public Action OnMoneyChanged;
    public float SuperMoney { get; private set; }
    private bool _isDone;
    public bool IsDone => _isDone;

    [Button]
    public void AddMoney(float amount)
    {
        SuperMoney += amount;
        OnMoneyChanged?.Invoke();
        Save();
    }
    public void RemoveMoney(float amount)
    {
        SuperMoney -= amount;
        OnMoneyChanged?.Invoke();
        Save();
    }

    public void LoadMoney()
    {
        /*var paw = PlayerPrefs.GetString(MONEY_KEY, DEFAULT_MONEY);
        Debug.Log("Money from PlayerPrefs:" + paw);
        _isDone = false;

        if (float.TryParse(paw.ToString(), out float result))
        {
            Debug.Log("Current paw:" + result);
            SuperMoney = result;
            OnMoneyChanged?.Invoke();
            _isDone = true;
        }
        else
        {
            Debug.LogError("Could not parse paw value from PlayerPrefs");
        }*/

        var spMoney = PlayFabManager.Data.PlayFabDataManager.Instance.GetData(MONEY_KEY);
        Debug.Log("Money from server:" + spMoney);

        if (float.TryParse(spMoney.ToString(), out float result))
        {
	        Debug.Log("Current paw:" + result);
	        SuperMoney = result;
	        OnMoneyChanged?.Invoke();
	        _isDone = true;
        }
        else
        {
	      //  Debug.LogError("Could not parse paw value from PlayerPrefs");
        }
    }

    public void Save()
    {
        /*PlayerPrefs.SetString(MONEY_KEY, SuperMoney.ToString());
        PlayerPrefs.Save();*/

        PlayFabManager.Data.PlayFabDataManager.Instance.SaveData(MONEY_KEY, SuperMoney.ToString());
    }
}
