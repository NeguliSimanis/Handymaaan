using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    
	void Start ()
    {
        Time.timeScale = 0f;	
	}
	

    public void StartGame()
    {
        Time.timeScale = 1f;
    }
}
