using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameFruitManager : Patterns.Singleton<MiniGameFruitManager>
{
	[SerializeField]
	GameObject startBoomPosition;
	[SerializeField]
	GameObject boom;
	public float gravityChangeSpeed = 1.0f;
	private bool IsBoomPowerActive = false;
	private bool IsUpgradePowerActive = false;

	public Action<bool> TriggerSelectAnimation;
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
	}
	public bool isBoomPowerActive
	{
		get => IsBoomPowerActive; 
		set
		{
			IsBoomPowerActive = value;
			isPowerActive = value;
			TriggerSelectAnimation?.Invoke(value);
		}
	}
	public bool isUpgradePowerActive
	{
		get => IsUpgradePowerActive;
		set
		{
			IsUpgradePowerActive = value;
			isPowerActive = value;
			TriggerSelectAnimation?.Invoke(value);
		}
	}
	public bool isPowerActive { private set; get; } = false;
	public async UniTask TriggerBoomPowerAniamtion(Vector3 targerPosition)
	{
		Vector3 pointA = Camera.main.WorldToScreenPoint(startBoomPosition.transform.position);
		Vector3 pointB = Camera.main.WorldToScreenPoint(targerPosition);
		float duration = 0.5f;
		float timeElapsed = 0f;
		boom.SetActive(true);
		Vector3 initialScale = new(2f, 2f, 2f);
		while(timeElapsed < duration)
		{

			timeElapsed += Time.deltaTime;

			float t = timeElapsed / duration;

			
			float radiusX = pointB.x - pointA.x; 
			float radiusY = Mathf.Abs(pointB.y - pointA.y);

			
			float angle = Mathf.Lerp(0, Mathf.PI / 2, t);

			
			float x = pointA.x + radiusX * Mathf.Sin(angle); 
			float y = pointA.y + radiusY * Mathf.Sin(angle); 

			Vector3 screenPosition = new Vector3(x, y, pointA.z);

			
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

			
			boom.transform.position = worldPosition;
			boom.transform.localScale = Vector3.Lerp(initialScale, new Vector3(0.76f, 0.76f, 0.76f), t);

			await UniTask.Yield(PlayerLoopTiming.Update);
		}
		boom.SetActive(false);
	}
	public async UniTask TriggerFreeGravityPower()
	{
		float duration = 2f;
		float timeElapsed = 0f;
		isPowerActive = true;
		while(timeElapsed < duration)
		{
			timeElapsed += Time.deltaTime;

			Vector3 acceleration = Input.acceleration;

			Vector3 newGravity = new Vector3(acceleration.x, -acceleration.y, acceleration.z);

			Physics.gravity = Vector3.Lerp(Physics.gravity, newGravity * 9.8f, gravityChangeSpeed * Time.deltaTime);

			await UniTask.Yield(PlayerLoopTiming.Update);
		}
		Physics.gravity = new Vector3(0f, -9.8f, 0f);
		isPowerActive = false;
	}


}
