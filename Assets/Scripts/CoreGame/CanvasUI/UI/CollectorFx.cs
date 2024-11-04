using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

		for (int i = 0; i < quantity; i++)
		{
			// Instantiate each coin at the start position
			GameObject coin = Instantiate(clonePrefab, startPosition.position, Quaternion.identity);

			// Set the parent transform if specified
			if (parentFx != null)
			{
				coin.transform.SetParent(parentFx);
			}

			// Set the scale of the coin
			coin.transform.localScale = Vector3.one * scale;

			// Calculate the delay for the staggered effect
			float delay = i * 0.1f;

			// Set a random scatter position within a circle
			Vector2 randomPoint = Random.insideUnitCircle * 1;
			Vector3 scatterPosition = new Vector3(
				startPosition.position.x + randomPoint.x,
				startPosition.position.y + randomPoint.y,
				startPosition.position.z
			);

			// Animate the coin to scatter first
			coin.transform.DOMove(scatterPosition, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
			{
				// Then move the coin to the target position
				coin.transform.DOMove(endPosition.position, duration).SetEase(Ease.InQuad).OnComplete(() =>
				{
					// Scale down the coin to 0 before destroying it
					coin.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
					{
						// Destroy the coin after it reaches the target and scales down
						Destroy(coin);
					});
				});
			});

		}
	}
}
