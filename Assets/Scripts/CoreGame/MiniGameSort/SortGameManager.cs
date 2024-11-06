using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortGameManager : MonoBehaviour
{
	public BoxInfo boxPrefab;
	public List<tsInfo> tsPrefabs;
	public List<BoxInfo> allBoxList;
	public List<tsInfo> allTSList;
	public List<BoxInfo> col1, col2, col3;
	public List<Transform> spawnBoxList;
	public Transform boxParent;
	List<tsInfo> spawnList;

	void Awake()
	{
		tsInfo[] tempList = Resources.LoadAll<tsInfo>("Prefabs/minigame_sort/TraSua");
		foreach (tsInfo go in tempList)
		{
			tsPrefabs.Add(go);
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

		//BringTempToGame(spawnList); xiu nua sua lai
	}

	void BringTempToGame(List<tsInfo> listTemp)
	{
		int count = 0;
		do
		{
			BoxInfo chosenBox = GetRandomBoxInGame();
			tsInfo current = GetRandomFromTempList();
			int[] a = new int[3];
			for (int i = 0; i <= 2; i++)
			{
				a[i] = chosenBox.GetComponent<tsInfo>() != null ? chosenBox.objects[i].GetComponent<tsInfo>().id : -1;
			}

			bool isOkay = false;
			int slotToPut = -1;
			for (int i = 0; i <= 2; i++)
			{
				if (a[i] == -1)
				{
					isOkay = true;
					slotToPut = i;
				}
			}

			if (a[0] != a[1] && a[1] != a[2] && a[2] != a[0] && isOkay)
			{
				current.GetComponent<DragDrop>().UpdateBoxParent(slotToPut, chosenBox.gameObject);
				allTSList.Add(current);
				listTemp.Remove(current);
				count++;
			}
		} while (count < 23);
	}

	BoxInfo GetRandomBoxInGame()
	{
		return allBoxList[Random.Range(0, allBoxList.Count)];
	}

	tsInfo GetRandomFromTempList()
	{
		return spawnList[Random.Range(0, allBoxList.Count)];
	}

}
