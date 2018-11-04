using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    #region PROPERTIES
    public string itemName;
    float rotationSpeed = 2690f;
    public Sprite itemImage;
    [SerializeField]
    EnemyController parentEnemyController;
    #endregion

    #region MOVEMENT
    [SerializeField]
    Transform legRotationAxis;
    float legRotationSpeed = 330f;
    #endregion

    #region THROWING
    [SerializeField]
    float throwSpeed;
    Rigidbody2D thisRigidbody2D;
    float throwDuration = 0.4f;
    float throwEndTime;

    public bool isSelfDestructing = false;
    float selfDestructTimer = 3.5f;
    float selfDestructTime;

    [SerializeField]
    HeadDamageCollider headDamageCollider;
    #endregion

    #region ATTACK
    bool canDamageEnemy = false;
    bool hasDealtDamageThisTime = false;
    EnemyController currentEnemy;

    public int attackDamage;
    float attackDuration = 0.1f;
    float attackCooldownResetTime;
    float attackStartTime;
    #endregion

    #region CURRENT STATE
    public enum ItemState { OnGround, InInventory, Equipped, EquippedByEnemy, CarriedByEnemy};
    public enum EquippedSlot { RightHand, LeftHand, Head, LeftLeg, RightLeg};
    public EquippedSlot currentPlayerSlot; // slot in which the item is equipped
    public EquippedSlot originalLimbType;  // the original type of limb, because head can be equipped in place of arm, etc.
    public ItemState currentState;
    private bool isAttacking = false;

    public bool isThrown = false;
    bool isThrownRight = true;
    #endregion

    #region PLAYER
    [SerializeField]
    GameObject playerObject;
    PlayerController playerController;
    #endregion

    private void Start()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        playerController = playerObject.GetComponent<PlayerController>();
        if (currentState == ItemState.EquippedByEnemy)
        {
            DisablePhysics();
        }
        itemName = GenerateItemName.GenerateLimbName(originalLimbType);
    }

    void DisablePhysics()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;    
    }

    public void Throw(bool throwRight)
    {
        UnEquipItem(false);
        transform.parent = null;
        currentState = ItemState.OnGround;    
        if (gameObject.GetComponent<Rigidbody2D>() != null)
        {
            thisRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            thisRigidbody2D.isKinematic = false;
            thisRigidbody2D.freezeRotation = false;

            //gameObject.GetComponent<CircleCollider2D>().enabled = true;

            isThrown = true;
            throwEndTime = Time.time + throwDuration;
            if (throwRight)
            {
                isThrownRight = true;
                //rigidbody.AddForce(Vector2.right * throwSpeed);
                //rigidbody.AddForce(Vector2.right * throwSpeed);
            }
            else
            {
                isThrownRight = false;
               // rigidbody.AddForce(Vector2.right * -1 * throwSpeed);
            }
        }
    }


    private void Fly()
    {
        thisRigidbody2D.AddForce(new Vector2(5,5.5f) * throwSpeed);
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
            headDamageCollider.Reset();
            transform.position = playerController.headPosition.position;
        }
        if (slot == EquippedSlot.LeftHand)
        {
            transform.position = playerController.leftLimbPosition.position;
        }
        if (slot == EquippedSlot.LeftLeg)
        {
            transform.position = playerController.leftLegPosition.position;
        }
        if (slot == EquippedSlot.RightLeg)
        {
            transform.position = playerController.rightLegPosition.position;
        }
        transform.localEulerAngles = Vector3.zero;

        if (!playerController.isFacingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (gameObject.GetComponent<Rigidbody2D>() != null)
        {
            Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
            rigidbody.isKinematic = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.freezeRotation = true;
            rigidbody.rotation = 0f;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        DisableChildrenAndColliders(false);
    }

    /// <summary>
    /// unequip item and put it into inventory or remove it completely from player
    /// </summary>
    public void UnEquipItem(bool putInInventory = true)
    {
        playerController.equippedLimbs.Remove(this);
        if (putInInventory)
            AddToInventory(true);
        // called when you throw a head - unequip the item completely
        else
        {
            playerController.equippedSlots.UnequipHead();
        }
    }

    public void AddToInventory(bool isInInventory = false)
    {
        isSelfDestructing = false;
        if (currentState != Item.ItemState.EquippedByEnemy)
        {
            currentState = ItemState.InInventory;

            playerController.AddItemToInventory(this, isInInventory);

            if (gameObject.GetComponent<Rigidbody2D>()!= null)
            {
                thisRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
                thisRigidbody2D.isKinematic = true;
                thisRigidbody2D.velocity = Vector2.zero;
            }

            DisableChildrenAndColliders(true);
        }
    }

    /// <summary>
    /// Called when you add/remove item from inventory and other cases when you need to disable object functionality
    /// </summary>
    /// <param name="isAddingToInventory"></param>
    void DisableChildrenAndColliders(bool isAddingToInventory)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(!isAddingToInventory);
       
        }
        if (gameObject.GetComponent<CircleCollider2D>() != null)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
        if (thisRigidbody2D != null)
        {
            thisRigidbody2D.freezeRotation = false;
        }
        attackCooldownResetTime = Time.time + attackDuration;
        attackStartTime = Time.time;
       
    }

    private void Attack()
    {
        // Rotate the object around its local Z axis. Rotation speed increases the more time has passed since attack started
        //transform.Rotate(0, 0, Time.deltaTime * rotationSpeed * (1+ (Time.time - attackStartTime)));
        transform.RotateAround(legRotationAxis.position, new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime * (1 + (Time.time - attackStartTime)));

        if (currentState != ItemState.EquippedByEnemy)
        {
            DealDamageToEnemy();
        }
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
        if (currentState == ItemState.Equipped && playerController.isWalking)
        {
            SwingLeg();
        }
        if (currentState == ItemState.EquippedByEnemy)
        {
            if (parentEnemyController.isPlayerVisible && !parentEnemyController.isNearPlayer)
            {
                SwingLeg();
            }
        }
        if (isThrown)
        {
            Fly();
            CheckThrowEndTime();
        }
        if (isSelfDestructing)
        {
            if (Time.time > selfDestructTime)
            {
                SelfDestruct();
            }
        }
    }

    private void SwingLeg()
    {
        if ((currentPlayerSlot == EquippedSlot.RightLeg || currentPlayerSlot == EquippedSlot.LeftLeg) ||
            (originalLimbType == EquippedSlot.RightLeg || originalLimbType == EquippedSlot.LeftLeg))
        {
            //transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
            //transform.RotateAround(Vector3.zero, Vector3.up, 20 * Time.deltaTime);
            transform.RotateAround(legRotationAxis.position, new Vector3(0, 0, 1),legRotationSpeed  *Time.deltaTime);
        }
    }



    public void SelfDestruct()
    {
        Destroy(gameObject);
    }

    private void CheckThrowEndTime()
    {
        if (Time.time >throwEndTime)
        {
            isThrown = false;
            isSelfDestructing = true;
            selfDestructTime = Time.time + selfDestructTimer;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
