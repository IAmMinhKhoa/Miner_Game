using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftUpgrade : BaseUpgrade
{
    private Shaft shaft;
    private void Awake()
    {
        shaft = GetComponent<Shaft>();
    }
    protected override void RunUpgrade()
    {
        float nextScale = GetNextUpgradeScale(CurrentLevel);
        shaft.LevelBoost *= 1 + nextScale;

        switch (CurrentLevel)
        {
            case 10:
            case 50:
            case 100:
            case 200:
            case 400:
                shaft.CreateBrewer();
                break;
        }
    }
    private float GetNextUpgradeScale(int CurrentLevel)
    {
        return CurrentLevel switch
        {
            < 2 => 0,
            10 or 25 => 1.5f,
            50 or 100 or 200 or 400 => 2.5f,
            _ => CurrentLevel switch
            {
                < 10 => 0.098f,
                < 25 => 0.088f,
                <= 800 => 0.078f,
                _ => 0
            }
        };
    }

    protected override float GetNextUpgradeCostScale()
    {
        
        var value = shaft.shaftIndex switch
        {
            0 => CurrentLevel switch
            {
                <= 10 => 0.3f,
                <= 800 => 0.15f,
                _ => 0f
            },
            _ => CurrentLevel switch
            {
                <= 800 => 0.15f,
                _ => 0f
            }
        };
        Debug.Log(shaft.shaftIndex + " " + CurrentLevel + " " + value);
        return value;
    }

    public double GetInitialCost()
    {
        return initialCost;
    }

    public void SetInitialValue(int index, double initialCost, int level)
    {
        shaft.shaftIndex = index;
        Init(initialCost, level);
    }
}
