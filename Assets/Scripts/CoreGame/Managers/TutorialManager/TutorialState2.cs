using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialState2 : BaseTutorialState
{
	public TutorialState2(TutorialManager tutorialManager) : base(tutorialManager)
	{
	}

	public override void Do()
	{
		
	}

	public override void Enter()
	{
		tutorialManager.gameUI.tutotrialUI.GetComponent<Image>().raycastTarget = false;
		tutorialManager.gameUI.tutotrialUI.SetTextTutorial("Cho tiền nè thuê nhân viên đi");
		PawManager.Instance.AddPaw(2100f);
		tutorialManager.gameUI.ButtonesUI[3].gameObject.SetActive(true);
		//tutorialManager.gameUI.tutorialButton

	}

	public override void Exit()
	{
		
	}


}
