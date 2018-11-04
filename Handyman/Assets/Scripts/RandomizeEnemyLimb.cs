using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeEnemyLimb : MonoBehaviour
{

    //[SerializeField]
    public Sprite[] limbVariations;
    SpriteRenderer spriteRenderer;
    Item item;

    private void Start()
    {
        if (transform.parent.gameObject.GetComponent<Item>() != null)
        {
            item = transform.parent.gameObject.GetComponent<Item>();
            item.itemImage = limbVariations[Random.Range(0, limbVariations.Length - 1)];
        }
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = limbVariations[Random.Range(0, limbVariations.Length - 1)];
        
    }
}
