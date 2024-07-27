using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class ManagerView : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation m_managerSkeletonAnimation;

    private Manager m_manager;

    void OnEnable()
    {
        FindAnyObjectByType<ElevatorUI>().ShowManagerButton(false);
    }
    void OnDisable()
    {
        FindAnyObjectByType<ElevatorUI>().ShowManagerButton(true);
    }

    void Update()
    {
        if(m_manager != null)
        {
            if(m_manager.IsBoosting)
            {
                if(m_managerSkeletonAnimation.AnimationName != "Quan Ly Ho - Buff")
                    m_managerSkeletonAnimation.AnimationState.SetAnimation(0, "Quan Ly Ho - Buff", true);
            }
            else
            {
                if(m_managerSkeletonAnimation.AnimationName != "Quan Ly Ho - Idle")
                    m_managerSkeletonAnimation.AnimationState.SetAnimation(0, "Quan Ly Ho - Idle", true);
            }
        }
    }

    public void SetManager(Manager manager)
    {
        m_manager = manager;
    }
}
