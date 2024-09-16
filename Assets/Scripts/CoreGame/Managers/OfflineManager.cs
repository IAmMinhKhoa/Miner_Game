using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFabManager.Data;
using Cysharp.Threading.Tasks;
using System.Linq;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
public class OfflineManager : Patterns.Singleton<OfflineManager>
{
    #region ----Caculate attribute----
    List<double> _efficiencyFloors = new();
    List<double> _pawFloors = new();
    double _efficiencyElevator = 0;
    double _pawElevator = 0;
    double _efficiencyCounter = 0;
    List<OfflineBoost> _offlineBoosts = new();
    #endregion
    private bool isDone = false;
    public bool IsDone => isDone;
    // private void OnApplicationPause(bool pause)
    // {
    //     if (pause)
    //     {
    //         Save();
    //     }
    // }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        Save();
        PlayFabDataManager.Instance.SendDataBeforeExit().Forget();

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
        //SkinManager.Instance.Save();
        PlayFabDataManager.Instance.SaveData("LastTimeQuit", System.DateTime.Now.ToString());

    }

    public void LoadOfflineData()
    {
        PlayFabDataManager.Instance.GoToMainGame();
        string lastTimeQuit = PlayFabDataManager.Instance.GetData("LastTimeQuit");
        if (string.IsNullOrEmpty(lastTimeQuit))
        {
            isDone = true;
            return;
        }

        System.DateTime lastTime = System.DateTime.Parse(lastTimeQuit);
        System.TimeSpan timeSpan = System.DateTime.Now - lastTime;
        double seconds = timeSpan.TotalSeconds;
        CalculateManagerCooldown(seconds);
        double offlinePaw = PawBonus(seconds);
        // update ADS double up or something here
        PawManager.Instance.AddPaw(offlinePaw);

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
        _efficiencyFloors.Clear();
        _pawFloors.Clear();
        _offlineBoosts.Clear();

        foreach (var shaft in ShaftManager.Instance.Shafts)
        {
            _efficiencyFloors.Add(shaft.GetPureEfficiencyPerSecond());
            _pawFloors.Add(shaft.CurrentDeposit.CurrentPaw);

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

        // _efficiencyElevator = ElevatorSystem.Instance.EfficiencyBoost;
        // _pawElevator = ElevatorSystem.Instance.ElevatorDeposit.CurrentPaw;
        // if (ElevatorSystem.Instance.ManagerLocation.Manager != null)
        // {
        //     OfflineBoost offlineBoost = new OfflineBoost();
        //     offlineBoost.time = ElevatorSystem.Instance.ManagerLocation.Manager.CurrentBoostTime;
        //     offlineBoost.location = ManagerLocation.Elevator;
        //     offlineBoost.index = 0;
        //     _offlineBoosts.Add(offlineBoost);
        // }

        // _efficiencyCounter = Counter.Instance.EfficiencyBoost;
        // if (Counter.Instance.ManagerLocation.Manager != null)
        // {
        //     OfflineBoost offlineBoost = new OfflineBoost();
        //     offlineBoost.time = Counter.Instance.ManagerLocation.Manager.CurrentBoostTime;
        //     offlineBoost.location = ManagerLocation.Counter;
        //     offlineBoost.index = 0;
        //     _offlineBoosts.Add(offlineBoost);
        // }

        _offlineBoosts.Sort((x, y) => x.time.CompareTo(y.time));
        Debug.Log("Paw bonus: " + JsonConvert.SerializeObject(_offlineBoosts));
    }

    private double CaculateOfflinePaw(float offlineTime)
    {
        double pawBonus = 0;

        foreach (var offlineBoost in _offlineBoosts)
        {
            if (offlineTime <= 0)
            {
                break;
            }

            float time = offlineBoost.time;
            float excuteTime = (offlineTime - time) > 0 ? time : offlineTime;

            if (time <= 0)
            {
                continue;
            }

            if (offlineBoost.location == ManagerLocation.Shaft)
            {
                for (int i = 0; i < _efficiencyFloors.Count; i++)
                {
                    if (i == offlineBoost.index)
                    {
                        _pawFloors[i] += _efficiencyFloors[i] * offlineBoost.bonus * excuteTime;
                    }
                    else
                    {
                        var managerBoost = _offlineBoosts.Find(x => x.location == ManagerLocation.Shaft && x.index == i);
                        if (managerBoost == null)
                        {
                            _pawFloors[i] += _efficiencyFloors[i] * excuteTime;
                        }
                        else
                        {
                            _pawFloors[i] += _efficiencyFloors[i] * managerBoost.bonus * excuteTime;
                        }
                    }
                }

                _offlineBoosts.Remove(offlineBoost);
            }
        }

        pawBonus += _pawFloors.Sum();

        return pawBonus;
    }


    public void TestPawBonus(float offlineTime)
    {
        GetOfflineData();
        double pawBonus = CaculateOfflinePaw(offlineTime);
        Debug.Log("Paw bonus: " + pawBonus);
    }
}

class OfflineBoost
{
    public float time;
    public ManagerLocation location;
    public int index;
    public float bonus;
}
