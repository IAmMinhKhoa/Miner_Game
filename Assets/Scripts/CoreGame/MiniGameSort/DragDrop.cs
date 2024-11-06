using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DragDrop : MonoBehaviour
{
	Collider2D collider2d;
	tsInfo tsInfo;
	public bool isDragging, isDrop, isHittingSomething;

	void Awake()
	{
		isDrop = false;
		collider2d = GetComponent<Collider2D>();
		tsInfo = GetComponent<tsInfo>();
	}

	private void FixedUpdate()
	{
		isHittingSomething = false;
	}

	void OnMouseDown()
	{
		isDragging = true;
		Debug.Log("a");
	}

	void OnMouseDrag()
	{
		transform.position = MouseWorldPosition();
		
	}

	void OnMouseUp()
	{
		isDrop= true;
		if (!isHittingSomething)
		{
			StartCoroutine(waiter());
		}
	}

	Vector3 MouseWorldPosition()
	{
		var mouseScreenPos = Input.mousePosition;
		mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
		return Camera.main.ScreenToWorldPoint(mouseScreenPos);
	}


	private void OnTriggerStay2D(Collider2D collision)
	{
		isHittingSomething = true;
		if (collision.GetComponent<BoxInfo>() != null && !collision.GetComponent<BoxInfo>().isFull && isDragging)
		{
			GameObject box = collision.gameObject;
			if (box.GetComponent<BoxInfo>().objectCount == 2)
			{
				if (isDrop)
				{
					for(int i = 0;i <= 2; i++)
					{
						if(box.GetComponent<BoxInfo>().objects[i] == null) UpdateBoxParent(i, box);
					}
					isDrop = false;
				}
			}
			else
			{
				if (isDrop)
				{
					float direct = transform.position.x - box.transform.position.x;
					if(tsInfo.parentBox != null) tsInfo.parentBox.UpdateBoxCount();
					if (direct < -0.1f && box.GetComponent<BoxInfo>().objects[0] == null)
					{
						UpdateBoxParent(0, box);
					}
					else if (direct > 0.1f && box.GetComponent<BoxInfo>().objects[2] == null)
					{
						UpdateBoxParent(2, box);
					}
					else if (direct > -0.1f && direct < 0.1f && box.GetComponent<BoxInfo>().objects[1] == null)
					{
						UpdateBoxParent(1, box);
					}
					else
					{
						transform.position = new Vector3(tsInfo.parentBox.slots[tsInfo.slot].position.x, tsInfo.parentBox.slots[tsInfo.slot].position.y, -1);
					}
					isDrop = false;
				}
			}

		}
		
	}

	public void UpdateBoxParent(int slot, GameObject box)
	{
		if(tsInfo.parentBox != null)
		{
			tsInfo.parentBox.objects[tsInfo.slot] = null;
			tsInfo.parentBox.UpdateBoxCount();
		}
		tsInfo.slot = slot;
		tsInfo.parentBox = box.GetComponent<BoxInfo>();
		tsInfo.parentBox.objects[tsInfo.slot] = gameObject;
		transform.parent = tsInfo.parentBox.transform;
		transform.position = new Vector3(box.GetComponent<BoxInfo>().slots[slot].transform.position.x, box.GetComponent<BoxInfo>().slots[slot].transform.position.y, -1);
		isDragging = false;

		box.GetComponent<BoxInfo>().UpdateBoxCount();
	}

	IEnumerator waiter()
	{
		yield return new WaitForSeconds(0.001f);
		if (tsInfo.parentBox != null) transform.position = new Vector3(tsInfo.parentBox.GetComponent<BoxInfo>().slots[tsInfo.slot].transform.position.x, tsInfo.parentBox.GetComponent<BoxInfo>().slots[tsInfo.slot].transform.position.y, -1);
		isDragging = false;
	}
}
