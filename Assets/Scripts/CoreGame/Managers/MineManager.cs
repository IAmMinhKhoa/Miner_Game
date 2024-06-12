using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : Patterns.Singleton<MineManager>
{
    [Header("Prefab")]
    [SerializeField]
    private Mine minePrefab;
    [SerializeField]
    private float mineYOffset = 0.5f;
    [SerializeField]
    private System.Numerics.BigInteger currentCost = 500;

    [Header("Basement")]
    [SerializeField]
    public List<Mine> Mines = new();

    public System.Numerics.BigInteger CurrentCost => currentCost;

    public void AddMine()
    {
        Transform lastMine = Mines[^1].transform;
        Mine newMine = Instantiate(minePrefab, lastMine.position, Quaternion.identity);
        newMine.transform.localPosition += new Vector3(0, -mineYOffset, 0);

        Mines.Add(newMine);
    }
}
