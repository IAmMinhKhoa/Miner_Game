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
                if(m_managerSkeletonAnimation.AnimationName != "Active")
                    m_managerSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);
            }
            else
            {
                if(m_managerSkeletonAnimation.AnimationName != "Idle")
                    m_managerSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
            }
        }
    }

    public void SetManager(Manager manager)
    {
        m_manager = manager;
    }
}
