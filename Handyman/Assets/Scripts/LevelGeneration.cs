using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    [SerializeField]
    Transform player;

    [SerializeField]
    GameObject[] defaultLevelSegment;

    [SerializeField]
    Transform leftGenerationPoint;
    [SerializeField]
    Transform rightGenerationPoint;

    float maxAllowedDistanceToRight = 27; // if you move further away, level generator object instantiates a new level segment and moves closer
    float maxAllowedDistanceToLeft = 13; // -''-

	void Start ()
    {
       // Debug.Log("distance to right point " + (rightGenerationPoint.position.x - transform.position.x)); // 26.73777
       // Debug.Log("distance to left point " + (transform.position.x - leftGenerationPoint.position.x)); // 12.97923
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log("distance to right point " + (rightGenerationPoint.position.x - transform.position.x)); // 26.73777
        Debug.Log("distance to left point " + (transform.position.x - leftGenerationPoint.position.x)); // 12.97923

        // check distance to left point
        if (transform.position.x - leftGenerationPoint.position.x > maxAllowedDistanceToLeft)
        {
            GenerateRightSegment();
        }


        // check distance to right point    
        if (rightGenerationPoint.position.x - transform.position.x > maxAllowedDistanceToRight)
        {
            GenerateLeftSegment();
        }
    }

    /// <summary>
    /// Generates a new level segment to the left of the player and moves the LevelGenerator more to the left
    /// </summary>
    void GenerateLeftSegment()
    {
        Instantiate(defaultLevelSegment[0],leftGenerationPoint);
        MoveToPlayerXPosition();
        
    }

    /// <summary>
    /// Generates a new level segment to the right of the player and moves the LevelGenerator more to the right
    /// </summary>
    void GenerateRightSegment()
    {
        Instantiate(defaultLevelSegment[0], rightGenerationPoint);
        MoveToPlayerXPosition();
    }

    void MoveToPlayerXPosition()
    {
        transform.position = new Vector3(player.position.x, transform.position.y, transform.position.y);
    }
}
