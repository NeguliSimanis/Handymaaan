using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFallSFX : MonoBehaviour
{
    Item item;

    //[SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip fallSFX;

    float fallSFXCooldownResetTime;
    float fallSFXCooldown = 0.3f;
    float fallSFXVolume = 0.7f;
    bool isFallSFXCooldown = false;

    private void Start()
    {
        item = gameObject.GetComponent<Item>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // object collides with player or enemy
        if (collision.gameObject.layer == 14 || collision.gameObject.layer == 15)
        {
            if (!isFallSFXCooldown)
            {
                PlayFallSFX();
            }
            else if (Time.time > fallSFXCooldownResetTime)
            {
                PlayFallSFX();
            }
        }            
    }

    void PlayFallSFX()
    {
        if (item.currentState == Item.ItemState.OnGround && Time.timeScale > 0)
        {
            isFallSFXCooldown = true;
            fallSFXCooldownResetTime = Time.time + fallSFXCooldown;
            audioSource.PlayOneShot(fallSFX, fallSFXVolume);
        }
    }
}
