using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Patterns.Singleton<PoolManager>
{
    public List<BYPool> pool_defaults;
    public Dictionary<string, BYPool> dic_pool = new Dictionary<string, BYPool>();



    private void Start()
    {
	    InitPool();
    }

    public void InitPool()
    {
	    foreach (BYPool pool in pool_defaults)
	    {
		    if (dic_pool.ContainsKey(pool.name_pool))
		    {
			 //   Debug.LogWarning($"Pool '{pool.name_pool}' đã tồn tại, bỏ qua không tạo lại.");
			    continue;
		    }

		    Create(pool);
//		    Debug.Log("Tạo pool: " + pool.name_pool);
		    dic_pool[pool.name_pool] = pool;
	    }

    }
    public virtual void AddNewPool(BYPool pool)
    {
        if(!dic_pool.ContainsKey(pool.name_pool))
        {
            Create(pool);
            //Debug.Log("tao pool");
            //Debug.Log(pool.name_pool);
            dic_pool[pool.name_pool] = pool;
        }
    }
    protected virtual void Create(BYPool pool)
    {
		//GameObject poolParent = new GameObject(pool.name_pool + "_Parent");

		for (int i=0; i<pool.total;i++)
        {
			Transform trans = Instantiate(pool.preFab);
            pool.elements.Add(trans);
			trans.SetParent(pool.parentSpawm);
			trans.gameObject.SetActive(false);

        }
    }
}
