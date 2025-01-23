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
		
	}

	public override void Enter()
	{
		tutorialManager.ShowHideGameUI(false);
		curentStep = 0;
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Chuyển trà sửa từ tầng xuống quầy để bán đi");
		tutorialManager.gameUI.tutorialButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(100, -54);
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
	}
	public void Step1()
	{
		tutorialManager.gameUI.tutotrialUI.HideTutorialText();
	}
	public void Step2()
	{
		Debug.Log("Step2");
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
