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
        float nextScale = GetNextExtractionSpeedScale(CurrentLevel);
        couter.BoostScale *= 1 + nextScale;

        if (IsNeedCreateTransporter(CurrentLevel))
        {
            couter.CreateTransporter();
        }
    }

    private float GetNextExtractionSpeedScale(int level)
    {
        switch (level)
        {
            case 25:
                return 1.6f;
            case 50:
            case 100:
            case 200:
            case 400:
                return 1.2f;
            default:
                if (level > 1 && level < 25)
                {
                    return 0.3f;
                }
                else if (level <= 2400)
                {
                    return 0.1f;
                }

                return 0;
        }
    }

    private bool IsNeedCreateTransporter(int level)
    {
        return level == 10 || level == 100 || level == 300 || level == 500;
    }
}
