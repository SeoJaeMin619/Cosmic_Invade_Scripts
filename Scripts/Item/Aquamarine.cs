using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aquamarine : Item
{
 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {         
                GameManager.aquamarineCount++;
                Destroy(gameObject); 
        }
    }
}
