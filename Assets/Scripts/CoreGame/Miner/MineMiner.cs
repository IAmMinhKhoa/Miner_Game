using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineMiner : BaseMiner
{
    [SerializeField]
    private Transform m_mineLocation;

    [SerializeField]
    private Transform m_depositLocation;

    public Mine CurrentMine { get; set; }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            //MoveMiner(m_mineLocation.position);
            MoveMiner(CurrentMine.MineLocation.position);
        }
    }

    protected override void CollectGold()
    {
        base.CollectGold();
        Debug.Log("Gold collected");
        float collectTime = (float) (GoldCapacity / GoldPerSecond);
        ChangeGoal();
        StartCoroutine(IECollectGold(collectTime));
    }

    protected override void DepositGold()
    {
        CurrentMine.CurrentDeposit.DepositGold(CurrentGold);
        Debug.Log("Gold deposited");
        CurrentGold = 0;
        ChangeGoal();
    }

    protected override IEnumerator IECollectGold(float collectTime)
    {
        Debug.Log("Gold collected");
        yield return new WaitForSeconds(collectTime);
        CurrentGold = GoldCapacity;
        //MoveMiner(m_depositLocation.position);
        MoveMiner(CurrentMine.MinerLocation.position);
    }
}
