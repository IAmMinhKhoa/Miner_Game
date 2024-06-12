using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ElevatorMiner : BaseMiner
{
    private int _currentBaseIndex = -1;
    private Deposit _currentDeposit;

    [SerializeField]
    private Elevator elevator;
    private void MoveToNextBase()
    {
        // Move to the next floor
        _currentBaseIndex++;
        Mine currentMine = MineManager.Instance.Mines[_currentBaseIndex];
        Vector2 nextPos = currentMine.DepositLocation.position;
        Vector2 fixPos = new(transform.position.x, nextPos.y);

        _currentDeposit = currentMine.CurrentDeposit;
        MoveMiner(fixPos);
    }

    protected override void CollectGold()
    {
        if (_currentDeposit
        && !_currentDeposit.CanCollectGold()
        || _currentBaseIndex == MineManager.Instance.Mines.Count - 1)
        {
            _currentBaseIndex = -1;
            ChangeGoal();
            Vector2 nextPos = new(transform.position.x, elevator.DepositLocation.position.y);
            MoveMiner(nextPos);
            return;
        }

        var amountToCollect = _currentDeposit.CollectGold(this);
        float collectTime = (float)(amountToCollect / GoldPerSecond);
        StartCoroutine(IECollectGold(amountToCollect, collectTime));
    }

    protected override IEnumerator IECollectGold(System.Numerics.BigInteger amountCollect, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);
        CurrentGold += amountCollect;
        _currentDeposit.DepositGold(amountCollect);

        if (_currentBaseIndex == MineManager.Instance.Mines.Count - 1)
        {
            _currentBaseIndex = -1;
            ChangeGoal();
            Vector2 nextPos = new(transform.position.x, elevator.DepositLocation.position.y);
            MoveMiner(nextPos);
        }
        else
        {
            MoveToNextBase();
        }
        
    }

    protected override void DepositGold()
    {
        if (CurrentGold <= 0)
        {
            _currentBaseIndex = -1;
            ChangeGoal();
            MoveToNextBase();
            return;
        }

        float depositeTime = (float)(CurrentGold / GoldPerSecond);
        StartCoroutine(IEDepositGold(CurrentGold, depositeTime));
    }

    protected override IEnumerator IEDepositGold(System.Numerics.BigInteger amount, float collectTime)
    {
        yield return new WaitForSeconds(collectTime);
        _currentDeposit.DepositGold(amount);
        CurrentGold = 0;
        _currentBaseIndex = -1;

        ChangeGoal();
        MoveToNextBase();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed");
            MoveToNextBase();
        }
    }
}
