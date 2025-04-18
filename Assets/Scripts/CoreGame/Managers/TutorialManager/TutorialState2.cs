using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialState2 : BaseTutorialState
{
	private int currentStep;
	private List<Action> listStep;
	private bool step3AutoProgressed = false;

	public TutorialState2(TutorialManager tutorialManager) : base(tutorialManager) { }

	public override void Do()
	{
		if (currentStep == 3 && !step3AutoProgressed)
		{
			step3AutoProgressed = true;
			GotoNextStep();
		}
	}

	public override void Enter()
	{
		tutorialManager.SetCurrentState(2);
		tutorialManager.ShowHideGameUI(false);

		PawManager.Instance.RemovePaw(PawManager.Instance.CurrentPaw - 20);

		listStep = new List<Action> { Step1, Step2, Step3, Step4, Step4_1, Step5, Step6, Step7, Step8, Step9, Step10 };
		currentStep = 0;
		step3AutoProgressed = false;

		InitializeUI();

		// Đảm bảo không bị trùng listener
		var nextButton = tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton;
		if (nextButton != null)
		{
			nextButton.onClick.RemoveAllListeners();
			nextButton.onClick.AddListener(GotoNextStep);
		}

		PawManager.Instance.AddPaw(2500f);
	}

	private void InitializeUI()
	{
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		tutorialUI.CreateTutorialClickNextStepButton();
		tutorialUI.gameObject.SetActive(true);
		tutorialUI.SetTextTutorial(LocalizationManager.GetLocalizedString(LanguageKeys.TutorialTitle2));
		tutorialUI.PlayAnimation("idle2");

		var buttonsUI = tutorialManager.gameUI.ButtonesUI;
		buttonsUI[3].gameObject.SetActive(true);
		buttonsUI[1].gameObject.SetActive(true);

		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-310, -940);

		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().BuyNewShaftButton.gameObject.SetActive(false);

		ManagersController.Instance.ManagerPrefab.HireManagerButton.enabled = false;
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager += ShowNextStepButton;

		ManagersController.Instance.ManagerDetailPrefab.StateButton(true);
	}

	private void Step1()
	{
		var managerPrefab = ManagersController.Instance.ManagerPrefab;
		managerPrefab.HideShowAllUITabManagerUI(false);
		managerPrefab.ShowHideTopButton(true, 0);

		tutorialManager.gameUI.tutotrialUI.transform.SetAsLastSibling();
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
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		var managerPrefab = ManagersController.Instance.ManagerPrefab;

		managerPrefab.OnInteractToTutorialManager -= ShowNextStepButton;
		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(false);

		managerPrefab.HireManagerButton.onClick.Invoke();
		tutorialUI.GetComponent<Image>().raycastTarget = false;
		tutorialUI.SetTextTutorial(LocalizationManager.GetLocalizedString(LanguageKeys.TutorialTitle3));
		tutorialUI.PlayAnimation("idle");
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
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;

		managerPrefab.CloseTutorialLine();
		tutorialUI.SetTextTutorial(LocalizationManager.GetLocalizedString(LanguageKeys.TutorialTitle4));
		tutorialUI.PlayAnimation("idle");

		managerPrefab.OnInteractToTutorialManager += ShowTextHireElevator;
		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(265f, 934f);
	}

	private void Step4_1()
	{
		ManagersController.Instance.ManagerPrefab.ClosePanel();
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
	}

	private void ShowTextHireElevator()
	{
		var managerPrefab = ManagersController.Instance.ManagerPrefab;
		managerPrefab.OnInteractToTutorialManager -= ShowTextHireElevator;

		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		tutorialUI.SetTextTutorial(LocalizationManager.GetLocalizedString(LanguageKeys.TutorialTitle5));
		tutorialUI.PlayAnimation("idle");

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
		var tutorialUI = tutorialManager.gameUI.tutotrialUI;

		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
		managerPrefab.HireManagerButton.onClick.Invoke();

		tutorialUI.GetComponent<Image>().raycastTarget = false;

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
		var managerPrefab = ManagersController.Instance.ManagerDetailPrefab;
		managerPrefab.HireOrFiredButton.onClick.Invoke();
		managerPrefab.gameObject.SetActive(false);
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(265f, 934f);
	}

	private void Step10()
	{
		ManagersController.Instance.ManagerPrefab.ClosePanel();

		var tutorialUI = tutorialManager.gameUI.tutotrialUI;
		tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
		tutorialUI.GetComponent<Image>().raycastTarget = false;

		tutorialManager.TutorialStateMachine.TransitonToState(TutorialState.State3);
	}

	private void GotoNextStep()
	{
		if (currentStep >= listStep.Count)
		{
			Debug.LogWarning("TutorialStep out of range");
			return;
		}

		listStep[currentStep++]?.Invoke();
		Debug.Log($"Current Step: {currentStep}");
	}

	public override void Exit()
	{
		PlayFabManager.Data.PlayFabDataManager.Instance.SaveData("TutorialState", "3");
		ManagersController.Instance.ManagerPrefab.HireManagerButton.enabled = true;
		ManagersController.Instance.ManagerPrefab.ClosePanel();
	}
}
