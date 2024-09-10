using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class BYPool
{
    public int total;
    public string name_pool;
    public List<Transform> elements = new List<Transform>();
    public Transform preFab;
    public int index = -1;
    public BYPool()
    {

    }
    public BYPool(int total,string name_pool,Transform preFab)
    {
        this.total = total;
        this.name_pool = name_pool;
        this.preFab = preFab;
    }
    /// <summary>
    /// create an element
    /// </summary>
    /// <returns></returns>
    public Transform Spawned()
    {
        index++;
        if(index>=elements.Count)
        {
            index = 0;
        }
        Transform trans = elements[index];
        trans.gameObject.SetActive(true);
        trans.gameObject.SendMessage("Spawned", SendMessageOptions.DontRequireReceiver);
        return trans;
    }
    /// <summary>
    /// hide an element
    /// </summary>
    public void DesSpawned(Transform trans)
    {
        //Debug.Log("DesSpawn"+trans);
        if(elements.Contains(trans))
        {
            //elements.Add(trans);
            trans.gameObject.SendMessage("DesSpawned", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
        }
    }
}
