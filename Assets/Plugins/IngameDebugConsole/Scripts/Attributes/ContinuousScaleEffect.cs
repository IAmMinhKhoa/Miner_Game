using UnityEngine;
using DG.Tweening;

public class ContinuousScaleEffect : MonoBehaviour
{
	// Kích thước nhỏ nhất và lớn nhất
	public Vector3 minScale = new Vector3(0.8f, 0.8f, 0.8f);
	public Vector3 maxScale = new Vector3(1.2f, 1.2f, 1.2f);
	// Thời gian phóng to/thu nhỏ
	

	private void Start()
	{
		// Tạo hiệu ứng phóng to thu nhỏ liên tục
		transform.DOScale(maxScale, 0.25f)
				 .SetEase(Ease.InOutSine) // Tùy chọn easing (mềm mại)
				 .SetLoops(-1, LoopType.Yoyo); // Lặp vô hạn và chuyển đổi Yoyo (qua lại)
	}
}
