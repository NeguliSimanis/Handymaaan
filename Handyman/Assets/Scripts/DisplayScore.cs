using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour {


    [SerializeField]
    Text scoreText;
    

	void Start ()
    {
		if (PlayerData.current == null)
        {
            PlayerData.current = new PlayerData();
        }
	}
	
	void Update ()
    {
        scoreText.text = "Score: " + PlayerData.current.currentScore;
	}
}
