using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackSlot : MonoBehaviour
{
    [SerializeField] Button slotButton;
    public EquippedSlots equippedSlots;
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
        equippedSlots.ShowEquippableSlots(itemInSlot);
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
