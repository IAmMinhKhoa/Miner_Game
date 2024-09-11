using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFabManager.Data;
using Cysharp.Threading.Tasks;
public class OfflineManager : Patterns.Singleton<OfflineManager>
{
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
}
