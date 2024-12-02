using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInfo : MonoBehaviour
{
	public bool isWaiting = true;

	public GameObject[] objects;
	public int col;
	public bool isFull;
	public int objectCount;
	public GameObject FX;
	public Transform boxParent;
	public List<Transform> slots;

	private void Awake()
	{
		objects = new GameObject[3];
		foreach (Transform t in transform)
		{
			slots.Add(t);
		}
		UpdateBoxCount();
	}

	public void UpdateBoxCount()
	{
		int temp = 0;
		foreach(GameObject t in objects)
		{
			if(t != null) temp++;
		}
		objectCount = temp;
		isFull = temp == 3;
	}

	public void CheckMatchingBox()
	{
		if (isFull)
		{
			tsInfo slot1 = objects[0].GetComponent<tsInfo>();
			tsInfo slot2 = objects[1].GetComponent<tsInfo>();
			tsInfo slot3 = objects[2].GetComponent<tsInfo>();

			if (slot1.id == slot2.id && slot2.id == slot3.id)
			{
				DestroyBox();
			}
		}
	}

	public void DestroyBox()
	{
		Instantiate(FX, transform.position, Quaternion.identity);
		FindObjectOfType<SortGameScore>().UpdateCurrentScore(100);
		FindObjectOfType<SortGameManager>().UpdateColAndCheck(col, -1);
		Destroy(gameObject);
	}

}
