using PlayFabManager.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxClickManager : MonoBehaviour
{
	public Transform fxClickPrefab;  
	[SerializeField]private Camera mainCamera;
	
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 clickPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			clickPosition.z = 0;
			Transform fxTransform = PoolManager.Instance.dic_pool["Click"].Spawned();
			if (fxTransform != null)
			{
				fxTransform.position = clickPosition;
				fxTransform.rotation = Quaternion.identity;
			}
		}
	}
}
