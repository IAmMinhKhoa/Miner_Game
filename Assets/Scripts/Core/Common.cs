using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Networking;

#region Shaft Enum Skin
public enum SkinShaftBg
{

    BR1 = 1,
    BR2 = 2
}
public enum SkinShaftWaitTable
{

    WT1 = 1,
    WT2 = 2
}
public enum SkinShaftMilkCup
{
    MC1 = 1,
    MC2 = 2
}
#endregion

public static class Common 
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

    public static IEnumerator FadeOut(CanvasGroup canvasGroup, float duration = 0.5f)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public static IEnumerator FadeIn(CanvasGroup canvasGroup, float duration = 0.5f)
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1;
    }


	public static bool CheckInternetConnection()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable)
		{
			//Debug.LogWarning("No internet connection detected.");
			return false;
		}

		//Debug.Log("Internet connection is available.");
		return true;

	}
	public  static string Text(this LanguageKeys key, object[] parameters = null)
	{
		return LocalizationManager.GetLocalizedString(key, parameters: parameters);
	}
	public static bool CheckDevice()
	{

		if(SystemInfo.deviceName.Contains("iPad")|| SystemInfo.deviceModel.Contains("iPad"))
		{
		return true;
		}
		return false;

	}
}
