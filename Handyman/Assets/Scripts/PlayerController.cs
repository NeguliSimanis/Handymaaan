using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region ITEMS
    [SerializeField]
    PlayerBag playerBag;
    [SerializeField]
    EquippedSlots equippedSlots;

    public List<Item> equippedLimbs = new List<Item>();
    public List<Item> inventory = new List<Item>();
    public Transform rightLimbPosition;
    public Transform leftLimbPosition;
    public Transform headPosition;
    #endregion

    #region MOVEMENT
    private bool isFacingRight = true;
    private bool isWalking = false;
    private Vector2 targetPosition;
    private Vector2 dirNormalized;
    #endregion

    #region ATTACK
    private bool isAttackCooldown = false; // true while you cannot attack because cooldown is active
    private float attackCooldownResetTime;
    #endregion

    #region UI
    [SerializeField]
    GameObject inventoryPanel;
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
        ManageKeyInput();
        CheckIfDead();
        CheckWherePlayerIsFacing();
    }

    void CheckWherePlayerIsFacing() 
    {
        if (isFacingRight && Input.GetAxisRaw("Horizontal") < 0)
        {
            isFacingRight = false;
            gameObject.transform.localScale = new Vector2(-1f, 1f);
        }
        else if (!isFacingRight && Input.GetAxisRaw("Horizontal") > 0)
        {
            isFacingRight = true;
            gameObject.transform.localScale = new Vector2(1f, 1f);
        }
    }

    void CheckIfDead()
    {
        if (PlayerData.current.currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ManageKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        }
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
        // ARM ATTACK
        if (Input.GetMouseButton(1) || Input.GetKeyDown(KeyCode.E))
        {
            if (!isAttackCooldown)
            {
                Attack();
            }
        }
        // HEAD THROW
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("HEADBUTT");
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
        foreach (Item limb in equippedLimbs)
        {
            if (limb.currentPlayerSlot == Item.EquippedSlot.LeftHand || limb.currentPlayerSlot == Item.EquippedSlot.RightHand)
                limb.StartAttack();
        }

    }

    void ManageMovementInput()
    {
        
        transform.position = new Vector2(transform.position.x, transform.position.y) + new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * PlayerData.current.moveSpeed * Time.deltaTime;
        /*if (Input.GetMouseButton(0))
         {
             GetTargetPositionAndDirection();
             CheckIfPlayerIsWalking();
         }
         if (isWalking)
         {
             CheckIfPlayerIsWalking();
             MovePlayer();
         }*/
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

    public void AddItemToInventory(Item itemToAdd, bool isInInventory = false)
    {
        if (!isInInventory)
            inventory.Add(itemToAdd);

        BackpackSlot slotToFill = playerBag.GetNextFreeSlot();
        slotToFill.FillSlot(itemToAdd);
        slotToFill.equippedSlots = equippedSlots;
    }
}
