using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortGameManager : MonoBehaviour
{
	public BoxInfo boxPrefab;
	public List<tsInfo> tsPrefabs;
	public List<BoxInfo> allBoxList;
	public List<tsInfo> allTSList;
	public tsInfo emptyTS;
	public List<BoxInfo> col1, col2, col3;
	public List<Transform> spawnBoxList;
	public Transform boxParent;
	public List<tsInfo> spawnList;

	void Awake()
	{
		tsInfo[] tempList = Resources.LoadAll<tsInfo>("Prefabs/minigame_sort/TraSua");
		foreach (tsInfo go in tempList)
		{
			if(go.id != -1) tsPrefabs.Add(go);
		}
	}

	public void OnStartClick()
	{
		FirstSpawnBox();
		FirstSpawnTS();
	}

	void FirstSpawnBox()
	{
		for(int i = 0; i < 3; i++)
		{
			for (int z = 0; z < 3; z++)
			{
				BoxInfo newBox = Instantiate(boxPrefab, spawnBoxList[i].position, Quaternion.identity, boxParent);
				switch (i)
				{
					case 0: col1.Add(newBox); break;
					case 1: col2.Add(newBox); break;
					case 2: col3.Add(newBox); break;
				}
				allBoxList.Add(newBox);
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
			if(go.id == index1)
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
					int newNumber = Random.Range(0, listTemp.Count);
					if (!randomNumbers.Contains(newNumber))
					{
						randomNumbers.Add(newNumber);
					}
				}

				newTs1 = listTemp[randomNumbers[0]];
				newTs2 = listTemp[randomNumbers[1]];
				newTs3 = listTemp[randomNumbers[2]];

			} while (newTs1.id == newTs2.id && newTs2.id == newTs3.id);

			newTs1 = Instantiate(newTs1);
			newTs2 = Instantiate(newTs2);
			newTs3 = Instantiate(newTs3);

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

	BoxInfo GetRandomBoxInGame()
	{
		return allBoxList[Random.Range(0, allBoxList.Count)];
	}
}
