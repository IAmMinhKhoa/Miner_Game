using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFabManager.Data;
using Cysharp.Threading.Tasks;
using System.Linq;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System;
public class OfflineManager : Patterns.Singleton<OfflineManager>
{
    #region ----Caculate attribute----
    List<double> _efficiencyFloors = new();
    List<double> _pawFloors = new();
    double _efficiencyElevator = 0;
    double _pawElevator = 0;
    int floorIndex = -1;
    double _totalPawTaken = 0;
    double _efficiencyCounter = 0;
    List<OfflineBoost> _offlineBoosts = new();
    #endregion
    private bool isDone = false;
    public bool IsDone => isDone;

    private readonly float _maxOfflineTime = 86400 * 30; // 30 days
    private readonly float _minOfflineTime = 1; // 1 second -- for testing
												// private void OnApplicationPause(bool pause)
												// {
												//     if (pause)
												//     {
												//         Save();
												//     }
												// }

	private bool isDataSaved = false;

	bool ApplicationWantsToQuit()
	{
		Debug.Log("Application is trying to quit...");
		Save();
		SendDataBeforeQuit().Forget(); // Gửi dữ liệu bất đồng bộ
		return isDataSaved; // Chỉ thoát khi dữ liệu đã được lưu
	}

	private async UniTaskVoid SendDataBeforeQuit()
	{
		try
		{
			await PlayFabDataManager.Instance.SendDataBeforeExit();
			isDataSaved = true;
			Debug.Log("Data sent successfully to PlayFab.");
		}
		catch (Exception ex)
		{
			Debug.LogError($"Error sending data to PlayFab: {ex.Message}");
			isDataSaved = false;
		}
	}

	private void Start()
	{
		Application.wantsToQuit += ApplicationWantsToQuit;
	}

	void OnApplicationFocus(bool focus)
    {
        Debug.Log("SYSTEM APPLICATION FOCUS:" + focus);
        bool isPaused = !focus;
        if (!isPaused)
        {
            string lastTimeQuit = PlayerPrefs.GetString("LastTimeQuit");
            if (!string.IsNullOrEmpty(lastTimeQuit))
            {
                System.DateTime lastTime = System.DateTime.Parse(lastTimeQuit);
                System.TimeSpan timeSpan = System.DateTime.Now - lastTime;
                Debug.Log("Time span: " + timeSpan.TotalSeconds);
            }
        }
        else
        {
            Save();
        }
    }

    private void Save()
    {
        ShaftManager.Instance.Save();
        ElevatorSystem.Instance.Save();
        Counter.Instance.Save();
        ManagersController.Instance.Save();
        PawManager.Instance.Save();
        SkinManager.Instance.Save();
        PlayFabDataManager.Instance.SaveData("LastTimeQuit", System.DateTime.Now.ToString());

    }


    public async void LoadOfflineData()
    {
        PlayFabDataManager.Instance.GoToMainGame();
		GetOfflineData();

		string lastTimeQuit = PlayFabDataManager.Instance.GetData("LastTimeQuit");
        if (string.IsNullOrEmpty(lastTimeQuit))
        {
            isDone = true;
            return;
        }

        System.DateTime lastTime = System.DateTime.Parse(lastTimeQuit);
        System.TimeSpan timeSpan = System.DateTime.Now - lastTime;
        double seconds = timeSpan.TotalSeconds;

        if (seconds >= _minOfflineTime)
        {
			
            var offlineMoney = await CaculateOfflinePaw((float)seconds);
			Debug.Log("Caculate Offline :" + offlineMoney);
			GameUI.Instance.OpenOffline(offlineMoney);
        }

        CalculateManagerCooldown(seconds);
        double offlinePaw = PawBonus(seconds);
		// update ADS double up or something here
		//  PawManager.Instance.AddPaw(offlinePaw);
		//TutorialManager.Instance.Triggertutorial(1);

        isDone = true;
    }

    private void CalculateManagerCooldown(double seconds)
    {
        foreach (var manager in ManagersController.Instance.ShaftManagers)
        {
            if (manager.CurrentBoostTime > 0 || manager.CurrentCooldownTime > 0)
            {
                var time = manager.CurrentBoostTime - seconds;
                if (time < 0)
                {
                    time -= manager.CurrentCooldownTime;

                    if (time <= 0)
                    {
                        manager.SetCurrentTime(0, 0);
                    }
                    else
                    {
                        manager.SetCurrentTime(0, (float)time);
                    }
                }
                else
                {
                    manager.SetCurrentTime((float)time, manager.CurrentCooldownTime);
                }

                manager.RunTimer();
            }
        }

        foreach (var manager in ManagersController.Instance.ElevatorManagers)
        {
            if (manager.CurrentBoostTime > 0 || manager.CurrentCooldownTime > 0)
            {
                var time = manager.CurrentBoostTime - seconds;
                if (time < 0)
                {
                    time -= manager.CurrentCooldownTime;

                    if (time <= 0)
                    {
                        manager.SetCurrentTime(0, 0);
                    }
                    else
                    {
                        manager.SetCurrentTime(0, (float)time);
                    }
                }
                else
                {
                    manager.SetCurrentTime((float)time, manager.CurrentCooldownTime);
                }

                manager.RunTimer();
            }
        }

        foreach (var manager in ManagersController.Instance.CounterManagers)
        {
            if (manager.CurrentBoostTime > 0 || manager.CurrentCooldownTime > 0)
            {
                var time = manager.CurrentBoostTime - seconds;
                if (time < 0)
                {
                    time -= manager.CurrentCooldownTime;

                    if (time <= 0)
                    {
                        manager.SetCurrentTime(0, 0);
                    }
                    else
                    {
                        manager.SetCurrentTime(0, (float)time);
                    }
                }
                else
                {
                    manager.SetCurrentTime((float)time, manager.CurrentCooldownTime);
                }

                manager.RunTimer();
            }
        }
    }

    private double PawBonus(double seconds)
    {
        double pawBonus = 0;
        return pawBonus;
    }

    private void GetOfflineData()
    {
        _efficiencyFloors = new List<double>();
        _pawFloors = new List<double>();
        _offlineBoosts = new List<OfflineBoost>();

        foreach (var shaft in ShaftManager.Instance.Shafts)
        {
			// _efficiencyFloors.Add(1f);
			// _pawFloors.Add(shaft.CurrentDeposit.CurrentPaw);

            var manager = shaft.ManagerLocation.Manager;
            if (manager != null && manager.CurrentBoostTime > 0 && manager.BoostType != BoostType.Costs)
            {
                OfflineBoost offlineBoost = new OfflineBoost();
                offlineBoost.time = shaft.ManagerLocation.Manager.CurrentBoostTime;
                offlineBoost.location = ManagerLocation.Shaft;
                offlineBoost.index = shaft.shaftIndex;
                offlineBoost.bonus = shaft.GetManagerBoost(manager.BoostType);
                _offlineBoosts.Add(offlineBoost);
            }
        }

        _efficiencyElevator = 1f;
        _pawElevator = ElevatorSystem.Instance.ElevatorDeposit.CurrentPaw;
        var ElevatorManager = ElevatorSystem.Instance.ManagerLocation.Manager;
        if (ElevatorManager != null && ElevatorManager.CurrentBoostTime > 0 && ElevatorManager.BoostType != BoostType.Costs)
        {
            OfflineBoost offlineBoost = new OfflineBoost();
            offlineBoost.time = ElevatorManager.CurrentBoostTime;
            offlineBoost.location = ManagerLocation.Elevator;
            offlineBoost.index = 0;
            offlineBoost.bonus = ElevatorSystem.Instance.GetManagerBoost(ElevatorManager.BoostType);
            _offlineBoosts.Add(offlineBoost);
        }

        _efficiencyCounter = 1f;

        _offlineBoosts.Sort((x, y) => x.time.CompareTo(y.time));
        Debug.Log("Paw bonus: " + JsonConvert.SerializeObject(_offlineBoosts));
        PlayerPrefs.SetString("OfflineBoosts", JsonConvert.SerializeObject(_offlineBoosts));
        PlayerPrefs.Save();
    }

    private async UniTask<OfflineMoneyData> CaculateOfflinePaw(float offlineTime)
    {
        if (offlineTime > _maxOfflineTime)
        {
            offlineTime = _maxOfflineTime;
        }
        float saveTime = offlineTime;

        foreach (var shaft in ShaftManager.Instance.Shafts)
        {
			//if (shaft.ManagerLocation.Manager == null) continue;
            _efficiencyFloors.Add(1f);
            _pawFloors.Add(shaft.CurrentDeposit.CurrentPaw);
        }

        double pawBonus = 0;
        var freeTime = offlineTime;

        _offlineBoosts = JsonConvert.DeserializeObject<List<OfflineBoost>>(PlayerPrefs.GetString("OfflineBoosts"));
        if (_offlineBoosts != null)
        {
            foreach (var offlineBoost in _offlineBoosts)
            {
                freeTime -= offlineBoost.time;
                if (freeTime <= 0)
                {
                    break;
                }
            }
        }

        if (freeTime > 0)
        {
            _offlineBoosts.Add(new OfflineBoost() { time = freeTime, location = ManagerLocation.Counter, index = -1, bonus = 1 });
        }
        Debug.Log("Free time: " + freeTime);

        foreach (var offlineBoost in _offlineBoosts)
        {
            if (offlineTime <= 0)
            {
                break;
            }

            float time = offlineBoost.time;
            float excuteTime = (offlineTime - time) > 0 ? time : offlineTime;
            offlineTime -= time;

            if (time <= 0)
            {
                continue;
            }

            for (int i = 0; i < _efficiencyFloors.Count; i++)
            {
                var managerBoost = _offlineBoosts.Find(x => x.location == ManagerLocation.Shaft && x.index == i);
                if (managerBoost == null)
                {
                    _efficiencyFloors[i] = 1f;
                }
                else
                {
                    _efficiencyFloors[i] = managerBoost.bonus;
                }

                _pawFloors[i] += ShaftManager.Instance.Shafts[i].GetPureEfficiencyPerSecond() * excuteTime * _efficiencyFloors[i];
            }

            //Elevator collection simulation
            var elevatorBoost = _offlineBoosts.Find(x => x.location == ManagerLocation.Elevator);
            if (elevatorBoost == null)
            {
                _efficiencyElevator = 1f;
            }
            else
            {
                _efficiencyElevator = elevatorBoost.bonus;
            }

            double q = 0d;
            for (int i = 0; i < _pawFloors.Count; i++)
            {
                //find i
                double n = excuteTime / ElevatorSystem.Instance.GetTempMoveTimeInCycle(i);
                q = 0d;
                for (int j = 0; j <= i; j++)
                {
                    var shaft = ShaftManager.Instance.Shafts[j];
                    q += _pawFloors[j];
                }
				Debug.Log("Calcute _totalPawTaken :" + ElevatorSystem.Instance.GetPureProductionInCycle() + "/" + n + "/" + _efficiencyElevator);
                _totalPawTaken = ElevatorSystem.Instance.GetPureProductionInCycle() * n * _efficiencyElevator;
                floorIndex = i;
                if (q >= _totalPawTaken)
                {
                    break;
                }
            }

            //Case
            for (int i = 0; i <= floorIndex; i++)
            {
			
				if (i == floorIndex)
                {
                    _pawFloors[i] -= _totalPawTaken;
                    _pawElevator += _totalPawTaken;
					if (_pawFloors[i] <= 0) _pawFloors[i] = 0;

				}
                else
                {
					
					_totalPawTaken -= _pawFloors[i];
                    _pawElevator += _pawFloors[i];
                    _pawFloors[i] = 0;
                }
            }
            //done here
            var counterBoost = _offlineBoosts.Find(x => x.location == ManagerLocation.Counter);
            if (counterBoost == null)
            {
                _efficiencyCounter = 1f;
            }
            else
            {
                _efficiencyCounter = counterBoost.bonus;
            }
            var couterPaw = Counter.Instance.GetPureEfficiencyPerSecond() * excuteTime * _efficiencyCounter;

			//rule if not have manager -> paw add = 0
			if (ElevatorSystem.Instance.ManagerLocation.Manager == null) _pawElevator = 0;
			if (Counter.Instance.ManagerLocation.Manager == null) couterPaw = 0;


			if (_pawElevator >= couterPaw)
            {
                pawBonus += couterPaw;
                _pawElevator -= couterPaw;
            }
            else
            {
                pawBonus += _pawElevator;
                _pawElevator = 0;
            }

            //temp turn off boost
            offlineBoost.bonus = 1;
        }

        Debug.Log("Paw bonus: " + pawBonus + " paw elevator: " + _pawElevator + " paw floors: " + _pawFloors.Sum());

        foreach (var shaft in ShaftManager.Instance.Shafts)
        {
			//change paw here
			//if (shaft.ManagerLocation.Manager == null) continue;
            shaft.CurrentDeposit.SetPaw(_pawFloors[shaft.shaftIndex]);
        }
        ElevatorSystem.Instance.ElevatorDeposit.SetPaw(_pawElevator);

        return new OfflineMoneyData(saveTime, pawBonus);
    }


    public async void TestPawBonus(float offlineTime)
    {
        GetOfflineData();
        var offlineData = await CaculateOfflinePaw(offlineTime);
        GameUI.Instance.OpenOffline(offlineData);
    }

    public double GetNSPaw()
    {
        double result = 0d;
        double shaftPaw = ShaftManager.Instance.GetTotalNS();
        //double elevatorPaw = ElevatorSystem.Instance.GetTotalNS();
        double elevatorPaw = ElevatorSystem.Instance.GetTotalNSVersion2();
        double couterPaw = Counter.Instance.GetTotalNS();
		//Debug.Log("NSPaw Shaft Power: " + shaftPaw);
		//Debug.Log("NSPaw Elevator Power: " + elevatorPaw);
		//Debug.Log("NSPaw Counter Power: " + couterPaw);
		if (shaftPaw > elevatorPaw)
        {
            result = elevatorPaw;
        }
        else
        {
            result = shaftPaw;
        }

        if (result > couterPaw)
        {
            result = couterPaw;
        }

        return result;
    }
}

class OfflineBoost
{
    public float time;
    public ManagerLocation location;
    public int index;
    public float bonus;
}

public class OfflineMoneyData
{
    public float time;
    public double paw;

    public OfflineMoneyData(float time, double paw)
    {
        this.time = time;
        this.paw = paw;
    }
}
