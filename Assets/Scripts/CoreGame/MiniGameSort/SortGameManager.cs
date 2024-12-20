using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;


public class SortGameManager : MonoBehaviour
{
	public BoxInfo boxPrefab;
	private BoxInfo lastBoxWaiting;
	public List<tsInfo> tsPrefabs;
	public List<BoxInfo> allBoxList;
	public List<tsInfo> allTSList;
	public tsInfo emptyTS;
	public int[] colsCount;
	public List<Transform> spawnBoxList;
	public Transform boxParent;
	public List<tsInfo> spawnList;
	public Transform[] clawPosList;
	public Transform holdPos;
	public int clawPos;
	public GameObject clawObject, StartUI, EndUI;
	public Camera cameraGameSort;
	public float clawDelayTime, currentDelayTime;
	private Animator clawAnim;

	[SerializeField] private bool isPlaying = false;
	public bool isStopped = false;
	void Awake()
	{
		tsInfo[] tempList = Resources.LoadAll<tsInfo>("Prefabs/minigame_sort/newTraSua");
		foreach (tsInfo go in tempList)
		{
			if(go.id != -1) tsPrefabs.Add(go);
		}
		clawPos = 0;
		clawDelayTime = 8f;
		currentDelayTime = clawDelayTime;
		colsCount = new int[3];
		clawAnim = clawObject.transform.GetChild(0).GetComponent<Animator>();
	}

	private void Update()
	{
		if (isPlaying && !isStopped)
		{
			if(currentDelayTime > 0)
			{
				currentDelayTime -= Time.deltaTime;
			}
			else
			{
				currentDelayTime = clawDelayTime;
				DropLoop();
			}
		}
	}

	void StartDropper()
	{
		SpawnABox();
		clawObject.transform.DOLocalMoveY(5.06f, clawDelayTime / 3).SetEase(Ease.OutBack, 1f);
	}

	void DropLoop()
	{

		if(lastBoxWaiting != null)
		{
			clawAnim.SetTrigger("Dropping");
			lastBoxWaiting.DropThis();
			UpdateColAndCheck(clawPos, 1);

			clawObject.transform.DOLocalMoveY(7.67f, clawDelayTime / 6).SetEase(Ease.InBack, clawDelayTime / 6).OnComplete(() =>
			{
				SpawnABox();
				clawObject.transform.DOLocalMove(clawPosList[clawPos].position, 0f).SetDelay(0.1f).OnComplete(() =>
				{
					clawObject.transform.DOLocalMoveY(5.06f, clawDelayTime / 3).SetEase(Ease.OutBack, 1f);
				});
			});
		}

		//claw logic
		if (clawPos != 2) clawPos++;
		else clawPos = 0;
	}

	public void OnStartClick()
	{
		isPlaying = true;
		GetComponent<SortGameScore>().ResetScore();
		ClearAll();
		FirstSpawnBox();
		FirstSpawnTS();
		StartDropper();
		GetComponent<SkillsSortGameManager>().OnClickPlay();
	}

	void ClearAll()
	{
		clawPos = 0;
		clawObject.transform.position = clawPosList[0].position;
		for(int i = 0; i < 3; i++)
		{
			colsCount[i] = 0;
		}
		
		foreach(Transform go in boxParent)
		{
			Destroy(go.gameObject);
		}

		foreach(tsInfo ts in allTSList)
		{
			Destroy(ts.gameObject);
		}

		if (allBoxList != null) allBoxList.Clear();
		if (allTSList != null) allTSList.Clear();
	}

	void FirstSpawnBox()
	{
		for(int i = 0; i < 3; i++)
		{
			for (int z = 0; z < 3; z++)
			{
				BoxInfo newBox = Instantiate(boxPrefab, spawnBoxList[i].position, Quaternion.identity, boxParent);
				newBox.boxParent = boxParent;
				newBox.holdPos = holdPos;
				newBox.col = i;
				newBox.DropThis();
				allBoxList.Add(newBox);
				UpdateColAndCheck(i, 1);
			}
		}
	}

	void FirstSpawnTS()
	{
		int index1 = Random.Range(0, tsPrefabs.Count);
		int index2;
		do
		{
			index2 = Random.Range(0, tsPrefabs.Count);
		} while (index2 == index1);

		spawnList = new List<tsInfo>();

		while (spawnList.Count < 17)
		{
			spawnList.Add(tsPrefabs[Random.Range(0, tsPrefabs.Count)]);
		};

		foreach (tsInfo go in tsPrefabs)
		{
			try
			{
				go.gameObject.GetComponent<DragDrop>().cameraGameSort = cameraGameSort;
			
			}
			catch (System.Exception)
			{

				Debug.LogError("Can't get component DragDrop");
			}

			if (go.id == index1)
			{
				spawnList.Add(go);
				spawnList.Add(go);
				spawnList.Add(go);
			}
			if (go.id == index2)
			{
				spawnList.Add(go);
				spawnList.Add(go);
				spawnList.Add(go);
			}
		}
		spawnList.Add(emptyTS);
		spawnList.Add(emptyTS);
		spawnList.Add(emptyTS);
		spawnList.Add(emptyTS);

		BringTempToGame(spawnList);
		ClearEmptySlot();
		foreach(BoxInfo box in allBoxList)
		{
			box.UpdateBoxCount();
		}
	}

	void SpawnABox()
	{
		BoxInfo newBox = Instantiate(boxPrefab, spawnBoxList[clawPos].position, Quaternion.identity, boxParent);
		newBox.col = clawPos;
		newBox.boxParent = boxParent;
		newBox.holdPos = holdPos;
		lastBoxWaiting = newBox;

		
		tsInfo newTs1, newTs2, newTs3;
		List<int> randomNumbers = new List<int>();

		while (randomNumbers.Count < 3)
		{
			int newNumber = Random.Range(0, tsPrefabs.Count);
			if (!randomNumbers.Contains(newNumber))
			{
				randomNumbers.Add(newNumber);
			}
		}

		newTs1 = Instantiate(tsPrefabs[randomNumbers[0]]);
		newTs2 = Instantiate(tsPrefabs[randomNumbers[1]]);
		newTs3 = Instantiate(tsPrefabs[randomNumbers[2]]);

		//add curent camera to bottle
		try
		{
			newTs1.gameObject.GetComponent<DragDrop>().cameraGameSort = cameraGameSort;
			newTs2.gameObject.GetComponent<DragDrop>().cameraGameSort = cameraGameSort;
			newTs3.gameObject.GetComponent<DragDrop>().cameraGameSort = cameraGameSort;
		}
		catch (System.Exception)
		{

			Debug.LogError("Can't get component DragDrop");
		}


		allTSList.Add(newTs1);
		allTSList.Add(newTs2);
		allTSList.Add(newTs3);

		newTs1.UpdateBoxParent(0, newBox.gameObject);
		newTs2.UpdateBoxParent(1, newBox.gameObject);
		newTs3.UpdateBoxParent(2, newBox.gameObject);
	}

	public void RecheckClawPosition()
	{
		clawObject.transform.DOLocalMoveY(7.67f, 0f);
		clawObject.transform.DOLocalMove(clawPosList[clawPos].position, 0f);
	}

	void ClearEmptySlot()
	{
		foreach(tsInfo go in allTSList)
		{
			if(go.id == -1)
			{
				go.ClearThis();
			}
		}
	}

	void BringTempToGame(List<tsInfo> listTemp)
	{
		for (int i = 0; i < 9; i++)
		{
			tsInfo newTs1, newTs2, newTs3;
			List<int> randomNumbers = new List<int>();

			do
			{
				while (randomNumbers.Count < 3)
				{
					if(listTemp.Count > 3)
					{
						int newNumber = Random.Range(0, listTemp.Count);
						if (!randomNumbers.Contains(newNumber))
						{
							randomNumbers.Add(newNumber);
						}
					}
					else
					{
						for(int x =  0; x < listTemp.Count; x++)
						{
							randomNumbers.Add(x);
						}
					}
				}

				newTs1 = listTemp[randomNumbers[0]];
				newTs2 = listTemp[randomNumbers[1]];
				newTs3 = listTemp[randomNumbers[2]];

			} while (newTs1.id == newTs2.id && newTs2.id == newTs3.id);

			newTs1 = Instantiate(newTs1);
			newTs2 = Instantiate(newTs2);
			newTs3 = Instantiate(newTs3);

			newTs1.gameObject.layer = LayerMask.NameToLayer("MinigameScene");
			newTs2.gameObject.layer = LayerMask.NameToLayer("MinigameScene");
			newTs3.gameObject.layer = LayerMask.NameToLayer("MinigameScene");

			allTSList.Add(newTs1);
			allTSList.Add(newTs2);
			allTSList.Add(newTs3);

			newTs1.UpdateBoxParent(0, allBoxList[i].gameObject);
			newTs2.UpdateBoxParent(1, allBoxList[i].gameObject);
			newTs3.UpdateBoxParent(2, allBoxList[i].gameObject);

			randomNumbers.Sort((a, b) => b.CompareTo(a));

			foreach (int index in randomNumbers)
			{
				listTemp.RemoveAt(index);
			}

			Debug.Log(listTemp.Count);
			allBoxList[i].UpdateBoxCount();
		}

	}

	public void AdjustClawDelayTime(float newValue)
	{
		clawDelayTime *= newValue;
	}

	public void UpdateColAndCheck(int col, int value)
	{
		colsCount[col] += value;
		if(colsCount[col] > 6)
		{
			EndGame();
		}
	}

	void EndGame()
	{
		isPlaying = false;
		clawPos = 0;
		clawDelayTime = 8f;
		FindObjectOfType<SortGameScore>().CheckSetHighScore();
		EndUI.SetActive(true);
		ClearAll();
	}

	public void AdjustAllDragCode(bool value)
	{
		foreach(tsInfo t in allTSList)
		{
			if(t != null)
			{
				t.gameObject.GetComponent<DragDrop>().enabled = value;
			}
		}
	}

	BoxInfo GetRandomBoxInGame()
	{
		return allBoxList[Random.Range(0, allBoxList.Count)];
	}
	public void OnClickBack()
	{
		SceneManager.UnloadScene("MinigameSort");
	}

	public void OnClickPause()
	{
		isStopped = !isStopped;
		AdjustAllDragCode(!isStopped);
	}
}
