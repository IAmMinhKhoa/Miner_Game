using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OfflineMoneyUI : MonoBehaviour
{
    [SerializeField] private GameObject m_offlineMoneyText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button doubleUpButton;
    [SerializeField] private TMP_Text m_timeText;

    private double m_offlineMoney;

    void OnEnable()
    {
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        doubleUpButton.onClick.AddListener(OnDoubleUpButtonClicked);
    }

    void OnDisable()
    {
        confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
        doubleUpButton.onClick.RemoveListener(OnDoubleUpButtonClicked);
    }

    public void SetOfflineMoney(OfflineMoneyData money)
    {
        m_offlineMoney = money.paw;
        SetOfflineMoneyText(Currency.DisplayCurrency(m_offlineMoney));

        //Make time from duration
        TimeSpan time = System.TimeSpan.FromSeconds(money.time);
        m_timeText.text = string.Format("{0:D2} giờ :{1:D2} phút :{2:D2} giây", time.Hours, time.Minutes, time.Seconds);
    }

    public void SetOfflineMoneyText(string text)
    {
        m_offlineMoneyText.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }

    void OnConfirmButtonClicked()
    {
        // Add offline money to player's paw
        PawManager.Instance.AddPaw(m_offlineMoney);
        // Close offline money UI
        gameObject.SetActive(false);
    }

    void OnDoubleUpButtonClicked()
    {
        m_offlineMoney *= 2;
        PawManager.Instance.AddPaw(m_offlineMoney);
        // Close offline money UI
        gameObject.SetActive(false);
    }
}
