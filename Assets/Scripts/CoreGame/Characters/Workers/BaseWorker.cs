using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Analytics;

public class BaseWorker : MonoBehaviour
{
    [SerializeField] protected BaseConfig config;
    [SerializeField] private bool isCollecting = true;
    [SerializeField] private double currentProduct = 0;

    public bool IsCollecting => isCollecting;
    public double CurrentProduct
    {
        get { return currentProduct; }
        set { currentProduct = value; }
    }

    public virtual void Move(Vector3 target)
    {
        transform.DOMove(target, config.MoveTime).OnComplete(() =>
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

    public virtual void Move(Vector3 target, float time)
    {
        transform.DOMove(target, time).OnComplete(() =>
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
        await UniTask.Delay((int)(config.WorkingTime * 1000));
    }

    protected virtual async UniTask IEDeposit()
    {
        await UniTask.Delay((int)(config.WorkingTime * 1000));
    }

    protected virtual async UniTask IECollect(double amount, float collectTime)
    {
        await UniTask.Delay((int)(collectTime * 1000));
        CurrentProduct += amount;
        ChangeGoal();
        Move(transform.position);
    }

    protected virtual async UniTask IEDeposit(double amount, float depositTime)
    {
        await UniTask.Delay((int)(depositTime * 1000));
        CurrentProduct -= amount;
        ChangeGoal();
        Move(transform.position);
    }
}
