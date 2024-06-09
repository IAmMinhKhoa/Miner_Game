using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;

public class GoldManager : Patterns.Singleton<GoldManager>
{
    [SerializeField]
    private Text m_goldText;
    [SerializeField]
    private string m_startingGold = "100";
    [SerializeField]
    private readonly string m_goldKey = "BasicGold";

    public BigInteger CurrentGold { get; private set; }

    private void AddGold(BigInteger amount)
    {
        CurrentGold += amount;
        PlayerPrefs.SetString(m_goldKey, CurrentGold.ToString());
        PlayerPrefs.Save();
    }

    private void SpendGold(BigInteger amount)
    {
        CurrentGold -= amount;
        PlayerPrefs.SetString(m_goldKey, CurrentGold.ToString());
        PlayerPrefs.Save();
    }

    private void LoadGold()
    {
        string gold = PlayerPrefs.GetString(m_goldKey, m_startingGold);

        //Parse string must be constant
        if (BigInteger.TryParse(gold.ToString(), out BigInteger result))
        {
            CurrentGold = result;
        }
        else
        {
            //Delete later
            // Handle the case where the string could not be parsed
            Debug.LogError("Could not parse gold value from PlayerPrefs");
        }

    }

    void Start()
    {
        LoadGold();
    }
    void Update()
    {
        m_goldText.text = Currency.DisplayCurrency(CurrentGold.ToString());
        CurrentGold = (CurrentGold * 101) / 100;
    }
}
