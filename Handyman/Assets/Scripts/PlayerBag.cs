using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    private bool areSlotsHighlighted = false;
    public BackpackSlot[] bagSlots;

    public BackpackSlot GetNextFreeSlot()
    {
        areSlotsHighlighted = true;
        foreach (BackpackSlot slot in bagSlots)
        {
            if (slot.isFilled == false)
            {
                Debug.Log("adding slot with id " + slot.slotID);
                return slot;
            }
        }
        Debug.Log("error");
        return bagSlots[0];
    }
}
