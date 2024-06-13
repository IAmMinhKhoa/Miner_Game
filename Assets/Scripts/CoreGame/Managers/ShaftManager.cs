using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftManager : Patterns.Singleton<ShaftManager>
{
    [Header("Prefab")]
    [SerializeField]
    private Shaft shaftPrefab;
    [SerializeField]
    private float shaftYOffset = 1.716f;
    [SerializeField]
    private double currentCost = 0;

    [Header("Basement")]
    [SerializeField]
    public List<Shaft> Shafts = new();

    [Header("Shaft")]
    [SerializeField]
    private Vector3 firstShaftPosition = new(0.656000018f,-0.0390000008f,0);

    public double CurrentCost => currentCost;

    private void Start()
    {
        //for demo
        Shaft firstShaft = Instantiate(shaftPrefab, firstShaftPosition, Quaternion.identity);
        Shafts.Add(firstShaft);
    }

    public void AddShaft()
    {
        Transform lastShaft = Shafts[^1].transform;
        Shaft newShaft = Instantiate(shaftPrefab, lastShaft.position, Quaternion.identity);
        newShaft.transform.localPosition += new Vector3(0, -shaftYOffset, 0);

        Shafts.Add(newShaft);
    }
}
