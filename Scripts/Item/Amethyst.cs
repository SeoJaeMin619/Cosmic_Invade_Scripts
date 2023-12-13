using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amethyst : Item
{       

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.amethystCount++;            
            Destroy(gameObject);
        }
        
    }
}
