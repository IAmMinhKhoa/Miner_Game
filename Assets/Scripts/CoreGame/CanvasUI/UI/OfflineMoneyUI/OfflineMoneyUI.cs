using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Sirenix.OdinInspector;

public class OfflineMoneyUI : MonoBehaviour
{
    [SerializeField] private GameObject m_offlineMoneyText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button doubleUpButton;
    [SerializeField] private TMP_Text m_timeText;
	[SerializeField] private CollectorFx collectFX;
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
        string hourText = LocalizationManager.GetLocalizedString(LanguageKeys.Hour);
        string minuteText = LocalizationManager.GetLocalizedString(LanguageKeys.Minutes);
        string secondText = LocalizationManager.GetLocalizedString(LanguageKeys.Seconds);

        m_timeText.text = string.Format("{0:D2} {1} :{2:D2} {3} :{4:D2} {5}",
	        time.Hours, hourText,
	        time.Minutes, minuteText,
	        time.Seconds, secondText);
    }

    public void SetOfflineMoneyText(string text)
    {
        m_offlineMoneyText.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }

    void OnConfirmButtonClicked()
    {
		StartCoroutine(confirmClaimPaw(m_offlineMoney, 35));
	}

    void OnDoubleUpButtonClicked()
    {
        m_offlineMoney *= 2;

		StartCoroutine(confirmClaimPaw(m_offlineMoney,50));
    }
	IEnumerator confirmClaimPaw(double pawOffline, int quantityFx=5)
	{
		PawManager.Instance.AddPaw(pawOffline);
		 collectFX.SpawnAndMoveCoin(quantityFx, this.transform,scale:0.8f);
		yield return new WaitForSeconds(3f);
		// Close offline money UI
		gameObject.SetActive(false);
	}

	[Button]
	private void testCollector(int quantity=15)
	{
		collectFX.SpawnAndMoveCoin(quantity, this.transform,scale:0.8f);
	}
}
