using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillsSortGameManager : MonoBehaviour
{
    public List<sortGameSkills> skills;
	public List<TextMeshProUGUI> skillCountText;
	public List<GameObject> buttons;
	public List<Sprite> activeSprites, buySprites;

	void Start()
	{
		buttons[0].GetComponent<Button>().onClick.AddListener(OnClickSkillFreeze);
		buttons[1].GetComponent<Button>().onClick.AddListener(OnClickSkillDestroy);
		OnClickPlay();
	}

	void OnClickPlay()
	{
		foreach(sortGameSkills sk in skills)
		{
			sk.skillCount = 3;
		}

	}

	void OnClickSkillFreeze()
	{
		if (skills[0].skillCount > 0)
		{
			skills[0].ActiveSkill();
			skills[0].skillCount -= 1;
			buttons[0].GetComponent<Image>().sprite = activeSprites[0];
			skillCountText[0].text = skills[0].skillCount.ToString();
		}
		else
		{
			//open panel
			buttons[0].GetComponent<Image>().sprite = buySprites[0];
		}
	}

	void OnClickSkillDestroy()
	{
		if (skills[1].skillCount > 0)
		{
			skills[1].ActiveSkill();
			skills[1].skillCount -= 1;
			buttons[1].GetComponent<Image>().sprite = activeSprites[1];
			skillCountText[1].text = skills[1].skillCount.ToString();
		}
		else
		{
			//open panel
			buttons[1].GetComponent<Image>().sprite = buySprites[1];
		}
	}
}
