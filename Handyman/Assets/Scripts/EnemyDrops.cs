using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField]
    GameObject[] PossibleDrops;

    public GameObject GetDrop()
    {
        return PossibleDrops[Random.Range(0, PossibleDrops.Length - 1)];
    }
}
