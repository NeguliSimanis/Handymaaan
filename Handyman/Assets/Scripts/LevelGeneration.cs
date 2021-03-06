﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {
    #region SPAWNIGN ENEMIES
    [SerializeField]
    bool canSpawnEnemies;

    [SerializeField]
    GameObject[] levelSegmentsWithEnemies;

    int segmentsUntilNextEnemySpawn;
    int default_segmentsUntilNextEnemySpawn = 3;
    #endregion

    [SerializeField]
    Transform player;

    [SerializeField]
    GameObject[] defaultLevelSegment;

    [SerializeField]
    Transform leftVisibilityPoint;
    [SerializeField]
    Transform leftGenerationPoint;

    [SerializeField]
    Transform rightVisibilityPoint;
    [SerializeField]
    Transform rightGenerationPoint;

    [SerializeField] float maxAllowedDistanceToRight = 26.73777f; // if you move further away, level generator object instantiates a new level segment and moves closer
    [SerializeField] float maxAllowedDistanceToLeft = 12.97923f; // -''-

    private float levelSegmentWidth;
    private float incrementLength;
    [SerializeField] float incrementMultiplier = 0.9f;

  /* #region MOUNTAINS
    [Header("MOUNTAINS")]
    [SerializeField]
    GameObject[] defaultMountainSegment;

    [SerializeField]
    Transform rightMountainGenerationPoint;

    [SerializeField]
    Transform leftMountainGenerationPoint;
    #endregion*/

    void Start ()
    {
        // Debug.Log("distance to right point " + (rightGenerationPoint.position.x - transform.position.x)); // 26.73777
        // Debug.Log("distance to left point " + (transform.position.x - leftGenerationPoint.position.x)); // 12.97923
        levelSegmentWidth = defaultLevelSegment[0].transform.GetChild(0).gameObject.GetComponent<BoxCollider2D>().size.x;
        incrementLength = incrementMultiplier * levelSegmentWidth;
        segmentsUntilNextEnemySpawn = default_segmentsUntilNextEnemySpawn + 2;
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("distance to right point " + (rightGenerationPoint.position.x - transform.position.x)); // 26.73777
        //Debug.Log("distance to left point " + (transform.position.x - leftVisibilityPoint.position.x)); // 12.97923

        // check distance to left point
        if (transform.position.x - leftVisibilityPoint.position.x > maxAllowedDistanceToLeft)
        {
            GenerateLeftSegment();
        }

        // check distance to right point    
        if (rightVisibilityPoint.position.x - transform.position.x > maxAllowedDistanceToRight)
        {
            GenerateRightSegment();
        }
    }

    /// <summary>
    /// Generates a new level segment to the left of the player and moves the LevelGenerator more to the left
    /// </summary>
    void GenerateLeftSegment()
    {
        GameObject levelSegment = GenerateSegment();
        levelSegment.transform.position = new Vector3(leftGenerationPoint.transform.position.x, transform.position.y, 0f);

       /* // generate mountain
        GameObject mountainSegment = Instantiate(defaultMountainSegment[0], null);
        mountainSegment.transform.position = new Vector3(leftMountainGenerationPoint.transform.position.x, transform.position.y, 0f);*/

        MoveOnXAxis(false);
        
        
    }

    /// <summary>
    /// Generates a new level segment to the right of the player and moves the LevelGenerator more to the right
    /// </summary>
    void GenerateRightSegment()
    {
        GameObject levelSegment = GenerateSegment();
        levelSegment.transform.position = new Vector3 (rightGenerationPoint.transform.position.x - 0.38528f, transform.position.y, 0f);

       /* // generate mountain
        GameObject mountainSegment = Instantiate(defaultMountainSegment[0], null);
        mountainSegment.transform.position = new Vector3(rightMountainGenerationPoint.transform.position.x, transform.position.y, 0f);*/

        MoveOnXAxis(true);
    }

    GameObject GenerateSegment()
    {
        GameObject levelSegment;
        if (segmentsUntilNextEnemySpawn <= 0)
        {
            levelSegment = Instantiate(levelSegmentsWithEnemies[0], null);
            segmentsUntilNextEnemySpawn = default_segmentsUntilNextEnemySpawn;
        }
        else
        {
            levelSegment = Instantiate(defaultLevelSegment[0], null);
            segmentsUntilNextEnemySpawn--;
        }
        return levelSegment;
    }

    void MoveOnXAxis(bool moveRight)
    {
        if (moveRight)
        {
            transform.position = new Vector3(transform.position.x + incrementLength, transform.position.y, transform.position.y);
            maxAllowedDistanceToLeft += incrementLength;
        }
        else
        {
            transform.position = new Vector3(transform.position.x - incrementLength, transform.position.y, transform.position.y);
            maxAllowedDistanceToRight += incrementLength;
        }
    }
}
