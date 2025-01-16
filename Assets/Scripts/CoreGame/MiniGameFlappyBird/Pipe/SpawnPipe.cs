using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPipe : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pipe_pf;
    [SerializeField] private float height_range = 0.45f;
    [SerializeField] private float time_spawn = 1.5f;
    [SerializeField] private float time_count = 0;
	private string name_pool = "Pipe";
    private bool isMouseClick;
	private void Awake()
	{
		
	}
	void Start()
    {
		BYPool pool = new BYPool(10, name_pool, pipe_pf.transform);
		PoolManager.Instance.AddNewPool(pool);
		InputManager.Instance.OnMouseClick.AddListener(OnMouseClick);
    }
    private void OnMouseClick(bool isMouseClick)
    {
        this.isMouseClick = isMouseClick;
    }    

    // Update is called once per frame
    void Update()
    {
        time_count += Time.deltaTime;
        if(isMouseClick)
        {
            if(time_count>=time_spawn)
            {
                time_count = 0;
                Spawn();
            }
        }
    }
    private void Spawn()
    {
        Vector3 posion = transform.position + new Vector3(0, Random.Range(-height_range, height_range), 0);
        Transform pipe = PoolManager.Instance.dic_pool["Pipe"].Spawned();
        pipe.position = posion;
        pipe.rotation = Quaternion.identity;
    }
}
