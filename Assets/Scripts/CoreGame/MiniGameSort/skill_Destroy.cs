using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_Destroy : sortGameSkills
{
	[SerializeField] private SortGameManager gameManager;
	public string layerName = "box_SortGame";


	void Update()
	{
		if (isUsing)
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Began)
				{
					Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

					int layerMask = 1 << LayerMask.NameToLayer(layerName);
					RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, layerMask);

					if (hit.collider != null)
					{
						hit.collider.gameObject.GetComponent<BoxInfo>().DestroyBox();
						isUsing = false;
						StartCoroutine(waitToEnable());
					}
				}
			}
		}
	}

	IEnumerator waitToEnable()
	{
		yield return new WaitForSeconds(0.5f);
		gameManager.AdjustAllDragCode(true);
	}

	public override void ActiveSkill()
	{
		if (!isUsing)
		{
			isUsing = true;
			gameManager.AdjustAllDragCode(!isUsing);
		}
	}
}
