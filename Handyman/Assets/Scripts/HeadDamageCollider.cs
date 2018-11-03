using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDamageCollider : MonoBehaviour
{
    [SerializeField]
    Item item;
    bool hasDealtDamage = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !hasDealtDamage)
        {
            hasDealtDamage = true;
            collision.gameObject.GetComponent<EnemyController>().TakeDamage((int)(item.attackDamage * 1.5f));      
        }
    }

    public void Reset()
    {
        hasDealtDamage = false;
    }


}
