using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CakeConfig", menuName = "CakeConfig")]
public class CakeConfig : ScriptableObject
{
	public float Value=1f;
	public float ProductPerSecond=1f;
}
