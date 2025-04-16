using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SlotReel : MonoBehaviour
{
	public GameObject symbolPrefab;
	public Sprite[] symbolSprites;
	public int symbolCount = 8;
	public float symbolHeight = 150f;
	public float gap = 20f;

	public int? forcedWinSpriteIndex = null;

	private float effectiveSpacing => symbolHeight + gap;
	private float loopHeight => effectiveSpacing * symbolCount;

	private List<SymbolSlot> symbols = new();

	public void Init()
	{
		float totalHeight = loopHeight;
		int winIndex = symbolCount / 2;

		// Xóa symbol cũ (nếu có)
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		symbols.Clear();

		for (int i = 0; i < symbolCount; i++)
		{
			GameObject symbol = Instantiate(symbolPrefab, transform);
			float y = i * effectiveSpacing - (totalHeight - effectiveSpacing) / 2f;
			symbol.transform.localPosition = new Vector3(0, y, 0);

			Image img = symbol.GetComponent<Image>();

			if (i == winIndex && forcedWinSpriteIndex != null)
			{
				img.sprite = symbolSprites[forcedWinSpriteIndex.Value];
			}
			else
			{
				img.sprite = symbolSprites[Random.Range(0, symbolSprites.Length)];
			}

			var logic = symbol.AddComponent<SymbolSlot>();
			logic.loopHeight = totalHeight;

			symbols.Add(logic);
		}
	}

	public void SetSpeed(float speed)
	{
		foreach (var s in symbols)
		{
			s.SetSpeed(speed);
		}
	}

	public void SnapToNearest()
	{
		float minDistance = float.MaxValue;
		SymbolSlot target = null;

		foreach (var s in symbols)
		{
			float dist = Mathf.Abs(s.transform.localPosition.y);
			if (dist < minDistance)
			{
				minDistance = dist;
				target = s;
			}
		}

		if (target != null)
		{
			float offset = target.transform.localPosition.y;
			foreach (var s in symbols)
			{
				s.transform.localPosition -= new Vector3(0, offset, 0);
			}
		}
	}

	public Sprite GetCenterSymbol()
	{
		float minDistance = float.MaxValue;
		Sprite centerSprite = null;

		foreach (var s in symbols)
		{
			float dist = Mathf.Abs(s.transform.localPosition.y);
			if (dist < minDistance)
			{
				minDistance = dist;
				centerSprite = s.GetComponent<Image>().sprite;
			}
		}

		return centerSprite;
	}
}
