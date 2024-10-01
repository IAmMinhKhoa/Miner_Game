using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

public class PawManager : Patterns.Singleton<PawManager>
{
    public Action<double> OnPawChanged;
    [SerializeField] private string m_startingPaw = "1000000000000000000";
    [SerializeField] private readonly string m_pawKey = "PawVolume";

    public double CurrentPaw { get; private set; }

    private bool isDone = false;
    public bool IsDone => isDone;

    [Button]
    public void AddPaw(double amount)
    {
        CurrentPaw += amount;
        OnPawChanged?.Invoke(CurrentPaw);
        Save();
    }

    public void RemovePaw(double amount)
    {
        CurrentPaw -= amount;
        OnPawChanged?.Invoke(CurrentPaw);
        Save();
    }

    public void Save()
    {
        PlayFabManager.Data.PlayFabDataManager.Instance.SaveData(m_pawKey, CurrentPaw.ToString());
    }

    public void LoadPaw()
    {
        var paw = PlayFabManager.Data.PlayFabDataManager.Instance.GetData(m_pawKey);
        Debug.Log("Paw from PlayerPrefs:" + paw);
        paw = paw == "" ? m_startingPaw : paw;


        if (Double.TryParse(paw.ToString(), out double result))
        {
            Debug.Log("Current paw:" + result);
            CurrentPaw = result;
            OnPawChanged?.Invoke(CurrentPaw);
            isDone = true;
        }
        else
        {
            Debug.LogError("Could not parse paw value from PlayerPrefs");
        }
    }

    [Button]
    public void CalculateBonus(float offlineTime)
    {
        OfflineManager.Instance.TestPawBonus(offlineTime);
    }


}

