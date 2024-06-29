using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouterUpdrage : BaseUpgrade
{
    private Couter couter;

    private void Start()
    {
        couter = GetComponent<Couter>();
    }
    protected override void RunUpgrade()
    {
        float nextScale = GetNextExtractionSpeedScale(CurrentLevel);
        couter.BoostScale *= 1 + nextScale;

        if (IsNeedCreateTransporter(CurrentLevel))
        {
            couter.CreateTransporter();
        }
    }

    private float GetNextExtractionSpeedScale(int level)
    {
        return level switch
        {
            < 2 => 0,
            25 => 1.6f,
            50 or 100 or 200 or 400 => 1.2f,
            _ => level switch
            {
                < 25 => 0.3f,
                <= 2400 => 0.1f,
                _ => 0
            }
        };
    }

    private bool IsNeedCreateTransporter(int level)
    {
        return level == 10 || level == 100 || level == 300 || level == 500;
    }
    protected override float GetNextUpgradeCostScale()
    {
        return CurrentLevel switch
        {
            <= 2400 => 0.2f,
            _ => 0f
        };
    }

    public void InitValue(int level)
    {
        Init(1041.67f, level);
    }
}
