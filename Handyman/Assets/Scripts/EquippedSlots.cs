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
    Button[] equippedSlots; // 0 - left arm, 1 - head, 2 - right arm

    #region EQUIPPED ITEMS
    Item headItem = null;
    Item leftArmItem = null;
    Item rightArmItem = null;
    #endregion

    private void Start()
    {
        equippedSlots[0].onClick.AddListener(EquipLeftArm);
        equippedSlots[1].onClick.AddListener(EquipHead);
        equippedSlots[2].onClick.AddListener(EquipRightArm);
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
