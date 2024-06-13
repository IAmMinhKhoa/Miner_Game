using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftUpdrage : BaseUpdrage
{
    private Shaft shaft;

    private void Start()
    {
        shaft = GetComponent<Shaft>();
        CurrentLevel = 1;
    }
    protected override void RunUpdrage()
    {
        float nextScale = GeNextUpdrageScale(CurrentLevel);
        shaft.BoostScale *= 1 + nextScale;

        if (CurrentLevel == 10 || CurrentLevel == 50 || CurrentLevel == 100 || CurrentLevel == 200 || CurrentLevel == 400)
        {
            shaft.CreateBrewer();
        }
    }
    private float GeNextUpdrageScale(int CurrentLevel)
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
