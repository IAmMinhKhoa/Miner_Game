using Sirenix.OdinInspector.Editor.GettingStarted;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialState2 : BaseTutorialState
{
	private int currentStep;
	private List<Action> listStep;

	public TutorialState2(TutorialManager tutorialManager) : base(tutorialManager) { }

	public override void Do()
	{
		if (currentStep == 3)
			GotoNextStep();
	}

	public override void Enter()
	{
		tutorialManager.SetCurrentState(2);
		tutorialManager.ShowHideGameUI(false);

		// Trừ PAW khi vào tutorial
		PawManager.Instance.RemovePaw(PawManager.Instance.CurrentPaw - 20);

		// Khởi tạo danh sách bước
		listStep = new List<Action> { Step1, Step2, Step3, Step4, Step5, Step6, Step7, Step8, Step9 };
		currentStep = 0;

		// Thiết lập UI ban đầu
		InitializeUI();

		// Sự kiện khi click Next
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.onClick.AddListener(GotoNextStep);

		// Cấp tiền cho người chơi
		PawManager.Instance.AddPaw(2100f);
	}

	private void InitializeUI()
	{
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		tutorialUI.CreateTutorialClickNextStepButton();
		tutorialUI.gameObject.SetActive(true);
		tutorialUI.SetTextTutorial("Cho tiền nè thuê nhân viên đi");

		var buttonsUI = tutorialManager.gameUI.ButtonesUI;
		buttonsUI[3].gameObject.SetActive(true);
		buttonsUI[1].gameObject.SetActive(true);

		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-310, -940);

		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().BuyNewShaftButton.gameObject.SetActive(false);

		// Disable button thuê quản lý
		ManagersController.Instance.ManagerPrefab.HireManagerButton.enabled = false;
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager += ShowNextStepButton;
	}

	private void Step1()
	{
		var managerPrefab = ManagersController.Instance.ManagerPrefab;
		managerPrefab.HideShowAllUITabManagerUI(false);
		managerPrefab.ShowHideTopButton(true, 0);

		tutorialManager.gameUI.tutotrialUI.gameObject.transform.SetAsLastSibling();
		ManagersController.Instance.OpenManagerPanel();

		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
	}

	private void ShowNextStepButton()
	{
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(115, -888);
	}

	private void Step2()
	{
		var managerPrefab = ManagersController.Instance.ManagerPrefab;

		managerPrefab.OnInteractToTutorialManager -= ShowNextStepButton;
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(false);

		managerPrefab.HireManagerButton.onClick.Invoke();
		tutorialManager.gameUI.tutotrialUI.GetComponent<Image>().raycastTarget = false;
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Mỗi quản lý sẽ có sức mạnh riêng càng nhiều sao càng mạnh");

		managerPrefab.AddListenerFromFXGacha();
		managerPrefab.OnInteractToTutorialManager += ManagerFXGachaUIIsCloseHandle;
	}

	private void ManagerFXGachaUIIsCloseHandle()
	{
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager -= ManagerFXGachaUIIsCloseHandle;
		GotoNextStep();
	}

	private void Step3()
	{
		var managerPrefab = ManagersController.Instance.ManagerPrefab;
		managerPrefab.OpenTutorialLine();
		managerPrefab._scrollRect.enabled = false;
	}

	private void Step4()
	{
		var managerPrefab = ManagersController.Instance.ManagerPrefab;
		managerPrefab.CloseTutorialLine();
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Thoát ra ngoài xem thành quả đi");
		managerPrefab.OnInteractToTutorialManager += ShowTextHireElevator;
	}

	private void ShowTextHireElevator()
	{
		var managerPrefab = ManagersController.Instance.ManagerPrefab;
		managerPrefab.OnInteractToTutorialManager -= ShowTextHireElevator;
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Thuê quản lý cho cả thang máy và quầy đi");
		tutorialManager.gameUI.tutorialState2UI.gameObject.SetActive(true);
		managerPrefab.OnInteractToTutorialManager += HandleTutorialStep4;
	}

	private void HandleTutorialStep4()
	{
		tutorialManager.gameUI.tutorialState2UI.gameObject.SetActive(false);
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		tutorialUI.GetComponent<Image>().raycastTarget = true;

		var managerPrefab = ManagersController.Instance.ManagerPrefab;
		managerPrefab.ShowHideTopButton(true, 1);
		managerPrefab.ShowHideTopButton(true, 2);
		managerPrefab.ShowActiveAllButton();

		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-125, 696);

		managerPrefab.OnInteractToTutorialManager -= HandleTutorialStep4;
	}

	private void Step5()
	{
		ManagerLocationUI.OnTabChanged?.Invoke(ManagerLocation.Elevator);
		ManagersController.Instance.ManagerPrefab._locationTabUI.ManagerTabFilter.Dictionary.Keys.ElementAt(1).onClick.Invoke();
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(115, -888);
	}

	private void Step6()
	{
		var managerPrefab = ManagersController.Instance.ManagerPrefab;
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
		managerPrefab.HireManagerButton.onClick.Invoke();
		tutorialManager.gameUI.tutotrialUI.GetComponent<Image>().raycastTarget = false;
		managerPrefab.AddListenerFromFXGacha();
		managerPrefab.OnInteractToTutorialManager += ManagerFXGachaUIIsCloseHandle;
	}

	private void Step7()
	{
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		tutorialUI.GetComponent<Image>().raycastTarget = true;
		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-276, 375);
	}

	private void Step8()
	{
		ManagersController.Instance.ManagerPrefab.ManagerSectionList.ManagerSectionUIList[0].ManagerGridUI.ManagerElementUiList[0].OnPointerClick(null);
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-194, -589);
	}

	private void Step9()
	{
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		var managerPrefab = ManagersController.Instance.ManagerDetailPrefab;
		managerPrefab.HireOrFiredButton.onClick.Invoke();
		managerPrefab.gameObject.SetActive(false);
		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
		tutorialUI.GetComponent<Image>().raycastTarget = false;
		tutorialManager.TutorialStateMachine.TransitonToState(TutorialState.State3);
	}

	private void GotoNextStep()
	{
		listStep[currentStep++]?.Invoke();
		Debug.Log($"Current Step: {currentStep}");
	}

	public override void Exit() {
		PlayFabManager.Data.PlayFabDataManager.Instance.SaveData("TutorialState", "3");
	}
}
