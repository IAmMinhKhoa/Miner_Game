using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeControl : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed_move = 0.8f;
    void Start()
    {
        
    }
    protected virtual void Spawned()
    {
        StopCoroutine(nameof(OnEndLife));
        StartCoroutine(nameof(OnEndLife), 5f);
    }
    IEnumerator OnEndLife(float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.dic_pool["Pipe"].DesSpawned(transform);
        PoolManager.Instance.dic_pool["Pipe"].DesSpawned(transform);
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left*Time.deltaTime*speed_move);
    }
    private void OnBecameInvisible()
    {
        PoolManager.Instance.dic_pool["Pipe"].DesSpawned(transform);
    }
}
