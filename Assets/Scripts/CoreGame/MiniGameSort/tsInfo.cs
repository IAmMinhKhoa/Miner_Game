using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tsInfo : MonoBehaviour
{
	public int id;
	public int slot;
	public BoxInfo parentBox;

	public void UpdateBoxParent(int slot1, GameObject box)
	{
		if (parentBox != null) parentBox.objects[slot] = null;
		slot = slot1;
		parentBox = box.GetComponent<BoxInfo>();
		parentBox.objects[slot1] = gameObject;
		transform.parent = parentBox.transform;
		transform.position = new Vector3(box.GetComponent<BoxInfo>().slots[slot1].transform.position.x, box.GetComponent<BoxInfo>().slots[slot1].transform.position.y, -1);
		box.GetComponent<BoxInfo>().UpdateBoxCount();
	}

	public void ClearThis()
	{
		if (parentBox != null) parentBox.objects[slot] = null;
		Destroy(gameObject);
	}
}
