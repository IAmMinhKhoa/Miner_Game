using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManagerLocation : MonoBehaviour
{
    private Manager _manager;
    public Manager Manager => _manager;
    [SerializeField] private int locationType;
    public ManagerLocation LocationType => (ManagerLocation)locationType;

    public virtual void RunBoost()
    {

    }

    public void SetManager(Manager manager)
    {
        _manager = manager;
        if (manager == null)
        {
            return;
        }
        manager.gameObject.transform.position = transform.position;
    }
}
