using PlayFabManager.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxClickManager : MonoBehaviour
{
	public Transform fxClickPrefab;  
	[SerializeField]private Camera mainCamera;
	public string name_fxClick_pool;

	void Start()
	{
		//PlayFabDataManager.LoadingIsDone += LoadingIsDone;
		LoadingIsDone();
	}

	private void LoadingIsDone()
	{
		BYPool pool = new BYPool(5, name_fxClick_pool, fxClickPrefab);
		PoolManager.Instance.AddNewPool(pool);
	}
	IEnumerator OnEndLife(Transform fxTransform)
	{
		yield return new WaitForSeconds(0.5f);
		PoolManager.Instance.dic_pool[name_fxClick_pool].DesSpawned(fxTransform);
	}
	private void OnDisable()
	{
	//	PlayFabDataManager.LoadingIsDone -= LoadingIsDone;
	}
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			clickPosition.z = 0;
			Transform fxTransform = PoolManager.Instance.dic_pool[name_fxClick_pool].Spawned();
			if(fxTransform != null )
			{
				fxTransform.position = clickPosition;
				fxTransform.rotation = Quaternion.identity;
				ParticleSystem fx = fxTransform.GetComponent<ParticleSystem>();
				fx.Play();
				StartCoroutine(OnEndLife(fxTransform));
			}	
		}
	}
}
