using System.Collections;
using System.Collections.Generic;
using NOOD.SerializableDictionary;
using UnityEngine;

public class ShaftUpgrade : BaseUpgrade
{
	private Shaft shaft;
	[SerializeField] private SerializableDictionary<int, int> evolutionLevelDic = new SerializableDictionary<int, int>();

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

		shaft.OnUpgrade?.Invoke(CurrentLevel);
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

	public override float GetNextUpgradeCostScale(int level)
	{

		var value = shaft.shaftIndex switch
		{
			0 => level switch
			{
				<= 10 => 0.3f,
				<= 800 => 0.15f,
				_ => 0f
			},
			_ => level switch
			{
				<= 800 => 0.15f,
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
}
