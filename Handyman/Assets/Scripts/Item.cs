using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region PROPERTIES
    string itemName;
    float rotationSpeed = 290f;
    public Sprite itemImage;
    #endregion

    #region ATTACK
    bool canDamageEnemy = false;
    bool hasDealtDamageThisTime = false;
    EnemyController currentEnemy;

    public int attackDamage;
    float attackDuration = 0.4f;
    float attackCooldownResetTime;
    float attackStartTime;
    #endregion

    #region CURRENT STATE
    public enum ItemState { OnGround, InInventory, Equipped, EquippedByEnemy};
    public enum EquippedSlot { RightHand, LeftHand, Head};
    public EquippedSlot currentPlayerSlot; // slot in which the item is equipped
    public ItemState currentState;
    private bool isAttacking = false;
    #endregion

    #region PLAYER
    [SerializeField]
    GameObject playerObject;
    PlayerController playerController;
    #endregion

    private void Start()
    {
        playerController = playerObject.GetComponent<PlayerController>();
    }


    public void EquipItem(EquippedSlot slot)
    {

        currentPlayerSlot = slot;
        currentState = ItemState.Equipped;
        
        playerController.equippedLimbs.Add(this);

        // physical placement in game world
        transform.parent = playerObject.transform;
        
        if (slot == EquippedSlot.RightHand)
        {
            transform.position = playerController.rightLimbPosition.position; 
        }
        if (slot == EquippedSlot.Head)
        {
            transform.position = playerController.headPosition.position;
        }
        if (slot == EquippedSlot.LeftHand)
        {
            transform.position = playerController.leftLimbPosition.position;
        }

        DisableChildren(false);
    }

    /// <summary>
    /// unequip item and put it into inventory
    /// </summary>
    public void UnEquipItem()
    {
        playerController.equippedLimbs.Remove(this);
        AddToInventory(true);
        Debug.Log("unequipping");
    }

    public void AddToInventory(bool isInInventory = false)
    {
        if (currentState != Item.ItemState.EquippedByEnemy)
        {
            currentState = ItemState.InInventory;

            playerController.AddItemToInventory(this, isInInventory);

            DisableChildren(true);
        }
    }

    /// <summary>
    /// Called when you add/remove item from inventory
    /// </summary>
    /// <param name="isAddingToInventory"></param>
    void DisableChildren(bool isAddingToInventory)
    {
        Debug.Log("disabling children of " + gameObject.name);
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!isAddingToInventory);
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        attackCooldownResetTime = Time.time + attackDuration;
        attackStartTime = Time.time;
    }

    private void Attack()
    {
        // Rotate the object around its local Z axis. Rotation speed increases the more time has passed since attack started
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeed * (1+ (Time.time - attackStartTime)));

        if (currentState != ItemState.EquippedByEnemy)
            DealDamageToEnemy();
    }

    private void DealDamageToEnemy()
    {
        if (!hasDealtDamageThisTime && canDamageEnemy)
        {
            hasDealtDamageThisTime = true;
            currentEnemy.TakeDamage(attackDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                canDamageEnemy = true;
                currentEnemy = collision.gameObject.GetComponent<EnemyController>();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isAttacking)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                canDamageEnemy = true;
                currentEnemy = collision.gameObject.GetComponent<EnemyController>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isAttacking)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (collision.gameObject.GetComponent<EnemyController>() == currentEnemy)
                    canDamageEnemy = false;
            }
        }
    }

    private void ResetAttackCooldown()
    {
        if (Time.time > attackCooldownResetTime)
        {
            isAttacking = false;
            hasDealtDamageThisTime = false;
        }
    }
    

    private void Update()
    {
        if (currentState == ItemState.Equipped || currentState == ItemState.EquippedByEnemy)
        {
            if (isAttacking)
            {
                Attack();
                ResetAttackCooldown();
            }
        }
    }
}
