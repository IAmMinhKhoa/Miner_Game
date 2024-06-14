using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CouterUpdrage : BaseUpgrade
{
    private Couter couter;

    private void Start()
    {
        couter = GetComponent<Couter>();
        CurrentLevel = 1;
    }
    protected override void RunUpgrade()
    {
        float nextScale = GeNextUpgradeScale(CurrentLevel);
        couter.BoostScale *= 1 + nextScale;

        if (CurrentLevel == 10 || CurrentLevel == 50 || CurrentLevel == 100 || CurrentLevel == 200 || CurrentLevel == 400)
        {
            couter.CreateTransporter();
        }
    }
    private float GeNextUpgradeScale(int CurrentLevel)
    {
        if (CurrentLevel > 1 && CurrentLevel < 10)
        {
            return 0.098f;
        }
        else if (CurrentLevel == 10 || CurrentLevel == 25)
        {
            return 1.2f;
        }
        else if (CurrentLevel < 25)
        {
            return 0.088f;
        }
        else if (CurrentLevel == 50 || CurrentLevel == 100 || CurrentLevel == 200 || CurrentLevel == 400)
        {
            return 2.5f;
        }
        else if (CurrentLevel <= 800)
        {
            return 0.078f;
        }
        else
        {
            return 0;
        }
    }
}
