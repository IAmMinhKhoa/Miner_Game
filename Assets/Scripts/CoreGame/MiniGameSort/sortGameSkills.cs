using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class sortGameSkills : MonoBehaviour
{
	public int skillCount;
	public bool isUsing = false;
	public abstract void ActiveSkill();
}
