using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInfo : MonoBehaviour
{
	public GameObject[] objects;
	public bool isFull;
	public int objectCount;

	public List<Transform> slots;

	private void Awake()
	{
		objects = new GameObject[3];
		foreach (Transform t in transform)
		{
			slots.Add(t);
		}
	}

}
