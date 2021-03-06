﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackSlot : MonoBehaviour
{
    [SerializeField] Button slotButton;
    public EquippedSlots equippedSlots;
    public PlayerBag playerBag; // all slots
    public bool isFilled = false;
    public int slotID;
    private Item itemInSlot;

    void Start()
    {
        slotButton = gameObject.GetComponent<Button>();
        if (isFilled == false)
        {
            slotButton.interactable = false;
        }
        slotButton.onClick.AddListener(SelectSlot);
    }

    private void SelectSlot()
    {
        equippedSlots.ShowEquippableSlots(itemInSlot, this);
        gameObject.GetComponent<Image>().color = Color.yellow;
        UnselectAllOtherSlots();
    }

    private void UnselectAllOtherSlots()
    {
        foreach (BackpackSlot slot in playerBag.bagSlots)
        {
            if (slot.slotID != slotID)
            {
                slot.gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void EmptySlot()
    {
        isFilled = false;
        slotButton.interactable = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        gameObject.GetComponent<Image>().color = Color.white;
    }

    public void FillSlot(Item itemToAdd)
    {
        itemInSlot = itemToAdd;
        isFilled = true;
        slotButton.interactable = true;

        int currentChildID = 0;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            // set item name
            if (currentChildID == 1)
            {
                child.gameObject.GetComponent<Text>().text = itemToAdd.itemName;
            }


            // set item image
            if (currentChildID==0)
            {
                Image childImage = child.gameObject.GetComponent<Image>();
                childImage.sprite = itemToAdd.itemImage;
                childImage.preserveAspect = true;  
            }
            currentChildID++;
        }
    }

    
}
