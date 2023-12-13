using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruby : Item
{   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {           
                GameManager.rubyCount++;
                Destroy(gameObject);          
        }
    }
}
