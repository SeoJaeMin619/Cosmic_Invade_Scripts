using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardObject : MonoBehaviour
{
    public ObjectSO objectSO;
    [SerializeField] private int objectHealth;
    [SerializeField] private List<GameObject> rewardItems = new List<GameObject>();

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
                SpawnRewardItems();
            }
        }
    }

    private void SpawnRewardItems()
    {
        foreach (GameObject rewardItem in rewardItems)
        {
            Instantiate(rewardItem, transform.position, Quaternion.identity);
        }
    }
}
