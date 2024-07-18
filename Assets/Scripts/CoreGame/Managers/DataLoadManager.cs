using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DataLoadManager : BaseGameManager
{
    #region ----Enums----
    private enum GameState
    {
        LoadTemplateData,
        LoadingTemplateData,
        LoadManagerData,
        LoadingManagerData,
        LoadShaftData,
        LoadingShaftData,
        LoadingElevatorData,
        LoadingCounterData,
        Done
    }
    #endregion

    #region ----Variables----
    private GameState dataGameState;
    #endregion

    protected override void Update()
    {
        base.Update();
        UpdateGameStates();
        Debug.Log("DataLoadManager Update:" + dataGameState);
    }

    void UpdateGameStates()
    {
        if (!base.IsDone()) return;

        switch (dataGameState)
        {
            case GameState.LoadTemplateData:
                LoadTemplateData();
                SetState(GameState.LoadingTemplateData);
                break;
            case GameState.LoadingTemplateData:
                if (CheckTemplateData())
                {
                    SetState(GameState.LoadManagerData);
                }
                break;
            case GameState.LoadManagerData:
                LoadManagerData();
                SetState(GameState.LoadingManagerData);
                break;
            case GameState.LoadingManagerData:
                if (CheckManagerData())
                {
                    SetState(GameState.LoadShaftData);
                }
                break;
            case GameState.LoadShaftData:
                break;
            case GameState.LoadingShaftData:
                break;
            case GameState.LoadingElevatorData:
                break;
            case GameState.LoadingCounterData:
                break;
            case GameState.Done:
                break;
        }
    }

    protected override void Init()
    {
        base.Init();
        totalGameStates += Enum.GetNames(typeof(GameState)).Length - 1;
        SetState(GameState.LoadTemplateData);
    }

    private void SetState(GameState state)
    {
        dataGameState = state;
    }

    protected new bool IsDone()
    {
        return dataGameState == GameState.Done;
    }

    public async UniTaskVoid LoadTemplateData()
    {
        MainGameData.managerDataSOList = Resources.LoadAll<ManagerDataSO>("ScriptableObjects/ManagerData").ToList();
        MainGameData.managerSpecieDataSOList = Resources.LoadAll<ManagerSpecieDataSO>("ScriptableObjects/ManagerSpecieData").ToList();
        MainGameData.managerTimeDataSOList = Resources.LoadAll<ManagerTimeDataSO>("ScriptableObjects/ManagerTimeData").ToList();

        MainGameData.isDone = true;
    }

    private bool CheckTemplateData()
    {
        return MainGameData.isDone;
    }

    private void LoadManagerData()
    {
        var managersController = ManagersController.Instance;
        managersController.Load();
    }

    private bool CheckManagerData()
    {
        return ManagersController.Instance.IsDone;
    }
}
