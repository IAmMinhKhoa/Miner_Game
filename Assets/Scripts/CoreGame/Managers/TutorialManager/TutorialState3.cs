using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialState3 : BaseTutorialState
{
	public TutorialState3(TutorialManager tutorialManager) : base(tutorialManager)
	{
	}

	public override void Do()
	{
		PlayFabManager.Data.PlayFabDataManager.Instance.SaveData("TutorialState", "-1");
		tutorialManager.SetCurrentState(-1);
		tutorialManager.ShowHideGameUI(true);
		tutorialManager.EndTutorial();
		tutorialManager.gameUI.tutorialState3UI.gameObject.SetActive(false);

	}

	public override void Enter()
	{
		tutorialManager.SetCurrentState(3);
		tutorialManager.ShowHideGameUI(false);
		tutorialManager.gameUI.tutorialState3UI.gameObject.SetActive(true);
		//Shaft UI button
		foreach (var shaft in ShaftManager.Instance.Shafts)
		{
			var shaftUI = shaft.GetComponent<ShaftUI>();
			shaftUI.ManagerButton.gameObject.GetComponent<CanvasGroup>().alpha = 1;
			shaftUI.UpgradeButton.gameObject.SetActive(true);
		}
		//CounterUI
		var counterUI = Counter.Instance.GetComponent<CounterUI>();
		counterUI.ManagerButton.gameObject.GetComponent<CanvasGroup>().alpha = 1;
		counterUI.UpgradeButton.gameObject.SetActive(true);

		//ElevatorUI
		var elevatorUI = ElevatorSystem.Instance.GetComponent<ElevatorUI>();
		elevatorUI.ManagerButton.gameObject.GetComponent<CanvasGroup>().alpha = 1;
		elevatorUI.UpgradeButton.gameObject.SetActive(true);

		ShaftManager.Instance.Shafts[0].GetComponent<ShaftUI>().BuyNewShaftButton.gameObject.SetActive(true);
		var buttonsUI = tutorialManager.gameUI.ButtonesUI;
		buttonsUI[3].SetActive(true);
		buttonsUI[1].SetActive(true);
		buttonsUI[4].SetActive(true);
		buttonsUI[7].SetActive(true);
		Debug.Log("Tutorial State 3");
	}

	public override void Exit()
	{
		
	}
}
