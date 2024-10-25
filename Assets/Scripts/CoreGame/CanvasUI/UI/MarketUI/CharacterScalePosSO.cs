using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/CharacterScaleAndPosUI")]
public class CharacterScalePosSO : ScriptableObject
{
	[SerializeField]
	List<CharScaleAndPos> listCharScaleAndPos;
	public List<CharScaleAndPos> ListCharScaleAndPos => listCharScaleAndPos;
}
[Serializable]
public struct CharScaleAndPos
{
	public string ID;
	public Vector3 scale;
	public Vector3 pos;
}


