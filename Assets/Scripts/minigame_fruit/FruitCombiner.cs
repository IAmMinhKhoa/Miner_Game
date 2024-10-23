using DG.Tweening;
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
            if (GetComponent<FruitInfo>().index == otherIndex && otherIndex != FruitList.list.Count)
            {
                if(gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
                {
                    Destroy(gameObject);
                    Destroy(collision.gameObject);
					Vector3 hitpoint = (transform.position + collision.gameObject.transform.position) / 2;
					GameObject newFruit = Instantiate(FruitList.list[otherIndex], hitpoint, Quaternion.identity, gameObject.transform.parent);
					newFruit.transform.DOScale(0.001f, 0f);
					newFruit.transform.DOScale(0.06f, 0.5f);
					Color newColor;
					if (ColorUtility.TryParseHtmlString(GetComponent<FruitInfo>().mergeColor, out newColor))
					{
						GameObject FX = Instantiate(combineFX, hitpoint, Quaternion.identity);
						FX.GetComponent<ParticleSystem>().startColor = newColor;
						ParticleSystem[] particleSystems = FX.GetComponentsInChildren<ParticleSystem>();
						foreach (var ps in particleSystems)
						{
							var main = ps.main;
							main.startColor = newColor;
						}
						Destroy(FX, 3f);
					}
				}
				GameObject.FindWithTag("manager").GetComponent<FruitGameManager>().UpdateScore(otherIndex);
			}
        }


    }
}
