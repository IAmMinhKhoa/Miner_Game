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
	[SerializeField] List<GameObject> buttonesUI;

	[Header("TutorialUI")]
	[SerializeField] public TutotrialUI tutotrialUI;
	[SerializeField] public GameObject tutorialState3UI;
	[SerializeField] public GameObject tutorialState2UI;

	[Header("Prefab Modal")]
	[SerializeField] private GameObject modal_Inventory;
	[SerializeField] private GameObject modal_soundSetting;
	[SerializeField] private GameObject modal_settingUI;
	[SerializeField] private GameObject modal_bankUI;
	[SerializeField] private GameObject modal_storeUI;
	[SerializeField] private GameObject modal_minigameUI;

	[SerializeField] private GameObject modal_offlineUI;

	public List<GameObject> ButtonesUI => buttonesUI;
	public ButtonBehavior BT_Manager => bt_Manager;
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

		StartCoroutine(UpdateUIEverySecond());
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
		modal_storeUI.GetComponent<MarketUIHandler>().FadeInContainer();
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
		modal_bankUI.SetActive(true);
		modal_bankUI.GetComponent<BankUI>().FadeInContainer();
	}
	public void OpenMinigame()
	{
		modal_minigameUI.SetActive(true);
	}

	public async void OpenOffline(OfflineMoneyData money)
	{
		UnityEngine.Debug.Log("OpenOffline");
		if (money.paw <= 0) return;
		modal_offlineUI.GetComponent<OfflineMoneyUI>().SetOfflineMoney(money);
		modal_offlineUI.SetActive(true);
	}
	#endregion

	private IEnumerator UpdateUIEverySecond()
	{
		while (true)
		{
			// Update the UI elements every second
			pawText.text = Currency.DisplayCurrency(PawManager.Instance.CurrentPaw);
			superMoneyText.text = Currency.DisplayCurrency(SuperMoneyManager.Instance.SuperMoney);
			pawPerSecondText.text = "+" + Currency.DisplayCurrency(OfflineManager.Instance.GetNSPaw()) + "/s";

			// Wait for 1 second before the next update
			yield return new WaitForSeconds(0.5f);
		}
	}
	//Show Hide game UI
	public void ButtonesInteract(bool enable)
	{
		foreach(var button in buttonesUI)
		{
			button.SetActive(enable);
		}
		//Shaft UI button
		foreach(var shaft in ShaftManager.Instance.Shafts)
		{
			var shaftUI = shaft.GetComponent<ShaftUI>();
			shaftUI.ManagerButton.gameObject.GetComponent<CanvasGroup>().alpha = enable ? 1: 0;
			shaftUI.UpgradeButton.gameObject.SetActive(enable);
		}
		//CounterUI
		var counterUI = Counter.Instance.GetComponent<CounterUI>();
		counterUI.ManagerButton.gameObject.GetComponent<CanvasGroup>().alpha = enable ? 1 : 0;
		counterUI.UpgradeButton.gameObject.SetActive(enable);

		//ElevatorUI
		var elevatorUI = ElevatorSystem.Instance.GetComponent<ElevatorUI>();
		elevatorUI.ManagerButton.gameObject.GetComponent<CanvasGroup>().alpha = enable ? 1 : 0;
		elevatorUI.UpgradeButton.gameObject.SetActive(enable);
	}
}
