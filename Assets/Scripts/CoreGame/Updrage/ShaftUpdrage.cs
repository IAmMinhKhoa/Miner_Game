using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftUpgrade : BaseUpgrade
{
    private Shaft shaft;

    private void Start()
    {
        shaft = GetComponent<Shaft>();
        CurrentLevel = 1;
    }
    protected override void RunUpgrade()
    {
        float nextScale = GetNextUpgradeScale(CurrentLevel);
        shaft.BoostScale *= 1 + nextScale;

        if (CurrentLevel == 10 || CurrentLevel == 50 || CurrentLevel == 100 || CurrentLevel == 200 || CurrentLevel == 400)
        {
            shaft.CreateBrewer();
        }
    }
    private float GetNextUpgradeScale(int CurrentLevel)
    {
        if (CheckSpecialUpgrade())
        {
            switch(CurrentLevel)
            {
                case 10:
                case 25:
                    return 1.5f;
                case 50:
                case 100:
                case 200:
                case 400:
                    return 2.5f;
            }
        }
        if (CurrentLevel > 1 && CurrentLevel < 10)
        {
            return 0.098f;
        }
        else if (CurrentLevel < 25)
        {
            return 0.088f;
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

    private bool CheckSpecialUpgrade()
    {
        return CurrentLevel == 10 || CurrentLevel == 25 || CurrentLevel == 50 || CurrentLevel == 100 || CurrentLevel == 200 || CurrentLevel == 400;
    }
}
