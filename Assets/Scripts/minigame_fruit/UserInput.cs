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



    private void Update()
    {
        if (manager.isHolding)
        {
			if (claw.transform.position.x <= (lpoint.transform.position.x + limit))
			{
				claw.transform.position = new Vector3(lpoint.transform.position.x + limit, claw.transform.position.y, claw.transform.position.z);
			}
			else if (claw.transform.position.x >= (rpoint.transform.position.x - limit))
			{
				claw.transform.position = new Vector3(rpoint.transform.position.x - limit, claw.transform.position.y, claw.transform.position.z);
			}
		}
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			switch (touch.phase)
			{
				case TouchPhase.Began:
					touchStartPos = touch.position;
				break;

				case TouchPhase.Moved:
					Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
					float minX = lpoint.transform.position.x + limit;
					float maxX = rpoint.transform.position.x - limit;
					claw.transform.position = new Vector3(Mathf.Clamp(touchedPos.x, minX, maxX), claw.transform.position.y, claw.transform.position.z);

					

					break;

				case TouchPhase.Ended:
					manager.OnClickDrop();
				break;
			}
		}
	}
}
