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
public enum ScreenAspectRatio
{
	unkown = 0,
	iPadRatio4_3,
	StandardRatio16_9,
	WideRatio195_9
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

	public static ScreenAspectRatio GetAspectRatio()
	{

		var screenRatio = (float)Screen.width / Screen.height;
		Debug.Log(Screen.width + "x" + Screen.height+"->"+screenRatio);

		const float tabletRatio = 4f / 3f; // Tỷ lệ cho tablet
		const float standardRatio = 16f / 9f; // Tỷ lệ cho màn hình chuẩn
		const float wideRatio = 19.5f / 9f; // Tỷ lệ cho màn hình rộng

		// Kiểm tra tỷ lệ 4:3 cho tablet
		if (screenRatio >= tabletRatio - 0.05f && screenRatio <= tabletRatio + 0.05f)
		{
			return ScreenAspectRatio.iPadRatio4_3;
		}
		// Kiểm tra tỷ lệ 16:9 cho màn hình chuẩn
		else if (screenRatio >= standardRatio - 0.05f && screenRatio <= standardRatio + 0.05f)
		{
			return ScreenAspectRatio.StandardRatio16_9;
		}
		// Kiểm tra tỷ lệ 19.5:9 cho màn hình rộng
		else if (screenRatio >= wideRatio - 0.05f && screenRatio <= wideRatio + 0.05f)
		{
			return ScreenAspectRatio.WideRatio195_9;
		}
		else
		{
			return ScreenAspectRatio.unkown; // Nếu không khớp với bất kỳ tỷ lệ nào
		}
	}
	public static bool IsTablet
	{
		get
		{
#if UNITY_IOS
//hiện ta chỉ mới detect thiết bị IPAD và IPHONE (CẦN THEO Tỉ lệ màn hình)
            bool deviceIsIpad = UnityEngine.iOS.Device.generation.ToString().Contains("iPad");
            if (deviceIsIpad)
            {
                return true; // Trả về true nếu là iPad (tablet)
            }

            bool deviceIsIphone = UnityEngine.iOS.Device.generation.ToString().Contains("iPhone");
            if (deviceIsIphone)
            {
                return false; // Trả về false nếu là iPhone (không phải tablet)
            }
#endif
#if UNITY_EDITOR
		return false; //debug (để theo ý mình để reponsive theo ý
#endif
			return false; // Nếu không phải iOS, trả về false
		}
	}
}
