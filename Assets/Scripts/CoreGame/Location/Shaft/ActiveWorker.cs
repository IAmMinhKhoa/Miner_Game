using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWorker : MonoBehaviour
{
	public ShaftUI shaftUI;
	private void OnMouseDown()
	{
		shaftUI.AwakeWorker();
	}
}
