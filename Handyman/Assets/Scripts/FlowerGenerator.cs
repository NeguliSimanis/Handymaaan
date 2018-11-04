using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] flowerPatches;

    [SerializeField]
    Transform[] flowerPatchLocations;

    float probabilityOfNoFlower = 0.7f;

	void Start ()
    {
        foreach (Transform location in flowerPatchLocations)
        {
            if (Random.Range(0f, 1f) >= probabilityOfNoFlower)
            {
                Debug.Log("spawning flower");
                Instantiate(flowerPatches[Random.Range(0, flowerPatches.Length - 1)], location);
            }
        }
	}
	
}
