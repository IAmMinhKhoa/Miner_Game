using System;
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

    protected WorkerState state = WorkerState.Idle;
    protected bool isArrive = false;

    public bool IsCollecting => isCollecting;
    public double CurrentProduct
    {
        get { return currentProduct; }
        set { currentProduct = value; }
    }


    public virtual void Move(Vector3 target)
    {
        Move(target, config.MoveTime);
        // transform.DOMove(target, config.MoveTime).SetEase(Ease.Linear).OnComplete(() =>
        // {
        //     if (isCollecting)
        //     {
        //         Collect();
        //     }
        //     else
        //     {
        //         Deposit();
        //     }
        // }).Play();
    }

    public virtual async void Move(Vector3 target, float moveTime)
    {
        Debug.Log("target: " + target);
        state = WorkerState.Moving;
        bool direction = transform.position.x > target.x;
        float distance = Vector3.Distance(target, this.transform.position);
        PlayAnimation(state, direction);

        isArrive = false;
        while (isArrive == false)
        {
            await UniTask.Yield();
            if(Vector3.Distance(this.transform.position, target) < 0.1f)
            {
                if(isArrive == false)
                {
                    if(IsCollecting)
                    {
                        Collect();
                    }
                    else
                    {
                        Deposit();
                    }
                    isArrive = true;
                }
            }
            else
            {
                isArrive = false;
                Vector3 dir = (target - transform.position).normalized;
                this.transform.position += dir * distance/moveTime * Time.deltaTime;
            }
        }
    }

    

    protected virtual async void Collect()
    {
        ChangeGoal();
        state = WorkerState.Working;
        PlayAnimation(state, false);
    }

    protected virtual async void Deposit()
    {
        ChangeGoal();
        state = WorkerState.Idle;
        PlayAnimation(state, false);
    }

    protected virtual void ChangeGoal()
    {
        isCollecting = !isCollecting;
    }

    protected virtual async UniTask IECollect()
    {
        
    }

    protected virtual async UniTask IEDeposit()
    {

    }

    protected virtual async UniTask IECollect(double amount, float collectTime)
    {

    }

    protected virtual async UniTask IEDeposit(double amount, float depositTime)
    {
        
    }

    protected enum WorkerState
    {
        Idle,
        Working,
        Moving
    }

    protected virtual void PlayAnimation(WorkerState state, bool direction)
    {
        
    }
}
