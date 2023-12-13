using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Rock"))
        {            
            Destroy(gameObject);
        }
    }
}
