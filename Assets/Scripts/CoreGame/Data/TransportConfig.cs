using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Transport", menuName = "TransportConfig")]
public class TransportConfig : ScriptableObject
{
	public float Value=1f;
	public float ProductPerSecond=1f;
}
