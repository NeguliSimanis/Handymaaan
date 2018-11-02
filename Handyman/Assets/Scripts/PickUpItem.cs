using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [SerializeField]
    Item item;
    string playerTag = "Player";
    bool canBePickedUp = false; // can pick up item if player is in the collider area;


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == playerTag)
        {
            canBePickedUp = true;
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
        if (canBePickedUp)
        {
            Debug.Log("PICK UP");
            item.AddToInventory();
        }
    }
}
