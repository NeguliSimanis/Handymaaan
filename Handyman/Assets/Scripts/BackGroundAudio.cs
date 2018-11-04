using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundAudio : MonoBehaviour {

    [SerializeField]
    AudioSource audioSource;

    #region AMBIENCE
    bool isFirstCooldown = true;
    bool hasFirstAudioPlayed = false;
    bool isAudioPlaying = false;

    [SerializeField]
    AudioClip []ambientSFX;
    int ambientSFXCount = 1;
    int lastAmbientSFXID = 0;

    float firstCicadasAudioCooldown = 1.5f;
    float audioMinCooldown = 10f;
    float audioMaxCooldown = 22f;
    float audioNextPlayTime;
    #endregion

    #region ENEMY
    [SerializeField]
    AudioClip enemyDeathSFX;
    float enemeyDeathSFXVolume = 0.2f;
    #endregion

    public void PlayEnemyDeathSFX()
    {
        audioSource.PlayOneShot(enemyDeathSFX, enemeyDeathSFXVolume);
    }
    void Update ()
    {
        if (isFirstCooldown)
        {
            if (Time.time > firstCicadasAudioCooldown)
            {
                isFirstCooldown = false;
            }
            return;
        }
		if (!hasFirstAudioPlayed)
        {
            hasFirstAudioPlayed = true;
            audioSource.PlayOneShot(ambientSFX[0]);
            isAudioPlaying = true;
            audioNextPlayTime = Time.time + audioMinCooldown;
        }
        if (Time.time > audioNextPlayTime)
        {
            audioNextPlayTime = Time.time + Random.Range(audioMinCooldown,audioMaxCooldown);
            if (lastAmbientSFXID == 0)
            {
                audioSource.PlayOneShot(ambientSFX[1]);
                lastAmbientSFXID = 1;
            }
            else
            {
                audioSource.PlayOneShot(ambientSFX[0]);
                lastAmbientSFXID = 0;
            }
        }
	}
}
