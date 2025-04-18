using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NOOD;
using Sirenix.OdinInspector;
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
	[SerializeField] public GameObject modal_showEvent;
	public List<GameObject> ButtonesUI => buttonesUI;
	public ButtonBehavior BT_Manager => bt_Manager;
	protected override void Awake()
	{
		base.Awake();
		/*modal_settingUI = GameData.Instance.InstantiatePrefab(PrefabEnum.ModalSetting);
		modal_bankUI = GameData.Instance.InstantiatePrefab(PrefabEnum.ModalBank);
		modal_minigameUI = GameData.Instance.InstantiatePrefab(PrefabEnum.ModalMiniGame);
		modal_offlineUI = GameData.Instance.InstantiatePrefab(PrefabEnum.ModalOfflineMoney);*/

	}

	private async void Start()
	{

		Debug.Log(bt_AddHeart.name);
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

	private GameObject InstantiateModal(PrefabEnum prefab)
	{
		GameObject mainObj=GameData.Instance.InstantiatePrefab(prefab);
		mainObj.transform.SetParent(this.transform);
		mainObj.transform.localScale = Vector3.one;
		return mainObj;
	}
	#region EVENT
	[Button]
	public void OpenModalShowEvent()
	{
			modal_showEvent=InstantiateModal( PrefabEnum.ModalShowEvent);
			modal_showEvent.SetActive(true);
			modal_showEvent.GetComponent<ModalShowEvent>().OpenModal();
	}
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
		modal_settingUI=InstantiateModal( PrefabEnum.ModalSetting);
		modal_settingUI.GetComponent<SettingUI>().FadeInContainer();
	}
	public void OpenBank()
	{
		modal_bankUI=InstantiateModal( PrefabEnum.ModalBank);
		modal_bankUI.SetActive(true);
		modal_bankUI.GetComponent<BankUI>().FadeInContainer();
	}
	public void OpenMinigame()
	{
		modal_minigameUI=InstantiateModal( PrefabEnum.ModalMiniGame);

		modal_minigameUI.SetActive(true);
		RectTransform rect = modal_minigameUI.GetComponent<RectTransform>();
		// Đặt anchor về stretch toàn phần
		rect.anchorMin = Vector2.zero;
		rect.anchorMax = Vector2.one;
		rect.offsetMin = Vector2.zero;  // left, bottom
		rect.offsetMax = Vector2.zero;  // right, top

		// Đảm bảo vị trí Z = 0
		Vector3 pos = rect.anchoredPosition3D;
		pos.z = 0;
		rect.anchoredPosition3D = pos;
	}

	public async void OpenOffline(OfflineMoneyData money)
	{
		UnityEngine.Debug.Log("OpenOffline");
		modal_showEvent=InstantiateModal( PrefabEnum.ModalOfflineMoney);
		if (money.paw <= 0) return;
		if (modal_showEvent != null)
		{
			modal_showEvent.GetComponent<ModalShowEvent>().CloseModal();
		}
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
