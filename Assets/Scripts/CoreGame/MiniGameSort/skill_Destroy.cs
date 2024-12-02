using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skill_Destroy : MonoBehaviour
{
	[SerializeField] private SortGameManager gameManager;
	public string layerName = "box_SortGame";
	public bool isSkillActivated = false;


	void Update()
	{
		if (isSkillActivated)
		{
			if (Input.touchCount > 0)
			{
				Touch touch = Input.GetTouch(0);

				if (touch.phase == TouchPhase.Began)
				{
					Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

					int layerMask = LayerMask.GetMask(layerName);
					RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero, Mathf.Infinity, layerMask);

					if (hit.collider != null)
					{
						hit.collider.gameObject.GetComponent<BoxInfo>().DestroyBox();
						isSkillActivated = false;
						gameManager.AdjustAllDragCode(!isSkillActivated);
					}
				}
			}
		}
	}

	public void ActiveSkill()
	{
		isSkillActivated = !isSkillActivated;
		gameManager.AdjustAllDragCode(!isSkillActivated);
	}
}
