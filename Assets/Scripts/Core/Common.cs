using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Common 
{

    /// <summary>
    /// Do actione affter N second
    /// </summary>
    /// <param name="time"></param>
    /// <param name="action"></param>
    /// <returns></returns>
   public static IEnumerator IeDoSomeThing(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }

    /// <summary>
    /// Convert second to minites & Second
    /// </summary>
    /// <param name="totalSeconds"></param>
    /// <returns>Input:130s -> Output: "2p10s"</returns>
    public static string ConvertSecondsToMinutes(float totalSeconds)
    {
        int totalSecondsInt = Mathf.FloorToInt(totalSeconds); // Chuy?n ??i float thành int

        if (totalSecondsInt < 60)
        {
            return $"{totalSecondsInt}s";
        }
        else
        {
            int minutes = totalSecondsInt / 60;
            int seconds = totalSecondsInt % 60;
            return $"{minutes}p{seconds}s";
        }       
    }
}
