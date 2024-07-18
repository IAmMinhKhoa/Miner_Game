using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineManager : Patterns.Singleton<OfflineManager>
{
    private bool isDone = false;
    public bool IsDone => isDone;
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Save();
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        Save();
    }

    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            string lastTimeQuit = PlayerPrefs.GetString("LastTimeQuit");
            if (!string.IsNullOrEmpty(lastTimeQuit))
            {
                System.DateTime lastTime = System.DateTime.Parse(lastTimeQuit);
                System.TimeSpan timeSpan = System.DateTime.Now - lastTime;
                Debug.Log("Time span: " + timeSpan.TotalSeconds);
            }
        }
    }

    private void Save()
    {
        PlayerPrefs.SetString("LastTimeQuit", System.DateTime.Now.ToString());
        ShaftManager.Instance.Save();
        ElevatorSystem.Instance.Save();
        Counter.Instance.Save();
        ManagersController.Instance.Save();

        PawManager.Instance.Save();
    }

    public void LoadOfflineData()
    {
        string lastTimeQuit = PlayerPrefs.GetString("LastTimeQuit");
        System.DateTime lastTime = System.DateTime.Parse(lastTimeQuit);
        System.TimeSpan timeSpan = System.DateTime.Now - lastTime;
        double seconds = timeSpan.TotalSeconds;

        CalculateManagerCooldown(seconds);
        double offlinePaw = PawBonus(seconds);
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
    }

    private double PawBonus(double seconds)
    {
        double pawBonus = 0;
        return pawBonus;
    }
}
