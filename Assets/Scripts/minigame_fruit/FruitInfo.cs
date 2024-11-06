using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FruitInfo : MonoBehaviour
{

    
    public int index;
	public Color mergeColor;


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
            //rb.isKinematic = true;
			rb.simulated = false;
            transform.position = claw.transform.position;
        }
        else
        {
            //rb.isKinematic = false;
			rb.simulated = true;
            GetComponent<FruitCombiner>().enabled = true;
        }
    }
}
