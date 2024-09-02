using System;
using System.Collections;
using NOOD;
using Sirenix.OdinInspector;
using UnityEngine;

public class MoneyManager : MonoBehaviorInstance<MoneyManager>
{
    private const string MONEY_KEY = "Money";
    private const string DEFAULT_MONEY = "1000";

    public Action OnMoneyChanged;
    public float Money { get; private set; }
    private bool _isDone;
    public bool IsDone => _isDone;

    [Button]
    public void AddMoney(float amount)
    {
        Money += amount;
        OnMoneyChanged?.Invoke();
        Save();
    }
    public void RemoveMoney(float amount)
    {
        Money -= amount;
        OnMoneyChanged?.Invoke();
        Save();
    }

    public void LoadMoney()
    {
        var paw = PlayerPrefs.GetString(MONEY_KEY, DEFAULT_MONEY);
        Debug.Log("Money from PlayerPrefs:" + paw);
        _isDone = false;

        if (float.TryParse(paw.ToString(), out float result))
        {
            Debug.Log("Current paw:" + result);
            Money = result;
            OnMoneyChanged?.Invoke();
            _isDone = true;
        }
        else
        {
            Debug.LogError("Could not parse paw value from PlayerPrefs");
        }
    }

    private void Save()
    {
        PlayerPrefs.SetString(MONEY_KEY, Money.ToString());
        PlayerPrefs.Save();
    }
}
