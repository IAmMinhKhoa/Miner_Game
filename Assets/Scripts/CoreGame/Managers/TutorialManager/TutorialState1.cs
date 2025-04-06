using Cysharp.Threading.Tasks;

using System;
using UnityEngine;

public class TutorialState1 : BaseTutorialState
{
	private int currentStep;
	private Action[] steps;
	private TutotrialUI tutorialUI;
	private ShaftUI shaftUI;

	public TutorialState1(TutorialManager tutorialManager) : base(tutorialManager) { }

	public override void Enter()
	{
		tutorialManager.SetCurrentState(1);
		tutorialManager.ShowHideGameUI(false);

		// Cache UI references
		tutorialUI = tutorialManager.gameUI.tutotrialUI;
		shaftUI = ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>();

		// Khởi tạo các bước
		currentStep = 0;
		steps = new Action[] { Step1, Step2, Step3 };

		// Thiết lập UI
		SetupTutorialUI();
		SetupShaftUI();
	}

	private void SetupTutorialUI()
	{
		tutorialUI.CreateTutorialClickNextStepButton();
		tutorialUI.gameObject.SetActive(true);
		tutorialUI.SetTextTutorial( LocalizationManager.GetLocalizedString(LanguageKeys.TutorialTitle1));
		tutorialUI.PlayAnimation("idle");
		tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -54);
		tutorialUI.TutorialClickNextStepButton.onClick.AddListener(GotoNextStep);
	}

	private void SetupShaftUI()
	{
		shaftUI.AddShaftPanel.gameObject.SetActive(false);
		shaftUI.activeWorkerButton.isClickable = false;
		shaftUI.BuyNewShaftButton.gameObject.SetActive(false);
	}

	public override void Do()
	{
		if (currentStep < steps.Length)
		{
			tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
			return;
		}

		tutorialUI.TriggerAddCoinEffect();
		tutorialManager.gameUI.ButtonesUI[1].gameObject.SetActive(true);
	}

	private void GotoNextStep()
	{
		if (currentStep < steps.Length)
		{
			steps[currentStep++]();
			tutorialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
		}
	}

	private void Step1()
	{
		ShaftManager.Instance.Shafts[0].AwakeWorker(true).Forget();
		tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-420, -190);
	}

	private void Step2()
	{

			ElevatorSystem.Instance.AwakeWorker(true);
			tutorialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(183, -680);

	}

	private void Step3()
	{
		Counter.Instance.AwakeWorker(true).Forget();
	}

	public override void Exit()
	{
		tutorialUI.DestroyTutorialClickNextStepButton();
		tutorialUI.TutorialClickNextStepButton.onClick.RemoveListener(GotoNextStep);
		shaftUI.activeWorkerButton.isClickable = true;
		PlayFabManager.Data.PlayFabDataManager.Instance.SaveData("TutorialState", "2");
	}
}
