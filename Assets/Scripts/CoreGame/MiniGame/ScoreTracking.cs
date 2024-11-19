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
				DontDestroyOnLoad(go); // Giữ đối tượng này khi chuyển cảnh
			}
			return instance;
		}
	}

	public void TrackEvent(TrackingEventType eventType, float content)
	{

		var eventData = new Dictionary<string, object>
		{
			{ "EventType", eventType.ToString() },
			{ "Score", content }
		};

		var request = new WriteClientPlayerEventRequest
		{
			EventName = eventType.ToString(),
			Body = eventData
		};

		PlayFabClientAPI.WritePlayerEvent(request, OnEventTracked, OnEventError);
	}

	private void OnEventTracked(WriteEventResponse response)
	{
		Debug.Log("Sự kiện đã được ghi nhận thành công!");
	}

	private void OnEventError(PlayFabError error)
	{
		Debug.LogError("Lỗi khi ghi nhận sự kiện: " + error.ErrorMessage);
	}
}


public enum TrackingEventType
{
	SortGameComplete,
	MergeGameComplete
}
