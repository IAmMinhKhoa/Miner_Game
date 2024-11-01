using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class DragDrop : MonoBehaviour
{
	Collider2D collider2d;
	tsInfo tsInfo;
	public bool isDragging, isDrop;

	void Awake()
	{
		isDrop = false;
		collider2d = GetComponent<Collider2D>();
		tsInfo = GetComponent<tsInfo>();
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
		StartCoroutine(waiter());
	}

	Vector3 MouseWorldPosition()
	{
		var mouseScreenPos = Input.mousePosition;
		mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
		return Camera.main.ScreenToWorldPoint(mouseScreenPos);
	}


	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.GetComponent<BoxInfo>() != null && !collision.GetComponent<BoxInfo>().isFull && isDragging)
		{
			GameObject box = collision.gameObject;
			if (box.GetComponent<BoxInfo>().objectCount == 2)
			{

			}
			else
			{
				if (isDrop)
				{
					float direct = transform.position.x - box.transform.position.x;

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
						transform.position = tsInfo.parentBox.slots[tsInfo.slot].position;
					}
					isDrop = false;
				}
			}

		}
		
	}

	void UpdateBoxParent(int slot, GameObject box)
	{
		if(tsInfo.parentBox != null) tsInfo.parentBox.objects[tsInfo.slot] = null;
		tsInfo.slot = slot;
		tsInfo.parentBox = box.GetComponent<BoxInfo>();
		tsInfo.parentBox.objects[tsInfo.slot] = gameObject;
		transform.parent = tsInfo.parentBox.transform;
		transform.position = box.GetComponent<BoxInfo>().slots[slot].transform.position;
		isDragging = false;
	}

	IEnumerator waiter()
	{
		yield return new WaitForSeconds(0.01f);
		if (tsInfo.parentBox != null) transform.position = tsInfo.parentBox.GetComponent<BoxInfo>().slots[tsInfo.slot].position;
		isDragging = false;
	}
}
