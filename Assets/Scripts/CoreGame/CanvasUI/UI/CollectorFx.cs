 using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NOOD.Sound;
using UnityEngine;

public class CollectorFx : MonoBehaviour
{
	public GameObject coinPrefab;
	public RectTransform startPosition;
	public RectTransform endPosition;
	private Transform parentFx;

	#region parameter animation
	#endregion
	public float duration = 0.8f;
	public float radius = 1.5f;
	public float range = 1f;

	public void SpawnAndMoveCoin(int quantity = 5, Transform _parentFx = null, float scale = 1f)
	{
		StartCoroutine(IESpawnAndMoveCoin(quantity, _parentFx, scale));
	}

	private IEnumerator IESpawnAndMoveCoin(int quantity = 5, Transform _parentFx = null, float scale = 1f)
	{
		List<GameObject> coinsGO = new List<GameObject>();
		for (int i = 0; i < quantity; i++)
		{
			GameObject coin = Instantiate(coinPrefab, endPosition);
			RectTransform coinRect = coin.GetComponent<RectTransform>();
			coin.transform.localScale = new Vector3(scale, scale, scale);
			coin.transform.parent = _parentFx;
			// Set vị trí ban đầu
			coinRect.anchoredPosition =startPosition.anchoredPosition;

			// Random vị trí trong vòng tròn bán kính 100

			Vector2 randomDir = Random.insideUnitCircle.normalized * Random.Range(range, radius);

			Vector2 targetPos = startPosition.anchoredPosition + randomDir;

			// Animate tới vị trí xung quanh với easing OutBack
			coinRect.DOAnchorPos(targetPos, duration)
				.SetEase(Ease.OutBack)
				.SetDelay(i * 0.01f); // delay nhẹ cho hiệu ứng tỏa

			coinsGO.Add(coin);
			yield return new WaitForSeconds(0.01f);

		}

		//
		foreach (var coin in coinsGO)
		{
			RectTransform coinRect = coin.GetComponent<RectTransform>();
			StartCoroutine(MoveCoin(coinRect, endPosition, 0.6f));
			yield return new WaitForSeconds(0.08f);
		}

	}

	private IEnumerator MoveCoin(RectTransform coin, RectTransform target, float duration)
	{
		// Chuyển vị trí thế giới của target thành vị trí local so với cha của coin
		Vector2 localTargetPos = coin.parent.InverseTransformPoint(target.position);

		coin.DOAnchorPos(localTargetPos, duration)
			.SetEase(Ease.InOutCubic)
			.OnComplete(() =>
			{
				SoundManager.PlaySound(SoundEnum.coin);
				Destroy(coin.gameObject, 0.1f);
			});


		yield return new WaitForSeconds(0);
	}
}
