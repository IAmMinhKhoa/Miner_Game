using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollower : MonoBehaviour
{
	[SerializeField] private GameObject CamHolder;

    // Update is called once per frame
    void Update()
    {
		transform.position = new Vector3(CamHolder.transform.position.x, CamHolder.transform.position.y, 1);
	}
}
