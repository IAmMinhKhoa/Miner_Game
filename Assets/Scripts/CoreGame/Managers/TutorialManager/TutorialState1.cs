using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Antlr3.Runtime.Misc;
using UnityEngine;

public class TutorialState1 : BaseTutorialState
{
	int curentStep;
	List<Action> stepFuncList = new();
	public TutorialState1(TutorialManager tutorialManager) : base(tutorialManager)
	{
	}

	public override void Do()
	{
		tutorialManager.gameUI.tutorialButton.gameObject.SetActive(true);
	}

	public override void Enter()
	{
		tutorialManager.ShowHideGameUI(false);
		curentStep = 0;
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Chuyển trà sửa từ tầng xuống quầy để bán đi");
		tutorialManager.gameUI.tutorialButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -54);

		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().AddShaftPanel.gameObject.SetActive(false);
		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().activeWorkerButton.isClickable = false;

		stepFuncList.Add(Step1);
		stepFuncList.Add(Step2);
		stepFuncList.Add(Step3);
		stepFuncList.Add(Step4);
		stepFuncList.Add(Step5);
		tutorialManager.gameUI.tutorialButton.onClick.AddListener(GotoNextStep);
	}

	public void GotoNextStep()
	{
		stepFuncList[curentStep++]();
		tutorialManager.gameUI.tutorialButton.gameObject.SetActive(false);
	}
	public void Step1()
	{
		tutorialManager.gameUI.tutotrialUI.HideTutorialText();
		ShaftManager.Instance.Shafts[0].AwakeWorker(true).Forget();
		tutorialManager.gameUI.tutorialButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-420, -190);
	}
	public void Step2()
	{
		ElevatorSystem.Instance.AwakeWorker(true);
		tutorialManager.gameUI.tutorialButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -680);
	}
	public void Step3()
	{
		Debug.Log("Step3");
	}
	public void Step4()
	{
		Debug.Log("Step4");
	}
	public void Step5()
	{
		Debug.Log("Step5");
	}
	
	public override void Exit()
	{
		
	}
}
