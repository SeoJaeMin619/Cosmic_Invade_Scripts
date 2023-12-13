using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject toTarget;
     
    IEnumerator PortalRoutine()
    {
        yield return null;
        Vector3 newPosition = toTarget.transform.position + new Vector3(1.5f, 1.5f, 0.0f);
        target.transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            target = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(PortalRoutine());
        }
    }
}
