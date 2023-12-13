using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleAttackTest : MonoBehaviour
{
    public List<GameObject> gameObjects;
    void Start()
    {
        gameObjects = new List<GameObject>();

        StartCoroutine(CheckList());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameObjects.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameObjects.Remove(collision.gameObject);
    }

    IEnumerator CheckList()
    {
        while (true)
        {
            foreach (var obj in gameObjects)
            {
              Debug.Log(obj.name);
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
