using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
    public BackpackSlot[] bagSlots;

    void Start()
    {
        foreach (BackpackSlot slot in bagSlots)
        {
            slot.playerBag = this;
        }
    }

    public BackpackSlot GetNextFreeSlot()
    {
        foreach (BackpackSlot slot in bagSlots)
        {
            if (slot.isFilled == false)
            {
                return slot;
            }
        }
        Debug.Log("error");
        return bagSlots[0];
    }
}
