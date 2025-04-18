using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Serialization;

public class TransportMachineCounter: MonoBehaviour
{
#region Prefab and Points
	[Header("Refactor V2")]
	[SerializeField] private GameObject _prefabCake;    // Prefab của bánh
	[SerializeField] private Transform _startPoint;     // Điểm bắt đầu
	[SerializeField] private Transform _endPoint;       // Điểm kết thúc
	#endregion

	#region Config and Shaft
	public TransportConfig Config => CurrentCounter.Config;  // Lấy config từ Shaft
	 public Counter CurrentCounter;                          // Shaft hiện tại
	#endregion

	#region State Management
	[SerializeField] private bool isWorking = false;    // Trạng thái làm việc
	public bool IsWorking => isWorking;                 // Property đọc trạng thái làm việc

	public bool forceWorking = false;  // Cờ để buộc làm việc
	#endregion

	#region Product Value and Speed
	// Giá trị của mỗi bánh (sản phẩm)
	public double ValueProduct
	{
		get
		{
			return Config.Value * CurrentCounter.EfficiencyBoost;
		}
	}

	// Tốc độ sản xuất bánh (số bánh được sản xuất mỗi giây)
	public double ProductPerSecond
	{
		get
		{
			// Tính toán tốc độ sản xuất, chia cho tất cả các yếu tố ảnh hưởng
			return Config.ProductPerSecond / CurrentCounter.ScaleBakingTime / CurrentCounter.SpeedBoost / CurrentCounter.GetGlobalBoost();
		}
	}
	private List<GameObject> cakeObjects = new List<GameObject>();
	#endregion

	#region Element UI

	[SerializeField] private SkeletonAnimation _skeletonConveyor;
	[SerializeField] private SkeletonAnimation _skeletonTable;
	[SerializeField] private SkeletonAnimation _skeletonCupboard;
	#endregion
	private void Start()
	{
		BYPool poolCake= new BYPool();
		//tạo pool cake ở đây -> tái sử dụng object
		poolCake.name_pool = "PoolCake_Counter";
		poolCake.parentSpawm = this.transform;
		poolCake.preFab = _prefabCake.transform;
		poolCake.index = -1;
		poolCake.total = 5;

		PoolManager.Instance.pool_defaults.Add(poolCake);
		PoolManager.Instance.InitPool();


	}

	private void Update()
	{

		if (!isWorking)
		{

			if (CurrentCounter.ManagerLocation.Manager != null)
			{
				forceWorking = true;
			}

			if (forceWorking)
			{
				isWorking = true;
				forceWorking = false;

				StartCoroutine(SpawmCakePerSecond());
			}
			if(cakeObjects.Count<=0) SetAnimation(AnimationState.Idle);
		}
	}


	[Button]
	private void MachineTransport(float time=10f)
	{
		// Gọi Coroutine để spawn bánh trong 2 giây
		StartCoroutine(SpawnCakesForDuration(time)); // 2 giây spawn bánh
	}

	public IEnumerator SpawnCakesForDuration(float duration) //dùng cho case click vào tầng để chạy thủ công trong 2 giây
	{
		if (isWorking) yield break;
		SetAnimation(AnimationState.Active);
		float elapsedTime = 0f;
		isWorking = true;
		Debug.Log("start flow creat cake");
		while (elapsedTime < duration)
		{
			SpawnAndMoveCake();  // Gọi hàm spawn bánh
			elapsedTime += (float)ProductPerSecond;  // Cộng thêm thời gian giữa các lần spawn
			yield return new WaitForSeconds((float)ProductPerSecond); // Chờ theo CakePerSecond trước khi spawn bánh tiếp theo
		}


		isWorking = false;

	}

	private IEnumerator SpawmCakePerSecond()
	{
		SetAnimation(AnimationState.Active);
		isWorking = true;
		SpawnAndMoveCake();
		yield return new WaitForSeconds((float)ProductPerSecond);
		isWorking = false;
		if (CurrentCounter.ManagerLocation.Manager == null)
		{
			SetAnimation(AnimationState.Idle);
		}
	}
	[Button]
	private void SpawnAndMoveCake()
	{
		// Spawn Cake tại vị trí _startPoint
		Transform cake = PoolManager.Instance.dic_pool["PoolCake_Counter"].Spawned();
		cake.transform.position = _startPoint.position;
		cakeObjects.Add(cake.gameObject);
		//Điều chỉnh tốc độ băng chuyền theo tốc độ sản xuất
		AdjustSpeedConveyor();
		//Multiply by 3 to ensure there are always 3 cakes in the transport machine
		cake.transform.DOMove(_endPoint.position, (float)ProductPerSecond*3).SetEase(Ease.Linear).OnComplete(() =>
		{
			Deposit();
			cakeObjects.Remove(cake.gameObject);
			PoolManager.Instance.dic_pool["PoolCake_Counter"].DesSpawned(cake);
		});
	}

	private void AdjustSpeedConveyor()
	{
		float conveyorSpeed =  1 / (float)ProductPerSecond;
		if (conveyorSpeed < 1) conveyorSpeed = 1;
		_skeletonConveyor.timeScale = conveyorSpeed;
	}
	private void Deposit()
	{
		//CurrentCounter.CurrentDeposit.AddPaw(ValueProduct);
		PawManager.Instance.AddPaw(ValueProduct);
	}

	#region SUPPORT
	public void SetAnimation(AnimationState state)
	{
		// Kích hoạt animation cho _skeletonConveyor
		SetSkeletonAnimation(_skeletonConveyor, state);

		// Kích hoạt animation cho _skeletonTable
		SetSkeletonAnimation(_skeletonTable, state);

		// Kích hoạt animation cho _skeletonCupboard
		SetSkeletonAnimation(_skeletonCupboard, state);
	}
	private void SetSkeletonAnimation(SkeletonAnimation skeleton, AnimationState state)
	{
		if (skeleton != null)
		{
			// Lấy animation hiện tại trên track 0
			TrackEntry currentAnimation = skeleton.state.GetCurrent(0);

			// Kiểm tra nếu animation hiện tại không phải animation bạn muốn
			if (currentAnimation == null || currentAnimation.Animation.Name != state.ToString())
			{
				// Nếu không phải animation hiện tại, chạy animation mới
				switch (state)
				{
					case AnimationState.Active:
						// Chạy animation "Active"
						skeleton.state.SetAnimation(0, "Active", true);
						break;
					case AnimationState.Idle:
						// Chạy animation "Idle"
						skeleton.state.SetAnimation(0, "Idle", true);
						skeleton.state.GetCurrent(0).MixDuration = 0f;

						break;
					default:
						Debug.LogWarning("Unknown animation state");
						break;
				}
			}
			else
			{
//				Debug.Log("Animation is already playing: " + state.ToString());
			}
		}
	}

	#endregion
}
