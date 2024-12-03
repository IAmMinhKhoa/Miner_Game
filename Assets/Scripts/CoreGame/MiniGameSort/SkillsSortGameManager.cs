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
	public List<GameObject> panelBuySkills;

	void Start()
	{
		buttons[0].GetComponent<Button>().onClick.AddListener(OnClickSkillFreeze);
		buttons[1].GetComponent<Button>().onClick.AddListener(OnClickSkillDestroy);
	}

	public void OnClickPlay()
	{
		foreach(sortGameSkills sk in skills)
		{
			sk.skillCount = 3;
		}
		UpdateSkillCount(0);
		UpdateSkillCount(1);
	}

	void OnClickSkillFreeze()
	{
		if (skills[0].skillCount == 0)
		{
			//open panel
			panelBuySkills[0].SetActive(true);
		}
		if (skills[0].skillCount > 0 && !skills[0].isUsing)
		{
			skills[0].ActiveSkill();
			skills[0].skillCount -= 1;
			
		}
		UpdateSkillCount(0);
	}

	void OnClickSkillDestroy()
	{
		if (skills[1].skillCount == 0)
		{
			//open panel
			panelBuySkills[1].SetActive(true);
		}
		if (skills[1].skillCount > 0 && !skills[1].isUsing)
		{
			skills[1].ActiveSkill();
			skills[1].skillCount -= 1;
			skillCountText[1].text = skills[1].skillCount.ToString();
		}
		UpdateSkillCount(1);
	}

	void UpdateSkillCount(int id)
	{
		if (skills[id].skillCount > 0) buttons[id].GetComponent<Image>().sprite = activeSprites[id];
		else buttons[id].GetComponent<Image>().sprite = buySprites[id];

		skillCountText[id].text = skills[id].skillCount.ToString();
	}
}
