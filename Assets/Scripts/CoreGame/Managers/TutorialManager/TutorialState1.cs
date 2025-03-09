using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;

public class TutorialState1 : BaseTutorialState
{
	int curentStep;
	List<Action> stepFuncList;
	public TutorialState1(TutorialManager tutorialManager) : base(tutorialManager)
	{
	}

	public override void Do()
	{
		if(curentStep < 3)
		{
			tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(true);
			return;
		}

		tutorialManager.gameUI.tutotrialUI.TriggerAddCoinEffect();
		tutorialManager.gameUI.ButtonesUI[1].gameObject.SetActive(true);



	}

	public override void Enter()
	{
		tutorialManager.SetCurrentState(1);
		stepFuncList = new();
		tutorialManager.ShowHideGameUI(false);
		curentStep = 0;
		tutorialManager.gameUI.tutotrialUI.CreateTutorialClickNextStepButton();
		tutorialManager.gameUI.tutotrialUI.gameObject.SetActive(true);
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Chuyển trà sửa từ tầng xuống quầy để bán đi");
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -54);

		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().AddShaftPanel.gameObject.SetActive(false);
		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().activeWorkerButton.isClickable = false;

		stepFuncList.Add(Step1);
		stepFuncList.Add(Step2);
		stepFuncList.Add(Step3);
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.onClick.AddListener(GotoNextStep);
	}

	public void GotoNextStep()
	{
		stepFuncList[curentStep++]();
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.gameObject.SetActive(false);
	}
	public void Step1()
	{
		ShaftManager.Instance.Shafts[0].AwakeWorker(true).Forget();
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-420, -190);
	}
	public void Step2()
	{
		ElevatorSystem.Instance.AwakeWorker(true);
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -680);
	}
	public void Step3()
	{
		Counter.Instance.AwakeWorker(true).Forget();
	}
	
	public override void Exit()
	{
		tutorialManager.gameUI.tutotrialUI.DestroyTutorialClickNextStepButton();
		tutorialManager.gameUI.tutotrialUI.TutorialClickNextStepButton.onClick.RemoveListener(GotoNextStep);
		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().activeWorkerButton.isClickable = true;
	}
}
