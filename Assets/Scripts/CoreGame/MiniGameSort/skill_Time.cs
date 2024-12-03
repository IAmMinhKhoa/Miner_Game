using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class skill_Time : sortGameSkills
{
	[SerializeField] private SortGameManager gameManager;
	[SerializeField] private TextMeshProUGUI countdownText;
	private float skillActivatedTime = 10f;
	bool isSkillActivated = false;

	private void Awake()
	{
		countdownText.text = "";
	}

	private void Update()
	{
		if (isUsing)
		{
			if(skillActivatedTime > 0)
			{
				skillActivatedTime -= Time.deltaTime;
				countdownText.text = skillActivatedTime.ToString("F1");
			}
			else
			{
				isUsing = false;
				countdownText.text = "";
			}

		}
	}

	public override void ActiveSkill()
	{
		if (!isUsing)
		{
			isUsing = true;
			skillActivatedTime = 10f;
			gameManager.currentDelayTime = 10f;
		}
	}

}
