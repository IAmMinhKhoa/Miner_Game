using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BaseWorker : MonoBehaviour
{
    public Action<Vector3> OnMoveToTarget;
    public Action<Vector3> OnArriveTarget;
    public Action<double> OnChangePawDone;

    [SerializeField] protected BaseConfig config;
    [SerializeField] private bool isCollecting = true;
    [SerializeField] private double currentProduct = 0;

    protected Transform collectTransform, depositTransform;

    protected WorkerState state = WorkerState.Idle;
    protected bool isArrive = false;

    Vector3 target;    
    public bool IsArrive => isArrive;
    public bool IsCollecting => isCollecting;
    public double CurrentProduct
    {
        get { return currentProduct; }
        set { currentProduct = value; }
    }

    protected virtual float WorkingTime
    {
        get { return config.WorkingTime; }
    }

    protected virtual float MoveTime
    {
        get { return config.MoveTime; }
    }

    private CancellationTokenSource cancellationToken = new CancellationTokenSource();

    public virtual void Move(Vector3 target)
    {
        Move(target, MoveTime);
    }

    public virtual void Move(Vector3 target, float moveTime)
    {
        this.target = target;
        OnMoveToTarget?.Invoke(target);
        state = WorkerState.Moving;
        bool direction = transform.position.x > target.x;
        PlayAnimation(state, direction);

        IEMove(target, moveTime);
    }

    private async UniTaskVoid IEMove(Vector3 target, float moveTime)
    {
        try
        {
            float distance = Vector3.Distance(collectTransform.position, depositTransform.position);
            float currentTime = 0f;
            isArrive = false;
            while (isArrive == false)
            {
                await UniTask.Yield(cancellationToken.Token);
                Vector3 dir = (target - transform.position).normalized;
                Vector3 tempPos = this.transform.position + dir * (distance / moveTime) * Time.deltaTime;
                currentTime += Time.deltaTime;

                if (Vector3.Distance(this.transform.position, target) <= Vector3.Distance(tempPos, target))
                {
                    this.transform.position = target;
                    isArrive = true;
                    OnArriveTarget?.Invoke(target);
                }
                else
                {
                    this.transform.position = tempPos;
                }

                if (currentTime >= moveTime)
                {
                    isArrive = true;
                    OnArriveTarget?.Invoke(target);
                }
            }

            if (this is ElevatorController)
                Debug.Log("IsCollecting: " + IsCollecting);
            if (IsCollecting)
            {
                Collect();
            }
            else
            {
                Deposit();
            }
        }
        catch (Exception ex) when (!(ex is OperationCanceledException)) // when (ex is not OperationCanceledException) at C# 9.0
        {
            Debug.LogError(ex);
            return;
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

    private void OnDestroy()
    {
        cancellationToken.Cancel();
        cancellationToken.Dispose();
    }
}
