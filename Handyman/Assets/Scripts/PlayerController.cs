using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region ITEMS
    [SerializeField]
    PlayerBag playerBag;

    public EquippedSlots equippedSlots;

    public List<Item> equippedLimbs = new List<Item>();
    public List<Item> inventory = new List<Item>();
    public Transform rightLimbPosition;
    public Transform leftLimbPosition;
    public Transform leftLegPosition;
    public Transform rightLegPosition;
    public Transform headPosition;
    #endregion

    #region MOVEMENT
    public bool isFacingRight = true;
    public bool isWalking = false;
    private Vector2 targetPosition;
    private Vector2 dirNormalized;
    [SerializeField]
    Transform playerUpperBound;
    [SerializeField]
    Transform playerLowerBound;
    #endregion

    #region ATTACK
    private bool isAttackCooldown = false; // true while you cannot attack because cooldown is active
    private float attackCooldownResetTime;
    #endregion

    #region UI
    [SerializeField]
    GameObject inventoryPanel;
    [SerializeField]
    GameObject defeatWindow;
    #endregion

    #region AUDIO
    [Header("AUDIO")]
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip throwHeadSFX;
    [SerializeField]
    AudioClip pickUpItemSFX;

    // WALKING
    [SerializeField]
    AudioClip [] walkSFXs;
    bool isWalkSFXCooldown = false;
    float minWalkSFXCooldown = 2.3f;
    float maxWalkSFXCooldown = 3.5f;
    float walkSFXCooldownResetTime;
    

    // PUNCHING
    [SerializeField]
    AudioClip punchContact;
    [SerializeField]
    AudioClip punchThrow;
    #endregion

    #region INTRO
    [Header("Game intro")]
    [SerializeField]
    Rigidbody2D[] introLimbs;
    bool isIntroPlayed = false;
    #endregion

    private void Start()
    {
        if (PlayerData.current == null)
        {
            PlayerData.current = new PlayerData();
        }
    }

    void Update()
    {
        ManageMovementInput();
        ManageAttackInput();
        ResetAttackCooldown();
        ManageKeyInput();
        CheckIfDead();
        CheckWherePlayerIsFacing();
        ManageWalkingAudio();
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
            defeatWindow.SetActive(true);
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
            ThrowHead();
        }
    }

    void ThrowHead()
    {
        if (equippedSlots.headItem != null)
        {
            audioSource.PlayOneShot(throwHeadSFX);
            equippedSlots.headItem.Throw(isFacingRight);
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
        bool shouldPlaySFX = false;
        foreach (Item limb in equippedLimbs)
        {
            if (limb.currentPlayerSlot == Item.EquippedSlot.LeftHand || limb.currentPlayerSlot == Item.EquippedSlot.RightHand)
            {
                shouldPlaySFX = true;
                limb.StartAttack();
            }
        }
        // play punch only if you have equipped a hand
        if (shouldPlaySFX)
            PlayPunchThrowSFX();
    }

    void PlayIntro()
    {
        isIntroPlayed = true;
        foreach (Rigidbody2D limb in introLimbs)
        {
            limb.isKinematic = false;
        }
    }

    void ManageMovementInput()
    {

        float verticalMovementMultiplier = Input.GetAxisRaw("Vertical");

        // don't allow player trespass upper border of level
        if (transform.position.y >= playerUpperBound.position.y && verticalMovementMultiplier > 0)
        {
            verticalMovementMultiplier = 0;
        }

        // don't allow player trespass lower border of level
        if (transform.position.y <= playerLowerBound.position.y && verticalMovementMultiplier < 0)
        {
            verticalMovementMultiplier = 0;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y) + new Vector2(Input.GetAxisRaw("Horizontal"), verticalMovementMultiplier) * PlayerData.current.moveSpeed * Time.deltaTime;
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if (!isIntroPlayed)
            {
                PlayIntro();
            }
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
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

    #region SFX methods
    /// <summary>
    /// plays sfx when your melee attack harms enemy
    /// </summary>
    public void PlayPunchContactSFX()
    {
        audioSource.PlayOneShot(punchContact,0.7f);
    }

    public void PlayPunchThrowSFX()
    {
        //audioSource.PlayOneShot(punchContact);
        audioSource.PlayOneShot(punchThrow,0.2f);
    }

    public void PlayPickupItemSFX()
    {
        audioSource.PlayOneShot(pickUpItemSFX, 0.25f);
    }

    void ManageWalkingAudio()
    {
        if (isWalking)
        {
            // play walk sfx if player has legs
            if (!isWalkSFXCooldown && (equippedSlots.rightLegItem != null || equippedSlots.leftLegItem != null))
            {
                isWalkSFXCooldown = true;
                walkSFXCooldownResetTime = Time.time + Random.Range(minWalkSFXCooldown, maxWalkSFXCooldown);
                audioSource.PlayOneShot(walkSFXs[Random.Range(0,walkSFXs.Length-1)], 0.2f);
            }
            else if (Time.time > walkSFXCooldownResetTime)
            {
                isWalkSFXCooldown = false;
            }
        }
    }
    #endregion
}
