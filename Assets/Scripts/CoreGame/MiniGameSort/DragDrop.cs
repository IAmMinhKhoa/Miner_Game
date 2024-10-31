using UnityEngine;
using UnityEngine.UIElements;

public class DragDrop : MonoBehaviour
{
	Vector3 offset;
	Collider2D collider2d;
	public bool isDragging;
	public Vector3 tempPos;
	public bool isCollide;

	void Awake()
	{
		collider2d = GetComponent<Collider2D>();
	}

	void OnMouseDown()
	{
		offset = transform.position - MouseWorldPosition();
		isDragging = true;
		tempPos = transform.position;
	}

	void OnMouseDrag()
	{
		transform.position = MouseWorldPosition() + offset;
		
	}

	void OnMouseUp()
	{
		isDragging = false;
		if (!isCollide)
		{
			transform.position = tempPos;
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
		isCollide = true;
		if (collision.GetComponent<BoxInfo>() != null && !collision.GetComponent<BoxInfo>().isFull)
		{
			GameObject box = collision.gameObject;
			Debug.Log(box.transform.position.x + "");
			if (box.GetComponent<BoxInfo>().objectCount == 2)
			{

			}
			else
			{
				if (!isDragging)
				{
					float direct = transform.position.x - box.transform.position.x;

					if(direct < -0.1f)
					{
						transform.position = box.GetComponent<BoxInfo>().slots[0].transform.position;
						tempPos = transform.position;
					}
					if(direct > 0.1f)
					{
						transform.position = box.GetComponent<BoxInfo>().slots[2].transform.position;
						tempPos = transform.position;
					}
					if (direct > -0.1f && direct < 0.1f)
					{
						transform.position = box.GetComponent<BoxInfo>().slots[1].transform.position;
						tempPos = transform.position;
					}
					return;
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		isCollide = false;
	}
}
