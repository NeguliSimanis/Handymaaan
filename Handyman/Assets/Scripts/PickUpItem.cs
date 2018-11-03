using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]
    Item item;
    string playerTag = "Player";
    bool canBePickedUp = false; // can pick up item if player is in the collider area;
    bool pickUpCommandReceived = false; // player picked on the item but was outside of the area

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == playerTag)
        {
            canBePickedUp = true;
            if (pickUpCommandReceived && item.currentState!= Item.ItemState.Equipped)
            {
                item.AddToInventory();
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == playerTag)
        {
           
            canBePickedUp = false;
        }
    }

    private void OnMouseDown()
    {
        if (canBePickedUp && item.currentState != Item.ItemState.Equipped)
        {
            item.AddToInventory();
        }
        else
        {
            pickUpCommandReceived = true;
        }
    }

    private void Update()
    {
        if (canBePickedUp && Input.GetKeyDown(KeyCode.F) && item.currentState != Item.ItemState.Equipped)
        {
            item.AddToInventory();
        }
    }
}
