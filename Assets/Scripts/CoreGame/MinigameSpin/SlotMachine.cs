using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
	public SlotReel[] reels;
	public GameObject symbolPrefab;
	public Sprite[] symbolSprites;
	public Button spinButton;

	public float spinSpeed = 300f;
	public float stopDelay = 0.5f;
	private int stoppedCount = 0;

	void Start()
	{
		spinButton.onClick.AddListener(StartSpin);
		InitReels();
	}

	void InitReels(bool? forceWin = null)
	{
		int winSpriteIndex = Random.Range(0, symbolSprites.Length);
		bool isWin = forceWin ?? (Random.value < 0.5f);
		for (int i = 0; i < reels.Length; i++)
		{
			reels[i].symbolPrefab = symbolPrefab;
			reels[i].symbolSprites = symbolSprites;
			reels[i].forcedWinSpriteIndex = isWin ? winSpriteIndex : (int?)null;

			reels[i].Init();
			reels[i].SetSpeed(0f);
		}
	}

	public void StartSpin()
	{
		stoppedCount = 0;


		foreach (var reel in reels)
		{
			reel.SetSpeed(spinSpeed);
		}

		Invoke(nameof(StopSpin), 2f);
	}

	void StopSpin()
	{
		for (int i = 0; i < reels.Length; i++)
		{
			float delay = i * stopDelay;
			StartCoroutine(SlowDown(reels[i], delay));
		}
	}

	System.Collections.IEnumerator SlowDown(SlotReel reel, float delay)
	{
		yield return new WaitForSeconds(delay);

		float speed = spinSpeed;
		while (speed > 30f)
		{
			speed -= 10f;
			reel.SetSpeed(speed);
			yield return new WaitForSeconds(0.05f);
		}

		reel.SetSpeed(0f);
		reel.SnapToNearest();

		stoppedCount++;
		if (stoppedCount == reels.Length)
		{
			CheckResult();
		}
	}

	void CheckResult()
	{
		Sprite a = reels[0].GetCenterSymbol();
		Sprite b = reels[1].GetCenterSymbol();
		Sprite c = reels[2].GetCenterSymbol();

		if (a == b && b == c)
		{
			Debug.Log("win");
		}
		else
		{
			Debug.Log("thua");
		}
	}
}
