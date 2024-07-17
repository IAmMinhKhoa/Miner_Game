using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineManager : Patterns.Singleton<OfflineManager>
{
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

        PawManager.Instance.Save();
    }
}
