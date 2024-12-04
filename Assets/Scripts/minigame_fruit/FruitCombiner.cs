using Cysharp.Threading.Tasks;
using DG.Tweening;
using NOOD.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FruitCombiner : MonoBehaviour
{
	public GameObject combineFX;
	public ListFruit FruitList;
	public TMP_Text combo_lb;
	public GameObject boomPowerAnimation;
	private bool isPowerActivating = false;
	private async void OnMouseDown()
	{
		if (isPowerActivating) return;
		if(MiniGameFruitManager.Instance.isBoomPowerActive)
		{
			isPowerActivating = true;
			SoundManager.PlaySound(SoundEnum.popMerge);
			await MiniGameFruitManager.Instance.TriggerBoomPowerAniamtion(transform.position);
			MiniGameFruitManager.Instance.isBoomPowerActive = false;
			Vector3 pos = gameObject.transform.position;
			Destroy(gameObject);

			Color newColor = GetComponent<FruitInfo>().mergeColor;
			GameObject FX = Instantiate(combineFX, pos, Quaternion.identity);
			FX.GetComponent<ParticleSystem>().startColor = newColor;
			ParticleSystem[] particleSystems = FX.GetComponentsInChildren<ParticleSystem>();
			foreach (var ps in particleSystems)
			{
				var main = ps.main;
				main.startColor = newColor; // Áp dụng màu cho tất cả các particle system
			}
			Destroy(FX, 3f); // Hủy đối tượng FX sau 3 giây
		}
		if(MiniGameFruitManager.Instance.isUpgradePowerActive)
		{
			Destroy(gameObject);
			isPowerActivating = true;
			SoundManager.PlaySound(SoundEnum.popMerge);
			int upgradeIndex = GetComponent<FruitInfo>().index;
			GameObject newFruit = Instantiate(FruitList.list[upgradeIndex], transform.position, Quaternion.identity, gameObject.transform.parent);
			newFruit.transform.DOScale(0.001f, 0f);
			newFruit.transform.DOScale(0.2f, 0.5f);
			Debug.Log(MiniGameFruitManager.Instance.isPowerActive);
			await UniTask.Delay(200);
			MiniGameFruitManager.Instance.isUpgradePowerActive = false;
			Debug.Log(MiniGameFruitManager.Instance.isPowerActive);
		}
		isPowerActivating = false;
	}
	private void Awake()
	{
		MiniGameFruitManager.Instance.TriggerSelectAnimation += TriggerBoomPowerAnimation;
	}
	private void OnDestroy()
	{
		MiniGameFruitManager.Instance.TriggerSelectAnimation -= TriggerBoomPowerAnimation;
	}

	private void TriggerBoomPowerAnimation(bool isActive)
	{
		if (gameObject.tag == "holdingfruit") return;
		boomPowerAnimation.gameObject.SetActive(isActive);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<FruitInfo>() != null && !MiniGameFruitManager.Instance.isPowerActive)
		{
			int otherIndex = collision.gameObject.GetComponent<FruitInfo>().index;

			if (GetComponent<FruitInfo>().index == otherIndex && otherIndex != FruitList.list.Count)
			{
				if (gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
				{
					Destroy(gameObject);
					Destroy(collision.gameObject);


					SoundManager.PlaySound(SoundEnum.popMerge);

					Vector3 hitpoint = (transform.position + collision.gameObject.transform.position) / 2;
					GameObject newFruit = Instantiate(FruitList.list[otherIndex], hitpoint, Quaternion.identity, gameObject.transform.parent);
					hitpoint.y += 1.1f;
					newFruit.transform.DOScale(0.001f, 0f);
					newFruit.transform.DOScale(0.2f, 0.5f);
					Color newColor = GetComponent<FruitInfo>().mergeColor; // Lấy màu từ mergeColor
					if (newColor != null) // Kiểm tra màu sắc có hợp lệ không
					{
						GameObject FX = Instantiate(combineFX, hitpoint, Quaternion.identity);
						FX.GetComponent<ParticleSystem>().startColor = newColor;
						ParticleSystem[] particleSystems = FX.GetComponentsInChildren<ParticleSystem>();
						foreach (var ps in particleSystems)
						{
							var main = ps.main;
							main.startColor = newColor; // Áp dụng màu cho tất cả các particle system
						}
						Destroy(FX, 3f); // Hủy đối tượng FX sau 3 giây
					}
					ComboMergeManager.Instance.comboCount++;
					ComboMergeManager.Instance.comboTime = ComboMergeManager.Instance.maxComboTime;
					//
					if(ComboMergeManager.Instance.comboCount>1)
					{
						TMP_Text newFruitComboLb = newFruit.GetComponent<FruitCombiner>().combo_lb;
						ComboUI.Instance.transform.position = newFruitComboLb.transform.position;
						ComboUI.Instance.UpdateComboUI(ComboMergeManager.Instance.comboCount);
						ComboUI.Instance.ShowComboUI();
						ComboUI.Instance.Hide(ComboUI.Instance.gameObject, 1f);
					}	

				}
				int score = otherIndex;
				float multipScore = ComboMergeManager.Instance.comboCount;
				Debug.Log("score game merge animal:" + score + "/" + multipScore);
				GameObject.FindWithTag("manager").GetComponent<FruitGameManager>().UpdateScore(score * multipScore);
			}
		}


	}

}
