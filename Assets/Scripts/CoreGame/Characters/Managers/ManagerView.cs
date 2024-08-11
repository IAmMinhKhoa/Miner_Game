using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class ManagerView : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation m_managerSkeletonAnimation;
    [SerializeField] private SkeletonAnimation m_boostVFXSkeletonAnimation;

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
        if (m_manager != null)
        {
            if (m_manager.IsBoosting)
            {
                if (m_managerSkeletonAnimation.AnimationName != "Active")
                    m_managerSkeletonAnimation.AnimationState.SetAnimation(0, "Active", true);

                if (m_boostVFXSkeletonAnimation.gameObject.activeSelf == false)
                {
                    m_boostVFXSkeletonAnimation.gameObject.SetActive(true);
                    int level = (int)m_manager.Level;
                    string boostLevel = "Lv" + (level + 1).ToString();
                    m_boostVFXSkeletonAnimation.AnimationState.SetAnimation(0, "Lv" + boostLevel, true);
                }
            }
            else
            {
                if (m_managerSkeletonAnimation.AnimationName != "Idle")
                    m_managerSkeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                if (m_boostVFXSkeletonAnimation.gameObject.activeSelf == true)
                    m_boostVFXSkeletonAnimation.gameObject.SetActive(false);
            }
        }
    }

    public void SetManager(Manager manager)
    {
        m_manager = manager;
    }

    [Button]
    public void SwapManager(SkeletonDataAsset skeletonDataAsset)
    {
        m_managerSkeletonAnimation.skeletonDataAsset = skeletonDataAsset;
        m_managerSkeletonAnimation.Initialize(true, true);
    }
}
