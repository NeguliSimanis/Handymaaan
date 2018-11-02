using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSlots : MonoBehaviour
{

    private bool areSlotsHighlighted = false;
    Item itemToEquip;
    [SerializeField]
    Button[] equippedSlots; // 0 - left arm, 1 - head, 2 - right arm

    private void Start()
    {
        equippedSlots[0].onClick.AddListener(EquipLeftArm);
        equippedSlots[1].onClick.AddListener(EquipHead);
        equippedSlots[2].onClick.AddListener(EquipRightArm);
    }

    private void EquipLeftArm()
    {
        
    }

    private void EquipHead()
    {

    }

    private void EquipRightArm()
    {
        if (areSlotsHighlighted)
        {
            // shows equipped item image
            equippedSlots[2].gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Image childImage = equippedSlots[2].gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
            childImage.sprite = itemToEquip.itemImage;
            childImage.preserveAspect = true;

            // turns off highlight
            HideEquippableSlots();

            // TODO - HIDE ITEM FROM BACKPACK
        }
    }

    public void HideEquippableSlots()
    {
        areSlotsHighlighted = false;
        foreach (Button slot in equippedSlots)
        {
            slot.gameObject.GetComponent<Image>().color = Color.white;
        }
    }

    public void ShowEquippableSlots(Item itemForEquip)
    {
        areSlotsHighlighted = true;
        itemToEquip = itemForEquip;
        foreach (Button slot in equippedSlots)
        {

            slot.gameObject.GetComponent<Image>().color = Color.yellow;

            
        }
    }
}
