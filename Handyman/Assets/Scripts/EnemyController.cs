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
    bool isDead = false;
    #endregion

    #region HEALTH
    [SerializeField]
    int maxHP;
    int currentHP;
    #endregion

    #region LIMBS
    public Item enemyLimb;
    #endregion

    #region ATTACK
    bool isAttackCooldown = false;
    [SerializeField]
    float attackCooldown;
    float attackCoooldownResetTime;
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
        currentHP -= amount;
        Debug.Log(currentHP);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            DropItem();
            PlayerData.current.currentScore++;
            Destroy(gameObject);
        }
    }

    void DropItem()
    {
        enemyLimb.transform.parent = null;
        enemyLimb.currentState = Item.ItemState.OnGround;
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
            else
            {
                ManageAttackingPlayer();
            }
        }
        ResetAttackCooldown();
    }

    void ResetAttackCooldown()
    {
        if (Time.time > attackCoooldownResetTime)
        {
            isAttackCooldown = false;
        }
    }
    void ManageAttackingPlayer()
    {
        if (!isAttackCooldown)
        {
            isAttackCooldown = true;
            attackCoooldownResetTime = Time.time + attackCooldown;
            PlayerData.current.currentHP -= enemyLimb.attackDamage;
            PlayAttackAnimation();
        }
    }

    void PlayAttackAnimation()
    {
        Debug.Log("play");
        enemyLimb.StartAttack();
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
