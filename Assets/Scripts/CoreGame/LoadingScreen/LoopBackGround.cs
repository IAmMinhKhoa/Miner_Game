using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBackGround : MonoBehaviour
{
	public Transform[] backgrounds;
	private float groundSpeed = 2f;
	private float backgroundWidth;

	void Start()
	{
		backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
	}

	void Update()
	{
		foreach (Transform bg in backgrounds)
		{
			bg.Translate(Vector3.left * groundSpeed * Time.deltaTime);
			if (bg.position.x < -backgroundWidth)
			{
				RepositionBackground(bg);
			}
		}
	}

	private void RepositionBackground(Transform bg)
	{
		Transform farthestBackground = backgrounds[0];
		foreach (Transform b in backgrounds)
		{
			if (b.position.x > farthestBackground.position.x)
			{
				farthestBackground = b;
			}
		}

		Vector3 groundOffset = new Vector3(backgroundWidth - 0.01f, 0, 0);
		bg.position = farthestBackground.position + groundOffset;
	}
}
