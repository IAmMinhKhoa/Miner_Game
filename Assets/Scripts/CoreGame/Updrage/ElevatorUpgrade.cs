using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class ElevatorUpgrade : BaseUpgrade
{
    private ElevatorSystem elevatorSystem;
	private Dictionary<int, int> milestoneLevels = new Dictionary<int, int>
	{
		{ 10, 4 },
		{ 25, 4 },
		{ 50, 4 },
		{ 100, 4 },
		{ 200, 4 },
		{ 300, 4 },
		{ 400, 4 },
		{ 500, 4 },
		{ 600, 4 },
		{ 700, 4 },
		{ 800, 4 },
		{ 900, 4 },
		{ 1000, 4 },
		{ 1100, 4 },
		{ 1200, 4 },
		{ 1300, 4 },
		{ 1400, 4 },
		{ 1500, 4 },
		{ 1600, 4 },
		{ 1700, 4 },
		{ 1800, 4 },
		{ 1900, 4 },
		{ 2000, 4 },
		{ 2100, 4 },
		{ 2200, 4 },
		{ 2300, 4 },
		{ 2400, 4 },
	};
	protected override float CostsBoost
    {
        get => elevatorSystem.CostsBoost;
    }

    private void Awake()
    {
        elevatorSystem = GetComponent<ElevatorSystem>();
       // Init(1041.67f, 1);
    }

    protected override void RunUpgrade()
    {
        double nextScale = GetMoveTimeScale(CurrentLevel);
		if (milestoneLevels.TryGetValue(CurrentLevel, out int superMoney))
		{
			SuperMoneyManager.Instance.AddMoney(superMoney);
		}

		elevatorSystem.MoveTimeScale *= nextScale;
        elevatorSystem.LoadSpeedScale *= 1 + GetNextLoadingSpeedScale(CurrentLevel);
		if (TutorialManager.Instance.isTuroialing && TutorialManager.Instance.currentState == 3)
		{
			TutorialManager.Instance.TutorialStateMachine.TriggerClickableStates(3);
		}
	}

    private double GetMoveTimeScale(int level)
    {
        return 1f - 15 / (0.15f * (level - 1) + 5) / 100;
    }

    private float GetNextLoadingSpeedScale(int level)
    {
        return level switch
        {
            < 2 => 0,
            25 => 1.6f,
            50 or 100 or 200 or 400 => 1.2f,
            _ => level switch
            {
                < 25 => 0.32f,
                <= 2400 => 0.11f,
                _ => 0
            }
        };
    }
    public override float GetNextUpgradeCostScale(int level)
    {
        return level switch
        {
            < 2400 => 0.2f,
            _ => 0f
        };
    }

    public void InitValue(int level)
    {
        Init(1041.67f, level);
    }

    public override double GetProductionScale(int amoutOfNextLevel)
    {
        if (amoutOfNextLevel <= 0)
        {
            return 1d;
        }

        double scale = 1.00;
        for (int i = 1; i <= amoutOfNextLevel; i++)
        {
            scale *= 1 + GetNextLoadingSpeedScale(CurrentLevel + i);
        }

        return scale;
    }

    public override double GetSpeedScale(int amoutOfNextLevel)
    {
        double scale = 1d;

        for (int i = 1; i <= amoutOfNextLevel; i++)
        {
            scale *= GetMoveTimeScale(CurrentLevel + i);
        }

        return scale;
    }
}
