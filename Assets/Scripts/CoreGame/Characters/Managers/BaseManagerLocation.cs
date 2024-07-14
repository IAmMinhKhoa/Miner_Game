using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManagerLocation : MonoBehaviour
{
    public Manager Manager { get; set; }
    [SerializeField] private int locationType;
    public ManagerLocation LocationType => (ManagerLocation)locationType;

    public virtual void RunBoost()
    {

    }
}
