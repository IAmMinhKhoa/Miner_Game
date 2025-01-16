using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTracking : MonoBehaviour
{
	private static ScoreTracking instance;

	public static ScoreTracking Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject go = new GameObject("ScoreTracking");
				instance = go.AddComponent<ScoreTracking>();
				DontDestroyOnLoad(go);
			}
			return instance;
		}
	}

	public void TrackEvent(TrackingEventType eventType, float content)
	{

		PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
		{
			
			Statistics = new List<StatisticUpdate> {
		new StatisticUpdate { StatisticName = eventType.ToString(), Value = (int)content },
	}
		},
		result => { Debug.Log("User statistics updated"); },
		error => { Debug.LogError(error.GenerateErrorReport()); });
	}
}


public enum TrackingEventType
{
	SortGameComplete,
	MergeGameComplete
}
