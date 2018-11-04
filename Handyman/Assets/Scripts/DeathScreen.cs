using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSourceToDisable;
    
	void Start ()
    {
        audioSourceToDisable.enabled = false;
	}
}
