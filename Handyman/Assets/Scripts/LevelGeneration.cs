using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

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

    float maxAllowedDistanceToRight = 26.73777f; // if you move further away, level generator object instantiates a new level segment and moves closer
    float maxAllowedDistanceToLeft = 12.97923f; // -''-

	void Start ()
    {
       // Debug.Log("distance to right point " + (rightGenerationPoint.position.x - transform.position.x)); // 26.73777
       // Debug.Log("distance to left point " + (transform.position.x - leftGenerationPoint.position.x)); // 12.97923
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
        GameObject levelSegment = Instantiate(defaultLevelSegment[0], null);
        levelSegment.transform.position = new Vector3(leftGenerationPoint.transform.position.x, transform.position.y, 0f);
        MoveOnXAxis(false);
        
    }

    /// <summary>
    /// Generates a new level segment to the right of the player and moves the LevelGenerator more to the right
    /// </summary>
    void GenerateRightSegment()
    {
        GameObject levelSegment = Instantiate(defaultLevelSegment[0], null);
        levelSegment.transform.position = new Vector3 (rightGenerationPoint.transform.position.x, transform.position.y, 0f);
        MoveOnXAxis(true);
    }

    void MoveOnXAxis(bool moveRight)
    {
        if (moveRight)
        {
            transform.position = new Vector3(transform.position.x + 2, transform.position.y, transform.position.y);
            maxAllowedDistanceToLeft += 2;
        }
        else
        {
            transform.position = new Vector3(transform.position.x - 2, transform.position.y, transform.position.y);
            maxAllowedDistanceToRight += 2;
        }
    }
}
