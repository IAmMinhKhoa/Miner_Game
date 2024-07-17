using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "BaseConfig")]
public class BaseConfig : ScriptableObject
{
    public float WorkingTime = 4f;
    public float MoveTime = 1f;
    public double ProductPerSecond = 5f;
}
