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
    private bool isFacingRight = true;
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
    [SerializeField]
    GameObject itemToDrop;
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
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }
        playerTransform = player.gameObject.GetComponent<Transform>();
    }

    public void TakeDamage(int amount)
    {
        if (!isDead)
        {
            player.PlayPunchContactSFX();
        }
        else
            return;
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
       // enemyLimb.transform.parent = null;
       // enemyLimb.currentState = Item.ItemState.OnGround;

        GameObject drop = Instantiate(gameObject.GetComponent<EnemyDrops>().GetDrop());
        drop.transform.position = transform.position;
        drop.transform.parent = null;
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
        CheckWhereEnemyIsFacing();
        transform.position = new Vector2(transform.position.x, transform.position.y) + dirNormalized * moveSpeed * Time.deltaTime;
    }

    void CheckWhereEnemyIsFacing()
    {
        if (isFacingRight && dirNormalized.x < 0)
        {
            isFacingRight = false;
            gameObject.transform.localScale = new Vector2(-1f, 1f);
        }
        else if (!isFacingRight && dirNormalized.x > 0)
        {
            isFacingRight = true;
            gameObject.transform.localScale = new Vector2(1f, 1f);
        }
    }
}
