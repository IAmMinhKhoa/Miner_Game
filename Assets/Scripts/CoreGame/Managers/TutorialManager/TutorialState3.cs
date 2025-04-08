using System.Collections;
using System.Collections.Generic;
using NOOD.Sound;
using UnityEngine;
using UnityEngine.UI;

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
		tutorialManager.gameUI.tutotrialUI.gameObject.SetActive(false);
		var state2TutorialUI = tutorialManager.gameUI.tutorialState2UI;
		state2TutorialUI.SetActive(false);
	}

	public override void Enter()
	{
		tutorialManager.SetCurrentState(3);
		tutorialManager.ShowHideGameUI(false);
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial( LocalizationManager.GetLocalizedString(LanguageKeys.TutorialTitle6));
		tutorialManager.gameUI.tutotrialUI.PlayAnimation("idle3");
		tutorialManager.gameUI.tutotrialUI.GetComponent<Image>().raycastTarget = false;
		var state2TutorialUI = tutorialManager.gameUI.tutorialState2UI;
		state2TutorialUI.SetActive(true);
		state2TutorialUI.GetComponent<RectTransform>().anchoredPosition = new Vector2(671, 976);
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

		SoundManager.PlaySound(SoundEnum.donetutorial);
		Debug.Log("Tutorial State 3");

		//Mở Khóa button bán quản lý
		ManagersController.Instance.ManagerDetailPrefab.StateButton(false);
	}

	public override void Exit()
	{

	}
}
