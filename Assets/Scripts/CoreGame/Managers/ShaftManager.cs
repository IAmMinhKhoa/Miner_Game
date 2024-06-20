using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftManager : Patterns.Singleton<ShaftManager>
{
    [Header("Prefab")]
    [SerializeField] private Shaft shaftPrefab;
    [SerializeField] private float shaftYOffset = 1.716f;
    [SerializeField] private double currentCost = 0;

    [Header("Basement")]
    [SerializeField] public List<Shaft> Shafts = new();

    [Header("Shaft")]
    [SerializeField] private Vector3 firstShaftPosition = new(0.656000018f,-0.0390000008f,0);
    [SerializeField] int maxShaftCount = 30;

    public double CurrentCost => currentCost;
    private void Start()
    {
        Shaft firstShaft = Instantiate(shaftPrefab, firstShaftPosition, Quaternion.identity);
        Shafts.Add(firstShaft);
        firstShaft.ShaftIndex = Shafts.Count - 1;
        firstShaft.gameObject.GetComponent<ShaftUI>().NewShaftCostText.text = Currency.DisplayCurrency(CalculateNextShaftCost());
    }

    public void AddShaft()
    {
        Transform lastShaft = Shafts[^1].transform;
        Shaft newShaft = Instantiate(shaftPrefab, lastShaft.position, Quaternion.identity);
        newShaft.transform.localPosition += new Vector3(0, -shaftYOffset, 0);
        newShaft.IndexBoost = BaseShaftIndexScale();

        Shafts.Add(newShaft);
        newShaft.ShaftIndex = Shafts.Count - 1;
        newShaft.gameObject.GetComponent<ShaftUI>().NewShaftCostText.text = Currency.DisplayCurrency(CalculateNextShaftCost());
    }

    private double CalculateNextShaftCost()
    {
        int shaftCount = Shafts.Count;
        currentCost = shaftCount switch
        {
            0 => 10,
            1 => 2600,
            2 => 78000,
            _ => Mathf.Pow(20, shaftCount - 2) * 78000,
        };

        return currentCost;
    }

    private double BaseShaftIndexScale()
    {
        double scale = 1;

        int shaftCount = Shafts.Count;

        scale = shaftCount switch
        {
            0 => 1,
            1 => 50,
            _ => Mathf.Pow(10, shaftCount - 1) * 50,
        };
        return scale;
    }
}
