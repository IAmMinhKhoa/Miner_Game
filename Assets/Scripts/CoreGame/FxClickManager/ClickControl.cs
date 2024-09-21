using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickControl : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{


	}
	public void Spawned()
	{
		//Debug.LogError("Spawned Impact");
		StopCoroutine(nameof(OnEndLife));
		StartCoroutine(nameof(OnEndLife), 0.5f);
	}
	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator OnEndLife(float time)
	{
		yield return new WaitForSeconds(time);
		PoolManager.Instance.dic_pool["Click"].DesSpawned(transform);
	}
}
