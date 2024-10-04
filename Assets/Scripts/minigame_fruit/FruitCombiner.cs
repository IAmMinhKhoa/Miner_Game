using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitCombiner : MonoBehaviour
{
    public GameObject combineFX;
    public ListFruit FruitList;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<FruitInfo>() != null)
        {
            int otherIndex = collision.gameObject.GetComponent<FruitInfo>().index;
            if (GetComponent<FruitInfo>().index == otherIndex)
            {
                if(gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
                    if (otherIndex != 11)
                    {
                        Vector3 hitpoint = (transform.position + collision.gameObject.transform.position)/2;
                        Instantiate(FruitList.list[otherIndex], hitpoint, Quaternion.identity, gameObject.transform.parent);
                        Instantiate(combineFX, hitpoint, Quaternion.identity);
                        GameObject.FindWithTag("manager").GetComponent<FruitGameManager>().UpdateScore(otherIndex*2);
                    }
                }
            }
        }
    }


}
