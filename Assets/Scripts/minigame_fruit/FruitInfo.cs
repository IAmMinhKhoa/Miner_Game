using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitInfo : MonoBehaviour
{

    
    public int index;


    private GameObject claw;
    private Rigidbody2D rb;
    void Start()
    {
        claw = GameObject.Find("holdpoint");
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (tag == "holdingfruit")
        {
            rb.isKinematic = true;
            transform.position = claw.transform.position;
        }
        else
        {
            rb.isKinematic = false;
            GetComponent<FruitCombiner>().enabled = true;
        }
    }
}
