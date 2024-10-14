using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopGround : MonoBehaviour
{
    public float groundSpeed = 1f;  
    public float groundWidth = 0.5f; 

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.left * groundSpeed * Time.deltaTime);

        if (transform.position.x < startPosition.x - groundWidth)
        {
            transform.position = new Vector3(startPosition.x, transform.position.y, transform.position.z);
        }
    }
}
