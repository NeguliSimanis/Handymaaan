using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySFX : MonoBehaviour
{

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioClip openBagSFX;
    [SerializeField]
    AudioClip closeBagSFX;

    private void OnEnable()
    {
        audioSource.PlayOneShot(openBagSFX, 0.2f);
    }
    private void OnDisable()
    {
        audioSource.PlayOneShot(closeBagSFX, 0.2f);
    }
}
