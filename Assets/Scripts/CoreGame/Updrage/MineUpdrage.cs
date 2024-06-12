using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MineUpdrage : BaseUpdrage
{
    private Mine mine;

    private void Start()
    {
        mine = GetComponent<Mine>();
    }
    protected override void RunUpdrage()
    {
        Debug.Log("Mine Updrage");
        if (CurrentLevel % 10 == 0)
        {
            mine.CreateMiner();
        }

        foreach (var miner in mine.Miners)
        {
            miner.GoldPerSecond *= (BigInteger) m_collectPerSecondMutiplier;
            miner.GoldCapacity *= (BigInteger) m_collectCapacityMutiplier;
            
            if (CurrentLevel % 5 == 0)
            {
                miner.MoveSpeed *= m_moveSpeedMutiplier;
            }
        }
    }
}
