using UnityEngine;
using TMPro;

public class FPSDisplay : MonoBehaviour
{
	[Header("UI Text to display FPS")]
	[SerializeField] private TextMeshProUGUI fpsText;

	private int frameCount;
	private float elapsedTime;
	private const float updateRate = 0.5f; 

	private void Awake()
	{
		// Ép cố định 60 FPS và tắt VSync
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
	}

	private void Update()
	{
		frameCount++;
		elapsedTime += Time.unscaledDeltaTime;

		if (elapsedTime >= updateRate)
		{
			float fps = frameCount / elapsedTime;
			string color = fps switch
			{
				>= 60 => "#00FF00", 
				>= 30 => "#C900FF", 
				_ => "#FF0000"  
			};

			fpsText.text = $"<color={color}><size=80%>FPS:</size> {fps:0}</color>";
			frameCount = 0;
			elapsedTime = 0f;
		}
	}
}
