using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterConfig", menuName = "BaseConfig")]
public class BaseConfig : ScriptableObject
{
    [SerializeField] private float workingTime = 4f;
    [SerializeField] private float moveTime = 1f;
    [SerializeField] private double productPerSecond = 5f;

    public float WorkingTime
    {
        get { return workingTime; }
        private set { workingTime = value; }
    }

    public float MoveTime
    {
        get { return moveTime; }
        set { moveTime = value; }
    }

    public double ProductPerSecond
    {
        get { return productPerSecond; }
        set { productPerSecond = value; }
    }
}
