using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlots : MonoBehaviour
{

    private bool areSlotsHighlighted = false;
    Item itemToEquip;
    BackpackSlot backpackSlotToEmpty;
    [SerializeField]
    Button[] equippedSlots; // 0 - left arm, 1 - head, 2 - right arm, 3 - left leg, 4 - right leg

    #region EQUIPPED ITEMS
    public Item headItem = null;
    Item leftArmItem = null;
    Item rightArmItem = null;
    Item leftLegItem = null;
    Item rightLegItem = null;
    #endregion

    private void Start()
    {
        equippedSlots[0].onClick.AddListener(EquipLeftArm);
        equippedSlots[1].onClick.AddListener(EquipHead);
        equippedSlots[2].onClick.AddListener(EquipRightArm);
        equippedSlots[3].onClick.AddListener(EquipLeftLeg);
        equippedSlots[4].onClick.AddListener(EquipRightLeg);
    }

    private void EquipLeftLeg()
    {
        if (areSlotsHighlighted)
        {
            // Put currently equipped item in inventory
            if (leftLegItem != null)
            {
                leftLegItem.UnEquipItem();
            }

            leftLegItem = itemToEquip;
            // shows equipped item image
            equippedSlots[3].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Image childImage = equippedSlots[3].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            childImage.sprite = itemToEquip.itemImage;
            childImage.preserveAspect = true;

            // turns off highlight
            HideEquippableSlots();

            EquipItem(equippedSlots[3].gameObject.GetComponent<SlotType>().slotType);

            //HIDE ITEM FROM BACKPACK
            RemoveFromBackpack();
        }
    }

    private void EquipRightLeg()
    {
        if (areSlotsHighlighted)
        {
            // Put currently equipped item in inventory
            if (rightLegItem != null)
            {
                rightLegItem.UnEquipItem();
            }

            rightLegItem = itemToEquip;
            // shows equipped item image
            equippedSlots[4].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Image childImage = equippedSlots[4].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            childImage.sprite = itemToEquip.itemImage;
            childImage.preserveAspect = true;

            // turns off highlight
            HideEquippableSlots();

            EquipItem(equippedSlots[4].gameObject.GetComponent<SlotType>().slotType);

            //HIDE ITEM FROM BACKPACK
            RemoveFromBackpack();
        }
    }

    private void EquipLeftArm()
    {
        if (areSlotsHighlighted)
        {
            // Put currently equipped item in inventory
            if (leftArmItem != null)
            {
                leftArmItem.UnEquipItem();
            }

            leftArmItem = itemToEquip;
            // shows equipped item image
            equippedSlots[0].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Image childImage = equippedSlots[0].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            childImage.sprite = itemToEquip.itemImage;
            childImage.preserveAspect = true;

            // turns off highlight
            HideEquippableSlots();

            EquipItem(equippedSlots[0].gameObject.GetComponent<SlotType>().slotType);

            //HIDE ITEM FROM BACKPACK
            RemoveFromBackpack();
        }
    }

    public void UnequipHead()
    {
        headItem = null;
        equippedSlots[1].gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void EquipHead()
    {

        if (areSlotsHighlighted)
        {
            // Put currently equipped item in inventory
            if (headItem != null)
            {
                headItem.UnEquipItem();
            }

            headItem = itemToEquip;

            // shows equipped item image
            equippedSlots[1].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Image childImage = equippedSlots[1].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            childImage.sprite = itemToEquip.itemImage;
            childImage.preserveAspect = true;

            // turns off highlight
            HideEquippableSlots();

            EquipItem(equippedSlots[1].gameObject.GetComponent<SlotType>().slotType);

            //HIDE ITEM FROM BACKPACK
            RemoveFromBackpack();
        }
    }

    private void EquipRightArm()
    {

        if (areSlotsHighlighted)
        {
            // Put currently equipped item in inventory
            if (rightArmItem != null)
            {
                rightArmItem.UnEquipItem();
            }

            rightArmItem = itemToEquip;

            // shows equipped item image
            equippedSlots[2].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Image childImage = equippedSlots[2].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            childImage.sprite = itemToEquip.itemImage;
            childImage.preserveAspect = true;

            // turns off highlight
            HideEquippableSlots();

            EquipItem(equippedSlots[2].gameObject.GetComponent<SlotType>().slotType);

            //HIDE ITEM FROM BACKPACK
            RemoveFromBackpack();
        }
    }

    private void EquipItem(Item.EquippedSlot equippedSlot)
    {
        itemToEquip.EquipItem(equippedSlot);
    }

    private void RemoveFromBackpack()
    {
        backpackSlotToEmpty.EmptySlot();
    }

    public void HideEquippableSlots()
    {
        areSlotsHighlighted = false;
        foreach (Button slot in equippedSlots)
        {
            slot.gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    public void ShowEquippableSlots(Item itemForEquip, BackpackSlot slotToEmpty)
    {
        areSlotsHighlighted = true;
        itemToEquip = itemForEquip;
        backpackSlotToEmpty = slotToEmpty;
        foreach (Button slot in equippedSlots)
        {

            slot.gameObject.GetComponent<Image>().color = Color.yellow;

            
        }
    }
}
