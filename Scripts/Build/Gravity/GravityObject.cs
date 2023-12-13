using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] private float repulsionForce; 
    [SerializeField] private float maxRepulsionDistance= 5.0f; 
    [SerializeField] private Transform portal;
    [SerializeField] private TextMeshProUGUI deductedGoldText;


    private Transform player;
    private Rigidbody2D playerRigidbody;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRigidbody = player.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

       
        if (player != null && playerRigidbody != null)
        {
            
            Vector2 directionToPlayer = player.position - transform.position;
            
            float distance = directionToPlayer.magnitude;

           
            if (distance < maxRepulsionDistance)
            {
               
                float repulsionStrength = (maxRepulsionDistance - distance) * repulsionForce;               
                Vector2 force = directionToPlayer.normalized * repulsionStrength;                
                playerRigidbody.AddForce(-force); 
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int curGold = GameManager.goldcount;
            int reduceGold = Mathf.RoundToInt(curGold * 0.1f);
            GameManager.goldcount -= reduceGold;

            UpdateDeductedGoldText(reduceGold);

            player.position = portal.position;
        }
    }

    private void UpdateDeductedGoldText(int deductedGold)
    {
        if (deductedGoldText != null)
        {
            deductedGoldText.text = "- " + deductedGold.ToString();
            deductedGoldText.gameObject.SetActive(true);
            StartCoroutine(FadeOutText(deductedGoldText));
        }
        else
        {
            Debug.LogWarning("텍스트가 존재하지 않아요!");
        }
    }

    IEnumerator FadeOutText(TextMeshProUGUI text)
    {
        float startAlpha = text.alpha;
        float elapsedTime = 0f;
        float fadeDuration = 1f;

        while (elapsedTime < fadeDuration)
        {
            text.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        text.alpha = 1f;
        text.gameObject.SetActive(false);
    }
}
