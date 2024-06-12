using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using DG.Tweening;

public class BaseMiner : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private BigInteger m_goldPerSecond = 50;
    [SerializeField]
    private BigInteger m_goldCapacity = 100;
    [SerializeField]
    private BigInteger m_currentGold = 0;
    private bool m_isCollecting = true;

    public BigInteger GoldPerSecond
    {
        get { return m_goldPerSecond; }
        set { m_goldPerSecond = value; }
    }

    public BigInteger GoldCapacity
    {
        get { return m_goldCapacity; }
        set { m_goldCapacity = value; }
    }

    public BigInteger CurrentGold
    {
        get { return m_currentGold; }
        set { m_currentGold = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public void MoveMiner(UnityEngine.Vector3 target)
    {
        //
        transform.DOMove(target, 10f / moveSpeed).OnComplete(() =>
        {
            if (m_isCollecting)
            {
                CollectGold();
            }
            else
            {
                DepositGold();
            }
        }).Play();
    }

    protected virtual void CollectGold()
    {

    }

    protected virtual void DepositGold()
    {

    }

    protected virtual IEnumerator IECollectGold(float collectTime)
    {
        yield return null;
    }

    protected virtual IEnumerator IECollectGold(BigInteger amountCollect, float collectTime)
    {
        yield return null;
    }

    protected virtual IEnumerator IEDepositGold()
    {
        yield return null;
    }

    protected virtual IEnumerator IEDepositGold(BigInteger amount, float collectTime)
    {
        yield return null;
    }

    protected void ChangeGoal()
    {
        m_isCollecting = !m_isCollecting;
    }
}
