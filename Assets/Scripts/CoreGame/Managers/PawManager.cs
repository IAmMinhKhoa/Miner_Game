using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class PawManager : Patterns.Singleton<PawManager>
{
    [SerializeField] private string m_startingPaw = "10000000000";

    [SerializeField] private readonly string m_pawKey = "PawVollume";

    public double CurrentPaw { get; private set; }

    [Button]
    public void AddPaw(double amount)
    {
        CurrentPaw += amount;
        PlayerPrefs.SetString(m_pawKey, CurrentPaw.ToString());
        PlayerPrefs.Save();
    }

    public void RemovePaw(double amount)
    {
        CurrentPaw -= amount;
        PlayerPrefs.SetString(m_pawKey, CurrentPaw.ToString());
        PlayerPrefs.Save();
    }

    public async UniTaskVoid Save()
    {
        PlayerPrefs.SetString(m_pawKey, CurrentPaw.ToString());
        PlayerPrefs.Save();
    }

    private void LoadPaw()
    {
        var paw = PlayerPrefs.GetString(m_pawKey, m_startingPaw);
        Debug.Log("Paw from PlayerPrefs:" + paw);

        if (Double.TryParse(paw.ToString(), out double result))
        {
            Debug.Log("Current paw:" + result);
            CurrentPaw = result;
        }
        else
        {
            Debug.LogError("Could not parse paw value from PlayerPrefs");
        }
    }

    void Start()
    {
        LoadPaw();
    }
}

