using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawManager : Patterns.Singleton<PawManager>
{
    [SerializeField] private string m_startingPaw = "500";

    [SerializeField] private readonly string m_pawKey = "PawVollume";

    public double CurrentPaw { get; private set; }

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

    public void Save()
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

