using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{
    [SerializeField] private GameObject claw, lpoint, rpoint;
	[SerializeField] private FruitGameManager manager;
	private bool moveLeft;
    public float limit;
	private Vector2 touchStartPos;

	private void Start()
	{
		limit = 0;
		claw.GetComponent<Renderer>().enabled = false;
	}

	private void Update()
    {
		if (Input.touchCount > 0 && manager.isPlaying)
		{
			Touch touch = Input.GetTouch(0);

			switch (touch.phase)
			{
				case TouchPhase.Began:
					touchStartPos = touch.position;
					claw.GetComponent<Renderer>().enabled = true;
					Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
					float minX = lpoint.transform.position.x + limit;
					float maxX = rpoint.transform.position.x - limit;
					claw.transform.position = new Vector3(Mathf.Clamp(touchedPos.x, minX, maxX), claw.transform.position.y, claw.transform.position.z);
					break;

				case TouchPhase.Moved:
					Vector3 touchedPos1 = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
					float minX1 = lpoint.transform.position.x + limit;
					float maxX1 = rpoint.transform.position.x - limit;
					claw.transform.position = new Vector3(Mathf.Clamp(touchedPos1.x, minX1, maxX1), claw.transform.position.y, claw.transform.position.z);
					break;

				case TouchPhase.Ended:
					manager.OnClickDrop();
					claw.GetComponent<Renderer>().enabled = false;
					break;
			}
		}
	}
}
