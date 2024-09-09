using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using PlayFabManager.Data;
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
        LoadElevatorData,
        LoadingElevatorData,
        LoadCounterData,
        LoadingCounterData,
        LoadPawData,
        LoadingPawData,
        LoadOfflineData,
        LoadingOfflineData,
        LoadSkinData,
        LoadingSkinData,
        Done
    }
    #endregion

    #region ----Variables----
    [SerializeField]
    private GameState dataGameState;
	private bool isDataLoading = false;
    #endregion

    protected override void Update()
    {
        base.Update();
        UpdateGameStates();
        Debug.Log("DataLoadManager Update:" + dataGameState);
    }

    async void UpdateGameStates()
    {
        if (!base.IsDone() || isDataLoading == true) return;

        switch (dataGameState)
        {
            case GameState.LoadTemplateData:
                await LoadTemplateData();
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
                LoadShaftData();
                SetState(GameState.LoadingShaftData);
                break;
            case GameState.LoadingShaftData:
                if (CheckShaftData())
                {
                    SetState(GameState.LoadElevatorData);
                }
                break;
            case GameState.LoadElevatorData:
                LoadElevatorData();
                SetState(GameState.LoadingElevatorData);
                break;
            case GameState.LoadingElevatorData:
                if (CheckElevatorData())
                {
                    SetState(GameState.LoadCounterData);
                }
                break;
            case GameState.LoadCounterData:
                LoadCounterData();
                SetState(GameState.LoadingCounterData);
                break;
            case GameState.LoadingCounterData:
                if (CheckCounterData())
                {
                    SetState(GameState.LoadPawData);
                }
                break;
            case GameState.LoadPawData:
                LoadPawData();
                SetState(GameState.LoadingPawData);
                break;
            case GameState.LoadingPawData:
                if (CheckPawData())
                {
                    SetState(GameState.LoadOfflineData);
                }
                break;
            case GameState.LoadOfflineData:
                LoadOfflineData();
                SetState(GameState.LoadingOfflineData);
                break;
            case GameState.LoadingOfflineData:
                if (CheckOfflineData())
                {
                    SetState(GameState.Done);
                }
                break;
            //case GameState.LoadSkinData:
            //	LoadSkinData();
            //	SetState(GameState.LoadingSkinData);
            //	break;
            //case GameState.LoadingSkinData:
            //	if (CheckSkinData())
            //	{
            //		SetState(GameState.Done);
            //	}
            //	break;

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

    #region ----Private Methods----
    private async UniTask LoadTemplateData()
    {
		isDataLoading = true;
        MainGameData.managerDataSOList = Resources.LoadAll<ManagerDataSO>("ScriptableObjects/ManagerData").ToList();
        MainGameData.managerSpecieDataSOList = Resources.LoadAll<ManagerSpecieDataSO>("ScriptableObjects/ManagerSpecieData").ToList();
        MainGameData.managerTimeDataSOList = Resources.LoadAll<ManagerTimeDataSO>("ScriptableObjects/ManagerTimeData").ToList();
        await PlayFabDataManager.Instance.LoadData();
        MainGameData.isDone = true;
		isDataLoading = false;
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

    private async UniTaskVoid LoadShaftData()
    {
        var shaftManager = ShaftManager.Instance;
        shaftManager.InitializeShafts();
    }

    private bool CheckShaftData()
    {
        return ShaftManager.Instance.IsDone;
    }
    private bool CheckSkinData()
    {
        return SkinManager.Instance.isDone;
    }

    private async UniTaskVoid LoadElevatorData()
    {
        var elevatorManager = ElevatorSystem.Instance;
        elevatorManager.InitializeElevators();
    }

    private bool CheckElevatorData()
    {
        return ElevatorSystem.Instance.IsDone;
    }

    private void LoadCounterData()
    {
        Debug.Log("LoadCounterData");
        var counterManager = Counter.Instance;
        counterManager.InitializeCounter();
    }

    private bool CheckCounterData()
    {
        return Counter.Instance.IsDone;
    }

    private void LoadPawData()
    {
        var pawManager = PawManager.Instance;
        pawManager.LoadPaw();
    }

    private void MoneyPawData()
    {
        var moneyManager = SuperMoneyManager.Instance;
        moneyManager.LoadMoney();
    }

    private bool CheckPawData()
    {
        return PawManager.Instance.IsDone;
    }

    private void LoadOfflineData()
    {
        var offlineManager = OfflineManager.Instance;
        offlineManager.LoadOfflineData();
    }
    private void LoadSkinData()
    {
        var skinManager = SkinManager.Instance;
        skinManager.FindSkinDataSO();
        skinManager.Load();
    }

    private bool CheckOfflineData()
    {
        return OfflineManager.Instance.IsDone;
    }
    #endregion
}
