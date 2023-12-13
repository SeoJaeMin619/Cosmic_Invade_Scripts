using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour
{
    public ObjectSO objectSO;
    [SerializeField] private int objectHealth;
    [SerializeField] private List<GameObject> trapItems = new List<GameObject>();

    private void Awake()
    {
        objectHealth = objectSO.ObjectHp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            objectHealth -= 50;
            if (objectHealth <= 0)
            {
                Destroy(gameObject);
            }
            SpawnTraps();

        }
    }
    private void SpawnTraps()
    {
        foreach (GameObject trapItem in trapItems)
        {
            Instantiate(trapItem, transform.position, Quaternion.identity);           
        }
    }
}
