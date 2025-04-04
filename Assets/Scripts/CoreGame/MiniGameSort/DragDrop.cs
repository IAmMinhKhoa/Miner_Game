using System.Collections;
using NOOD.Sound;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragDrop : MonoBehaviour
{
	Collider2D collider2d;
	tsInfo tsInfo;
	public bool isDragging, isDrop, isHittingSomething;
	public Camera cameraGameSort;
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
		if (enabled)
		{
			SoundManager.PlaySound(SoundEnum.click);
			isDragging = true;
			isDrop = false;
			if (tsInfo.parentBox != null)
			{
				tsInfo.parentBox.objects[tsInfo.slot] = null;
				tsInfo.parentBox.UpdateBoxCount();
				tsInfo.gameObject.transform.parent = null;
			}
			transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 10;
		}
	}

	void OnMouseDrag()
	{
		if(enabled)
		{
			transform.position = MouseWorldPosition();
		}
	}

	void OnMouseUp()
	{
		if (enabled)
		{
			isDrop = true;
			if (!isHittingSomething || isDrop)
			{
				StartCoroutine(waiter());
			}
			transform.GetChild(0).GetComponent<Renderer>().sortingOrder = 7;
		}
	}

	Vector3 MouseWorldPosition()
	{
		var mouseScreenPos = Input.mousePosition;
		mouseScreenPos.z = cameraGameSort.WorldToScreenPoint(transform.position).z;
		return cameraGameSort.ScreenToWorldPoint(mouseScreenPos);
	}


	private void OnTriggerStay2D(Collider2D collision)
	{
		if (enabled)
		{
			isHittingSomething = true;
			if (collision.GetComponent<BoxInfo>() != null && !collision.GetComponent<BoxInfo>().isFull && isDragging)
			{
				GameObject box = collision.gameObject;
				if (isDrop)
				{
					int choosenSlot = -1;
					float lastDistance = -1;
					for (int i = 0; i <= 2; i++)
					{
						if (box.GetComponent<BoxInfo>().objects[i] == null)
						{
							float distance = Vector3.Distance(transform.position, box.transform.GetChild(i).position);

							if (lastDistance == -1 || choosenSlot == -1)
							{
								lastDistance = distance;
								choosenSlot = i;
							}
							else if (distance < lastDistance)
							{
								lastDistance = distance;
								choosenSlot = i;
							}
						}
					}
					if (choosenSlot != -1 && lastDistance != -1)
					{
						UpdateBoxParent(choosenSlot, box);
					}
				}
			}
			else if (collision.GetComponent<BoxInfo>() != null && collision.GetComponent<BoxInfo>().isFull && isDragging)
			{
				if (isDrop)
				{
					StartCoroutine(waiter());
				}
			}
			else if (collision.GetComponent<BoxInfo>() == null && collision.GetComponent<tsInfo>() != null)
			{
				if (isDrop)
				{
					StartCoroutine(waiter());
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
		box.GetComponent<BoxInfo>().CheckMatchingBox();
	}

	IEnumerator waiter()
	{
		yield return new WaitForSeconds(0.05f);
		if (tsInfo.parentBox != null) transform.position = new Vector3(tsInfo.parentBox.GetComponent<BoxInfo>().slots[tsInfo.slot].transform.position.x, tsInfo.parentBox.GetComponent<BoxInfo>().slots[tsInfo.slot].transform.position.y, -1);
		tsInfo.parentBox.objects[tsInfo.slot] = gameObject;
		tsInfo.parentBox.UpdateBoxCount();
		transform.parent = tsInfo.parentBox.transform;
		isDragging = false;
	}
}
