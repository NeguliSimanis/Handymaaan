using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region
    public List<Item> equippedLimbs = new List<Item>();
    public List<Item> inventory = new List<Item>();
    public Transform rightLimbPosition;
    #endregion

    #region MOVEMENT
    private bool isWalking = false;
    private Vector2 targetPosition;
    private Vector2 dirNormalized;
    #endregion

    #region ATTACK
    private bool isAttackCooldown = false; // true while you cannot attack because cooldown is active
    private float attackCooldownResetTime;
    #endregion

    private void Start()
    {
        if (PlayerData.current == null)
        {
            PlayerData.current = new PlayerData();
        }   
    }

    void Update ()
    {
        ManageMovementInput();
        ManageAttackInput();
        ResetAttackCooldown();
    }

    void ResetAttackCooldown()
    {
        if (isAttackCooldown && Time.time > attackCooldownResetTime)
        {
            isAttackCooldown = false;
        }
    }

    void ManageAttackInput()
    {
        if (Input.GetMouseButton(1))
        {
            if (!isAttackCooldown)
            {
                Attack();
            }
        }
    }

    void Attack()
    {
        isAttackCooldown = true;
        attackCooldownResetTime = Time.time + PlayerData.current.attackCooldown;
        UseAttackLimb();
    }

    void UseAttackLimb()
    {
        equippedLimbs[0].StartAttack();
    }

    void ManageMovementInput()
    {
        if (Input.GetMouseButton(0))
        {
            GetTargetPositionAndDirection();
            CheckIfPlayerIsWalking();
        }
        if (isWalking)
        {
            CheckIfPlayerIsWalking();
            MovePlayer();
        }
    }

    void CheckIfPlayerIsWalking()
    {
        if (Vector2.Distance(targetPosition, transform.position) <= 0.02f)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }
    }


    void GetTargetPositionAndDirection()
    {
        targetPosition = Input.mousePosition;
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        GetDirNormalized(targetPosition);
    }

    void GetDirNormalized(Vector2 sourceVector)
    {
        dirNormalized = new Vector2(sourceVector.x - transform.position.x, sourceVector.y - transform.position.y);
        dirNormalized = dirNormalized.normalized;
    }

    void MovePlayer()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y) + dirNormalized * PlayerData.current.moveSpeed * Time.deltaTime;
    }
}
