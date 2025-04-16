using UnityEngine;

public class SymbolSlot : MonoBehaviour
{
	public float speed = 300f;
	public float loopHeight = 1200f;
	private bool isMoving = true;

	public void SetSpeed(float s)
	{
		speed = s;
		isMoving = speed > 0f;
	}

	void Update()
	{
		if (!isMoving) return;

		transform.localPosition -= new Vector3(0, speed * Time.deltaTime, 0);

		if (transform.localPosition.y < -loopHeight / 2f)
		{
			transform.localPosition += new Vector3(0, loopHeight, 0);
		}
	}
}
