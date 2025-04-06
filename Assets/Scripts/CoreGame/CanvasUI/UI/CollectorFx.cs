 using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NOOD.Sound;
using UnityEngine;

public class CollectorFx : MonoBehaviour
{
	public GameObject clonePrefab;
	public RectTransform startPosition;
	public RectTransform endPosition;
	private Transform parentFx;

	#region parameter animation
	#endregion
	public float duration = 1f;

	/// <summary>
	/// Spawns and animates a specified number of coins from the start position to the end position.
	/// </summary>
	/// <param name="quantity">Number of coins to spawn and animate.</param>
	/// <param name="_parentFx">Optional parent transform to assign to the coins.</param>
	/// <param name="scale">Scale to apply to each coin.</param>
	public void SpawnAndMoveCoin(int quantity = 5, Transform _parentFx = null, float scale = 1f)
	{
		// Set the parent transform if provided
		if (_parentFx != null)
		{
			parentFx = _parentFx;
		}

		List<GameObject> _objCoins = new List<GameObject>();

		// Vòng lặp đầu tiên để tạo và scatter các đồng xu
		for (int i = 0; i < quantity; i++)
		{
			// Instantiate mỗi đồng xu tại vị trí ban đầu
			GameObject coin = Instantiate(clonePrefab, startPosition.position, Quaternion.identity);

			// Set parent transform nếu được chỉ định
			if (parentFx != null)
			{
				coin.transform.SetParent(parentFx);
			}

			// Đặt scale của đồng xu
			coin.transform.localScale = Vector3.one * scale;

			// Thêm đồng xu vào danh sách
			_objCoins.Add(coin);

			// Tính toán vị trí scatter ngẫu nhiên trong vòng tròn
			Vector2 randomPoint = Random.insideUnitCircle * 1.3f;
			Vector3 scatterPosition = new Vector3(
				startPosition.position.x + randomPoint.x,
				startPosition.position.y + randomPoint.y,
				startPosition.position.z
			);

			// Animate đồng xu scatter ra vị trí ngẫu nhiên
			coin.transform.DOMove(scatterPosition, 0.5f).SetEase(Ease.OutQuad);
		}

		// Chia danh sách _objCoins thành hai danh sách con
		List<GameObject> coinsGroup1 = new List<GameObject>();
		List<GameObject> coinsGroup2 = new List<GameObject>();

		for (int i = 0; i < _objCoins.Count; i++)
		{
			if (i % 2 == 0)
			{
				coinsGroup1.Add(_objCoins[i]);
			}
			else
			{
				coinsGroup2.Add(_objCoins[i]);
			}
		}

		// Làm cho từng đồng xu trong nhóm 1 bay lên
		for (int i = 0; i < coinsGroup1.Count; i++)
		{
			float delay = i * 0.1f; // Độ trễ để từng đồng xu bay lên lần lượt
			GameObject coin = coinsGroup1[i]; // Tạo một biến coin cục bộ để tránh vấn đề tham chiếu

			coin.transform.DOMove(endPosition.position, duration)
				.SetEase(Ease.InQuad)
				.SetDelay(delay)
				.OnComplete(() =>
				{
					coin.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
					{
						SoundManager.PlaySound(SoundEnum.coin);
						Destroy(coin);
					});
				});
		}

		// Làm cho từng đồng xu trong nhóm 2 bay lên
		for (int i = 0; i < coinsGroup2.Count; i++)
		{
			float delay = i * 0.1f; // Độ trễ để từng đồng xu bay lên lần lượt
			GameObject coin = coinsGroup2[i]; // Tạo một biến coin cục bộ để tránh vấn đề tham chiếu

			coin.transform.DOMove(endPosition.position, duration)
				.SetEase(Ease.InQuad)
				.SetDelay(delay)
				.OnComplete(() =>
				{
					coin.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
					{
						Destroy(coin);
					});
				});
		}
	}
}
