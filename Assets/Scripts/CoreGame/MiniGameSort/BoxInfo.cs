using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInfo : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();
	public bool isFull;
	public int objectCount;

	public List<Transform> slots;

	private void Awake()
	{
		foreach (Transform t in transform)
		{
			slots.Add(t);
		}
	}

}
