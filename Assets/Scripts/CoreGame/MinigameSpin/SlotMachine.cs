using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
public enum symbolItem
{
	item1,
	item2, 
	item3
}
public class SlotMachine : MonoBehaviour
{
	public SlotReel[] reels;
	public GameObject symbolPrefab;
	public Sprite[] symbolSprites;
	public Button spinButton;
	public float spinSpeed = 3000f;
	public float stopDelay = 0.6f;

	private int stoppedCount = 0;
	private List<symbolItem> serverResult;
	private Dictionary<symbolItem, Sprite> itemToSprite;

	void Start()
	{
		spinButton.onClick.AddListener(StartSpin);
		InitMapping();
		InitReels();
	}

	void InitMapping()
	{
		if (symbolSprites.Length != System.Enum.GetValues(typeof(symbolItem)).Length)
		{
			
			return;
		}

		itemToSprite = new Dictionary<symbolItem, Sprite>
	{
		{ symbolItem.item1, symbolSprites[0] },
		{ symbolItem.item2, symbolSprites[1] },
		{ symbolItem.item3, symbolSprites[2] }
	};
	}

	void InitReels()
	{
		foreach (var reel in reels)
		{
			reel.symbolPrefab = symbolPrefab;
			reel.symbolSprites = symbolSprites;
			reel.forcedWinSpriteIndex = null;

			reel.Init();
			reel.SetSpeed(0f);
		}
	}

	public void StartSpin()
	{
		stoppedCount = 0;

		serverResult = new List<symbolItem>
	{
		symbolItem.item1,
		symbolItem.item1,
		symbolItem.item3
	};
		for (int i = 0; i < reels.Length; i++)
		{
			int spriteIndex = (int)serverResult[i];
			reels[i].forcedWinSpriteIndex = spriteIndex;
			

			foreach (Transform child in reels[i].transform)
				child.DOKill();
			reels[i].Init();
			reels[i].SetSpeed(spinSpeed);
		}

		Invoke(nameof(StopSpin), 2.5f);
	}


	void StopSpin()
	{
		const float baseDelay = 0.2f;
		for (int i = 0; i < reels.Length; i++)
		{
			float delay = baseDelay + i * stopDelay;

			
			if (i == 1) delay += 0.15f;

			SlowDown(reels[i], delay);
		}
	}




	void SlowDown(SlotReel reel, float delay)
	{
		DOVirtual.DelayedCall(delay, () =>
		{
			reel.SetSpeed(0f);
			reel.StopWithTween(0.5f);

			stoppedCount++;
			if (stoppedCount == reels.Length)
				DOVirtual.DelayedCall(0.6f, CheckResult);
		});
	}

	void CheckResult()
	{
		var results = new List<symbolItem>();

		foreach (var reel in reels)
		{
			Sprite sprite = reel.GetCenterSymbol();
			if (sprite == null)
			{
				return;
			}

			int index = symbolSprites.ToList().IndexOf(sprite);
			if (index >= 0 && index < symbolSprites.Length)
			{
				results.Add((symbolItem)index);
			}
			else
			{	
				return;
			}
		}

		if (results.Count != reels.Length)
		{
			return;
		}

		if (results.All(r => r == results[0]))
		{
			Debug.Log($"WIN: {results[0]}");
		}
		else
		{
			Debug.Log($"LOSE: {string.Join(", ", results)}");
		}
	}
}
