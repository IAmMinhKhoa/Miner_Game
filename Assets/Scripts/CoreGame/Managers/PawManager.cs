using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawManager : Patterns.Singleton<PawManager>
{
    [SerializeField] private float m_startingPaw = 0;

    [SerializeField] private readonly string m_pawKey = "PawVollume";

    public float CurrentPaw { get; private set; }

    public void AddPaw(float amount)
    {
        CurrentPaw += amount;
        PlayerPrefs.SetFloat(m_pawKey, CurrentPaw);
        PlayerPrefs.Save();
    }

    public void RemovePaw(float amount)
    {
        CurrentPaw -= amount;
        PlayerPrefs.SetFloat(m_pawKey, CurrentPaw);
        PlayerPrefs.Save();
    }

    private void LoadPaw()
    {
        float paw = PlayerPrefs.GetFloat(m_pawKey, m_startingPaw);
        Debug.Log("Paw from PlayerPrefs:" + paw);

        CurrentPaw = paw;
    }

    void Start()
    {
        LoadPaw();
    }
}

