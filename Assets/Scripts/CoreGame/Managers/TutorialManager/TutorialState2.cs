using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialState2 : BaseTutorialState
{
	private int currentStep;
	List<Action> listStep;
	public TutorialState2(TutorialManager tutorialManager) : base(tutorialManager)
	{
	}

	public override void Do()
	{
		if(currentStep == 3)
		{
			GotoNextStep();
		}
	}

	public override void Enter()
	{
		tutorialManager.SetCurrentState(2);
		tutorialManager.ShowHideGameUI(false);

		var curentPaw = PawManager.Instance.CurrentPaw;
		PawManager.Instance.RemovePaw(curentPaw - 20);

		//ShaftManager

		//khai bao
		currentStep = 0;
		listStep = new();
		listStep.Add(Step1);
		listStep.Add(Step2);
		listStep.Add(Step3);
		listStep.Add(Step4);
		listStep.Add(Step5);
		listStep.Add(Step6);
		listStep.Add(Step7);

		tutorialManager.gameUI.tutotrialUI.CreateTutorialClickNextStepButton();

		tutorialManager.gameUI.tutotrialUI.gameObject.SetActive(true);
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Cho tiền nè thuê nhân viên đi");

		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().AddShaftPanel.gameObject.SetActive(false);
		tutorialManager.gameUI.ButtonesUI[3].gameObject.SetActive(true);
		tutorialManager.gameUI.ButtonesUI[1].gameObject.SetActive(true);
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-310, -940);
		//bat lai khi thoat state	
		ManagersController.Instance.ManagerPrefab.HireManagerButton.enabled = false;
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager += ShowNextStepButton;
		
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.onClick.AddListener(GotoNextStep);

		PawManager.Instance.AddPaw(2100f);
	}

	private void Step1()
	{

		ManagersController.Instance.ManagerPrefab.HideShowAllUITabManagerUI(false);
		ManagersController.Instance.ManagerPrefab.ShowHideTopButton(true, 0);

		tutorialManager.gameUI.tutotrialUI.gameObject.transform.SetAsLastSibling();
		ManagersController.Instance.OpenManagerPanel();
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(false);

	}
	private void ShowNextStepButton()
	{
		
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(115, -888);
	}

	private void Step2()
	{
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager -= ShowNextStepButton;
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
		ManagersController.Instance.ManagerPrefab.HireManagerButton.onClick.Invoke();
		tutorialManager.gameUI.tutotrialUI.GetComponent<Image>().raycastTarget = false;
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Mỗi quản lý sẽ có sức mạnh riêng càng nhiều sao càng mạnh");
		ManagersController.Instance.ManagerPrefab.AddListenerFromFXGacha();
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager += ManagerFXGachaUIIsCloseHandle;
	}
	private void ManagerFXGachaUIIsCloseHandle()
	{
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager -= ManagerFXGachaUIIsCloseHandle;
		GotoNextStep();
	}
	private void Step3()
	{
		ManagersController.Instance.ManagerPrefab.OpenTutorialLine();
		//Bat lai khi thoat state
		ManagersController.Instance.ManagerPrefab._scrollRect.enabled = false;
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager += HandleTutorialStep4;
	}
	private void Step4()
	{
		ManagersController.Instance.ManagerPrefab.CloseTutorialLine();
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Thoát ra ngoài xem thành quả đi");

	}
	private void HandleTutorialStep4()
	{
		tutorialManager.gameUI.tutotrialUI.GetComponent<Image>().raycastTarget = true;
		ManagersController.Instance.ManagerPrefab.ShowHideTopButton(true, 1);
		ManagersController.Instance.ManagerPrefab.ShowHideTopButton(true, 2);
		ManagersController.Instance.ManagerPrefab.ShowActiveAllButton();
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-125, 696);
	}
	private void Step5()
	{
		ManagerLocationUI.OnTabChanged?.Invoke(ManagerLocation.Elevator);
		ManagersController.Instance.ManagerPrefab._locationTabUI.ManagerTabFilter.Dictionary.Keys.ElementAt(1).onClick.Invoke();
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(115, -888);
	}
	private void Step6()
	{
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
		ManagersController.Instance.ManagerPrefab.HireManagerButton.onClick.Invoke();
		tutorialManager.gameUI.tutotrialUI.GetComponent<Image>().raycastTarget = false;
		ManagersController.Instance.ManagerPrefab.OnInteractToTutorialManager += ManagerFXGachaUIIsCloseHandle;
	}
	private void Step7()
	{
		tutorialManager.gameUI.tutotrialUI.GetComponent<Image>().raycastTarget = true;
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-310, -940);
	}
	private void GotoNextStep()
	{
		listStep[currentStep++]();
	}
	public override void Exit()
	{
		
	}


}
