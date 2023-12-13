using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapphire : Item
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.sapphireCount++;
            Destroy(gameObject);        
        }
    }
}
