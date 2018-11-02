﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region PROPERTIES
    string itemName;
    float rotationSpeed = 130f;
    public Sprite itemImage;
    #endregion

    #region ATTACK
    float attackDuration = 1f;
    float attackCooldownResetTime;
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
    }

    public void AddToInventory()
    {
        currentState = ItemState.InInventory;
        playerController.AddItemToInventory(this);

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
            child.gameObject.SetActive(false);
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        attackCooldownResetTime = Time.time + attackDuration;
        Debug.Log("attacking with limb!");
    }

    private void Attack()
    {
        // Rotate the object around its local Z axis
        transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
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
        // TEMPORARY FOR TESTING    
        if (Input.GetKey(KeyCode.E))
        {
            EquipItem(EquippedSlot.RightHand);
        }

        if (isAttacking)
        {
            Attack();
            ResetAttackCooldown();
        }
  
    }
}
