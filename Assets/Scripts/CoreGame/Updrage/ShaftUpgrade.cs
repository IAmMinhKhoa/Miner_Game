using System.Collections;
using System.Collections.Generic;
using NOOD.SerializableDictionary;
using UnityEngine;

public class ShaftUpgrade : BaseUpgrade
{
	private Shaft shaft;
	[SerializeField] private SerializableDictionary<int, int> evolutionLevelDic = new SerializableDictionary<int, int>();
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
	};
	protected override float CostsBoost
	{
		get
		{
			return shaft.CostsBoost;
		}
	}
	private void Awake()
	{
		shaft = GetComponent<Shaft>();
	}
	protected override void RunUpgrade()
	{
		float nextScaleCakeValue = GetNextUpgradeCakeValue(CurrentLevel);
		float nextScaleBakingTime= GetNextUpgradeBakingTime(CurrentLevel);
		shaft.ScaleCakeValue *= 1 + nextScaleCakeValue;
		shaft.ScaleBakingTime *= 1 + nextScaleBakingTime;
		if (milestoneLevels.TryGetValue(CurrentLevel, out int superMoney))
		{
			SuperMoneyManager.Instance.AddMoney(superMoney);
		}
		switch (CurrentLevel)
		{
			case 10:
			case 50:
			case 100:
			case 200:
			case 400:
				//shaft.CreateBrewer();
				break;
		}
		if(TutorialManager.Instance.isTuroialing && TutorialManager.Instance.currentState == 3)
		{
			TutorialManager.Instance.TutorialStateMachine.TriggerClickableStates(3);
		}
		shaft.OnUpgrade?.Invoke(CurrentLevel);
	}
	private float GetNextUpgradeCakeValue(int CurrentLevel)
	{
		return CurrentLevel switch
		{
			< 2 => 0,
			10 or 25 => 0.48f,
			50 or 100 or 200 or 400 => 0.87f,
			_ => CurrentLevel switch
			{
				< 10 => 0.0479f,
				< 25 => 0.0431f,
				<= 800 => 0.078f,
				_ => 0
			}
		};
	}
	private float GetNextUpgradeBakingTime(int CurrentLevel)
	{
		return CurrentLevel switch
		{
			< 2 => 0,
			10 or 25 => 0.3243f,
			50 or 100 or 200 or 400 => 0.6516f,
			_ => CurrentLevel switch
			{
				< 10 => 0.0467f,
				< 25 => 0.0413f,
				<= 800 => 0.0369f,
				_ => 0
			}
		};
	}
	public override float GetNextUpgradeCostScale(int level)
	{

		var value = shaft.shaftIndex switch
		{
			0 => level switch
			{
				< 10 => 0.3f,
				< 800 => 0.15f,
				_ => 0f
			},
			_ => level switch
			{
				< 800 => 0.15f,
				_ => 0f
			}
		};

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
		shaft.OnUpgrade?.Invoke(level);
	}

	public override int GetNumberWorkerAtLevel(int level)
	{
		int initWorker = 1;
		return level switch
		{
			>= 400 => initWorker + 5,
			>= 200 => initWorker + 4,
			>= 100 => initWorker + 3,
			>= 50 => initWorker + 2,
			>= 10 => initWorker + 1,
			_ => initWorker
		};
	}

	public override double GetProductionCakeScale(int amoutOfNextLevel)
	{
		if (amoutOfNextLevel <= 0)
		{
			return 1d;
		}

		double scale = 1.00;
		for (int i = 1; i <= amoutOfNextLevel; i++)
		{
			scale *= 1 + GetNextUpgradeCakeValue(CurrentLevel + i);
		}

		return scale;
	}

	public override double GetProductionBakingTime(int amoutOfNextLevel)
	{
		if (amoutOfNextLevel <= 0)
		{
			return 1d;
		}

		double scale = 1.00;
		for (int i = 1; i <= amoutOfNextLevel; i++)
		{
			scale *= 1 + GetNextUpgradeBakingTime(CurrentLevel + i);
		}

		return scale;
	}
}
/*
 * 1
 *
 */
