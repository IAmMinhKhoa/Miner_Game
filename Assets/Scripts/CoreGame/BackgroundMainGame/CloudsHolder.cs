using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsHolder : MonoBehaviour
{
	[SerializeField] private Transform camPos;
	public float upOffset;
	public float downOffset;

	private void LateUpdate()
	{
		if(transform.position.y < camPos.position.y - upOffset)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + 11f, transform.position.z);
		}
		if (transform.position.y > camPos.position.y - downOffset)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y - 11f, transform.position.z);
		}
	}
}
