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
    [SerializeField] int attackDamage;
    float attackDuration = 0.4f;
    float attackCooldownResetTime;
    float attackStartTime;
    #endregion

    #region CURRENT STATE
    public enum ItemState { OnGround, InInventory, Equipped};
    public enum EquippedSlot { RightHand, LeftHand, Head};
    private EquippedSlot currentPlayerSlot; // slot in which the item is equipped
    private ItemState currentState;
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
        currentState = ItemState.InInventory;

        playerController.AddItemToInventory(this, isInInventory);

        DisableChildren(true);
        
    }

    /// <summary>
    /// Called when you add/remove item from inventory
    /// </summary>
    /// <param name="isAddingToInventory"></param>
    void DisableChildren(bool isAddingToInventory)
    {
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision");
        if (isAttacking && collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(attackDamage);
        }
    }

    private void ResetAttackCooldown()
    {
        if (Time.time > attackCooldownResetTime)
        {
            isAttacking = false;
        }
    }

    private void Update()
    {
        if (isAttacking)
        {
            Attack();
            ResetAttackCooldown();
        }
  
    }
}
