using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UI.Inventory;
using UnityEngine.SceneManagement;

public class GameUI : Patterns.Singleton<GameUI> //GAME HUD (MANAGER UI GAME)

{
	[Header("UI TEXT")]
	[SerializeField] private TextMeshProUGUI pawText;
	[SerializeField] private TextMeshProUGUI superMoneyText;
	[SerializeField] private TextMeshProUGUI pawPerSecondText;

	[Header("Top UI Button")]
	[SerializeField] private ButtonBehavior bt_AddHeart;
	[SerializeField] private ButtonBehavior bt_AddCoin;
	[SerializeField] private ButtonBehavior bt_Setting;
	[SerializeField] private ButtonBehavior bt_Minigame;

	[Header("Bottom UI Button")]
	[SerializeField] private ButtonBehavior bt_Manager;
	[SerializeField] private ButtonBehavior bt_Inventory;
	[SerializeField] private ButtonBehavior bt_Store;
	[SerializeField] private ButtonBehavior bt_Sound;

	[Header("Prefab Modal")]
	[SerializeField] private GameObject modal_Inventory;
	[SerializeField] private GameObject modal_soundSetting;
	[SerializeField] private GameObject modal_settingUI;
	[SerializeField] private GameObject modal_bankUI;
	private void Start()
	{
		bt_AddHeart.onClickEvent.AddListener(OpenBank);
		bt_AddCoin.onClickEvent.AddListener(OpenBank);
		bt_Setting.onClickEvent.AddListener(OpenSetting);
		bt_Minigame.onClickEvent.AddListener(OpenMinigame);

		bt_Manager.onClickEvent.AddListener(OpenBookManager);
		bt_Inventory.onClickEvent.AddListener(OpenInventory);
		bt_Store.onClickEvent.AddListener(OpenStore);
		bt_Sound.onClickEvent.AddListener(OpenSound);
	}

	#region EVENT
	private void OpenBookManager()
	{
		ManagersController.Instance.OpenManagerPanel();
	}
	public void OpenInventory()
	{
		modal_Inventory.GetComponent<InventoryUIManager>().FadeInContainer();
	}
	public void OpenStore()
	{
		
	}
	public void OpenSound()
	{
		modal_soundSetting.GetComponent<SoundSetting>().FadeInContainer();
	}
	public void OpenSetting()
	{
		modal_settingUI.GetComponent<SettingUI>().FadeInContainer();
	}
	public void OpenBank()
	{
		modal_bankUI.GetComponent<BankUI>().FadeInContainer();
	}
	public void OpenMinigame()
	{
		SceneManager.LoadSceneAsync("DemoMinigame");
	}
	#endregion

	private void Update()
	{
		pawText.text = Currency.DisplayCurrency(PawManager.Instance.CurrentPaw);
		superMoneyText.text = Currency.DisplayCurrency(SuperMoneyManager.Instance.SuperMoney);

		pawPerSecondText.text = "+" + Currency.DisplayCurrency(OfflineManager.Instance.GetNSPaw()) + "/s";
	}
}
