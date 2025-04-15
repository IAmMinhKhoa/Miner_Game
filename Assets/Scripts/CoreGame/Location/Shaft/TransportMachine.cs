using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class TransportMachine : MonoBehaviour
{
	[Header("Refactor V2")]
	public GameObject _prefabCake; // prefab của cake
	public Transform _startPoint;  // Điểm bắt đầu
	public Transform _endPoint;    // Điểm kết thúc

	private bool isCreatedPool = false;
	//config
	private CakeConfig Config=>CurrentShaft.Config;
	//----
	[SerializeField] private bool isWorking = false;
	public bool IsWorking => isWorking;


	public bool forceWorking = false;
	public Shaft CurrentShaft;

	//--value capacity & amount product cake
	public double ValueProduct //Giá trị mỗi bánh
	{
		get
		{
	//		Debug.Log("khoa valueproduct :"+ Config.Value*CurrentShaft.ScaleCakeValue);
			return Config.Value*CurrentShaft.EfficiencyBoost;
		}
	}

	public double ProductPerSecond //Tốc độ sản xuất bánh (cứ <ProductPerSecond> giây sẽ tạo 1 bánh)
	{
		get
		{

			return  Config.ProductPerSecond /CurrentShaft.ScaleBakingTime/ CurrentShaft.SpeedBoost/CurrentShaft.GetGlobalBoost();
		}
	}

	private void Start()
	{
		BYPool poolCake= new BYPool();
		//tạo pool cake ở đây -> tái sử dụng object
		poolCake.name_pool = "PoolCake_Shaft_"+CurrentShaft.shaftIndex;
		poolCake.parentSpawm = this.transform;
		poolCake.preFab = _prefabCake.transform;
		poolCake.index = -1;
		poolCake.total = 5;

		PoolManager.Instance.pool_defaults.Add(poolCake);
		PoolManager.Instance.InitPool();
		isCreatedPool = true;

	}

	private void Update()
	{
		if (!isCreatedPool) return;
		if (!isWorking)
		{
			if (CurrentShaft.ManagerLocation.Manager != null)
			{
				forceWorking = true;
			}

			if (forceWorking)
			{
				isWorking = true;
				forceWorking = false;
				StartCoroutine(SpawmCakePerSecond());
			}
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
		isWorking = true;
		//SpawnAndMoveCake();
		yield return new WaitForSeconds((float)ProductPerSecond);
		isWorking = false;
	}
	[Button]
	private void SpawnAndMoveCake()
	{
		// Spawn Cake tại vị trí _startPoint
		//GameObject cake = Instantiate(_prefabCake, _startPoint.position, Quaternion.identity);
		Transform cake = PoolManager.Instance.dic_pool["PoolCake_Shaft_"+CurrentShaft.shaftIndex].Spawned();
		cake.transform.position = _startPoint.position;
		// Di chuyển Cake từ _startPoint đến _endPoint bằng DOTween
		//Multiply by 3 to ensure there are always 3 cakes in the transport machine
		cake.transform.DOMove(_endPoint.position, (float)ProductPerSecond*3).SetEase(Ease.Linear).OnComplete(() =>
		{
			Deposit();
			//Destroy(cake); // Destroy khi cake di chuyển xong
			PoolManager.Instance.dic_pool["PoolCake_Shaft_"+CurrentShaft.shaftIndex].DesSpawned(cake);
		});
	}

	private void Deposit()
	{
		CurrentShaft.CurrentDeposit.AddPaw(ValueProduct);
	}

}
