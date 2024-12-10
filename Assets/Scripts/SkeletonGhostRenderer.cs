using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGhostRenderer : MonoBehaviour
{
	static readonly Color32 TransparentBlack = new Color32(0, 0, 0, 0);
	const string colorPropertyName = "_Color";

	float fadeSpeed = 10;
	Color32 startColor;
	MeshFilter meshFilter;
	MeshRenderer meshRenderer;

	MaterialPropertyBlock mpb;
	int colorId;

	void Awake()
	{
		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		meshFilter = gameObject.AddComponent<MeshFilter>();

		colorId = Shader.PropertyToID(colorPropertyName);
		mpb = new MaterialPropertyBlock();
	}

	public void Initialize(Mesh mesh, Material[] materials, Color32 color, bool additive, float speed, int sortingLayerID, int sortingOrder)
	{
		StopAllCoroutines();

		gameObject.SetActive(true);
		meshRenderer.sharedMaterials = materials;
		meshRenderer.sortingLayerID = sortingLayerID;
		meshRenderer.sortingOrder = sortingOrder;
		meshFilter.sharedMesh = Instantiate(mesh);
		startColor = color;
		mpb.SetColor(colorId, color);
		meshRenderer.SetPropertyBlock(mpb);

		fadeSpeed = speed;

		if (additive)
			StartCoroutine(FadeAdditive());
		else
			StartCoroutine(Fade());
	}

	IEnumerator Fade()
	{
		Color32 c = startColor;
		Color32 black = SkeletonGhostRenderer.TransparentBlack;

		float t = 1f;
		for (float hardTimeLimit = 5f; hardTimeLimit > 0; hardTimeLimit -= Time.deltaTime)
		{
			c = Color32.Lerp(black, startColor, t);
			mpb.SetColor(colorId, c);
			meshRenderer.SetPropertyBlock(mpb);

			t = Mathf.Lerp(t, 0, Time.deltaTime * fadeSpeed);
			if (t <= 0)
				break;

			yield return null;
		}

		Destroy(meshFilter.sharedMesh);
		gameObject.SetActive(false);
	}

	IEnumerator FadeAdditive()
	{
		Color32 c = startColor;
		Color32 black = SkeletonGhostRenderer.TransparentBlack;

		float t = 1f;

		for (float hardTimeLimit = 5f; hardTimeLimit > 0; hardTimeLimit -= Time.deltaTime)
		{
			c = Color32.Lerp(black, startColor, t);
			mpb.SetColor(colorId, c);
			meshRenderer.SetPropertyBlock(mpb);

			t = Mathf.Lerp(t, 0, Time.deltaTime * fadeSpeed);
			if (t <= 0)
				break;

			yield return null;
		}

		Destroy(meshFilter.sharedMesh);

		gameObject.SetActive(false);
	}

	public void Cleanup()
	{
		if (meshFilter != null && meshFilter.sharedMesh != null)
			Destroy(meshFilter.sharedMesh);

		Destroy(gameObject);
	}
}
