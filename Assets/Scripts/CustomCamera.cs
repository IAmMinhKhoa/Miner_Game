using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NOOD;
using TMPro;
using UnityEngine;

public class CustomCamera : Patterns.Singleton<CustomCamera>
{
	[SerializeField] private float minY, maxY;
	[SerializeField] private float _decelerateSpeed = 0.1f;
	[SerializeField] private float _maxSpeed = 10;

	[Header("Camera shake")]
	[SerializeField] private float _duration = 0.2f;
	[SerializeField] private float _magnitude = 0.1f;
	[SerializeField] GameObject minYpos;
	private Camera _camera;
	private Vector3 touchPos;
	private bool _isDragging;
	private float _currentSpeed;
	private Vector3 _oldPos;
	private bool _isOverShooting;
	private Vector3 _dir;
	public bool PreventScroll { set; get; } = false;
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();
		_camera = this.GetComponentInChildren<Camera>();
	}

	void Start()
	{
		_camera.orthographicSize = NoodyCustomCode.CalculateOrthoCamSize(_camera, 0).size;
		float screenHeight = Camera.main.pixelHeight;
		var mainCamera = Camera.main;
		//minY = screenHeight * minY / 1920;
		

		minY = minYpos.transform.position.y + mainCamera.orthographicSize;

		ShaftManager.Instance.OnNewShaftCreated += ShaftManager_OnNewShaftCreated;
	}
	void Update()
	{
		if (PreventScroll) return;
		if (NoodyCustomCode.IsPointerOverUIElement() == true) return;


		// Detect left mouse button press (or touch)
		if (Input.GetMouseButtonDown(0) && !PreventScroll)
		{
			touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			_isDragging = true;
			_oldPos = transform.position;
			_isOverShooting = false;
		}

		if (_isOverShooting == false)
		{
			_currentSpeed = Vector3.Distance(_oldPos, transform.position) * 30;
			_dir = (this.transform.position - _oldPos).normalized;
			_oldPos = transform.position;
		}

		// If the left mouse button is held down and dragging is active
		if (_isDragging && Input.GetMouseButton(0))
		{
			Vector3 difference = touchPos - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 tempPos = transform.position + new Vector3(0, difference.y, 0);
			tempPos.y = Mathf.Clamp(tempPos.y, minY, maxY);

			// Move the camera by the difference
			transform.position = tempPos;
		}

		// Reset dragging flag when the mouse button is released
		if (Input.GetMouseButtonUp(0))
		{
			_isDragging = false;
			Vector3 endTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (endTouchPos != touchPos)
				OverShootAnimation(_dir);
		}
	}

	private void ShaftManager_OnNewShaftCreated()
	{
		NoodyCustomCode.ObjectShake(_camera.gameObject, _duration, _magnitude);
	}

	private async void OverShootAnimation(Vector3 direction)
	{
		_isOverShooting = true;
		while (_currentSpeed != 0f)
		{
			_currentSpeed -= _decelerateSpeed;
			if (_currentSpeed < 0) _currentSpeed = 0;
			float appliedSpeed = Mathf.Clamp(_currentSpeed, 0, _maxSpeed);

			Vector3 overShoot = direction * appliedSpeed * Time.deltaTime;
			Vector3 tempPos = transform.position + new Vector3(0, overShoot.y, 0);
			tempPos.y = Mathf.Clamp(tempPos.y, minY, maxY);

			transform.position = tempPos;
			await UniTask.Yield();
		}
	}


	public void SetMaxY(float y)
	{
		maxY = y;
	}

	public Transform GetCurrentTransform()
	{
		return transform;
	}
	public void SetYCamera(float value)
	{
		Vector3 position = gameObject.transform.position; // Get the current position
		position.y = value; // Update the y value
		gameObject.transform.position = position; // Assign the updated position back to the transform
	}

}
