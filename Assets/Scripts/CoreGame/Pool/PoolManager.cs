using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Patterns.Singleton<PoolManager>
{
    public List<BYPool> pool_defaults;
    public Dictionary<string, BYPool> dic_pool = new Dictionary<string, BYPool>();
	protected override void Awake()
	{
		isPersistent = false;
		base.Awake();

	}
	private void Start()
    {
        foreach(BYPool pool in pool_defaults)
        {
            Create(pool);
            dic_pool[pool.name_pool] = pool;
            
        }
    }
    public  void AddNewPool(BYPool pool)
    {
        if(!dic_pool.ContainsKey(pool.name_pool))
        {
            Create(pool);
            dic_pool[pool.name_pool] = pool;
        }
    }
    public  void Create(BYPool pool)
    {
		GameObject parentObject = new GameObject(pool.preFab.name + "_Parent");
		for (int i=0; i<pool.total;i++)
        {
            Transform trans = Instantiate(pool.preFab, parentObject.transform);
            pool.elements.Add(trans);
            trans.gameObject.SetActive(false);
        }
    }
}
