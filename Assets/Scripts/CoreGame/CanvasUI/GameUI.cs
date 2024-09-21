using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUI : Patterns.Singleton<GameUI> //GAME HUD (MANAGER UI GAME)

{
	[Header("UI TEXT")]
	[SerializeField] private TextMeshProUGUI pawText;
	[SerializeField] private TextMeshProUGUI superMoneyText;
	[SerializeField] private TextMeshProUGUI pawPerSecondText;
	[Header("UI Button")]
	[SerializeField] private Button btn_Manager;
	private void Start()
	{
		btn_Manager.onClick.AddListener(OpenBookManager);

	}

	#region EVENT
	private void OpenBookManager()
	{
		ManagersController.Instance.OpenManagerPanel();
	}
	#endregion

	private void Update()
	{
		pawText.text = Currency.DisplayCurrency(PawManager.Instance.CurrentPaw);
		superMoneyText.text = Currency.DisplayCurrency(SuperMoneyManager.Instance.SuperMoney);

		pawPerSecondText.text = "+" + Currency.DisplayCurrency(OfflineManager.Instance.GetNSPaw()) + "/s";
	}
}
