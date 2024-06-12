using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;

public class BaseWorker : MonoBehaviour
{
    [SerializeField] protected float workingTime = 4f;
    [SerializeField] protected float moveTime = 1f;
    [SerializeField] protected float productPerSecond = 5;

    [SerializeField] private bool isCollecting = true;
    [SerializeField] private float currentProduct = 0;

    public float WorkingTime
    {
        get { return workingTime; }
        set { workingTime = value; }
    }

    public float MoveTime
    {
        get { return moveTime; }
        set { moveTime = value; }
    }

    public float ProductPerSecond
    {
        get { return productPerSecond; }
        set { productPerSecond = value; }
    }

    public float CurrentProduct
    {
        get { return currentProduct; }
        set { currentProduct = value; }
    }

    public virtual void Move(Vector3 target)
    {
        transform.DOMove(target, moveTime).OnComplete(() =>
        {
            if (isCollecting)
            {
                Collect();
            }
            else
            {
                Deposit();
            }
        }).Play();
    }

    protected virtual void Collect()
    {
        
    }

    protected virtual void Deposit()
    {
        
    }

    protected virtual void ChangeGoal()
    {
        isCollecting = !isCollecting;
    }

    protected virtual async UniTask IECollect()
    {
        await UniTask.Delay((int)(workingTime * 1000));
    }

    protected virtual async UniTask IEDeposit()
    {
        await UniTask.Delay((int)(workingTime * 1000));
    }
}
