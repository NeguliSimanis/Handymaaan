using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region PLAYER
    public PlayerController player;
    private Transform playerTransform;
    #endregion

    #region STATE
    public bool isPlayerVisible = false;
    public bool isNearPlayer = false;
    #endregion

    #region Properties
    [SerializeField]
    int maxHP;
    int currentHP;
    #endregion

    #region MOVEMENT

    [SerializeField]
    float moveSpeed;
    Vector2 dirNormalized;
    #endregion

    private void Start()
    {
        currentHP = maxHP;
        playerTransform = player.gameObject.GetComponent<Transform>();
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("take damage");
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        // managing following players
        if (isPlayerVisible)
        {
            if (!isNearPlayer)
            {
                GetDirNormalized();
                FollowPlayer();
            }
        }
    }

    void GetDirNormalized()
    {
        dirNormalized = new Vector2(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y);
        dirNormalized = dirNormalized.normalized;
    }

    private void FollowPlayer()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y) + dirNormalized * moveSpeed * Time.deltaTime;
    }
}
