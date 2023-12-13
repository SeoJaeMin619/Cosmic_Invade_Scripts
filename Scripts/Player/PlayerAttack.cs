using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static SoundManager;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    
    [SerializeField] KeyCode attackKey = KeyCode.Space;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 20f;    
    [SerializeField] private float timeSinceLastShoot = 0f;
    public static float shootinterval = 0.3f;

    [Header("Reload")]    
    public static int maxBulletQuantity = 50;
    public static int curBulletQuantity;
    public static float reloadSpeed = 3f;
    [SerializeField] private Image reloadFillImage;
    [SerializeField] private TextMeshProUGUI maxBulletTxt;
    [SerializeField] private TextMeshProUGUI curBulletTxt;
    [SerializeField] private GameObject reloadPanel;

    private bool isReload = false;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        curBulletQuantity = maxBulletQuantity;
        reloadPanel.SetActive(isReload);
        UpdateBulletUI();

    }    

    void Update()
    {
        timeSinceLastShoot += Time.deltaTime;

        float horizonInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if ((Input.GetKey(attackKey) && timeSinceLastShoot >= shootinterval && !isReload && (horizonInput != 0 || verticalInput != 0)))
        {
            Shoot();
            animator.SetBool("IsAttack", true);
            timeSinceLastShoot = 0f;
        }

        if (Input.GetKeyUp(attackKey))
        {
            animator.SetBool("IsAttack", false);
        }

        if(Input.GetKeyUp(KeyCode.R) && curBulletQuantity < maxBulletQuantity && !isReload)
        {
            StartCoroutine(Reload());
        }
        UpdateBulletUI();
    }

    private void Shoot()
    {
        float horizonInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 fireDirection = new Vector2(horizonInput, verticalInput).normalized;

        if (fireDirection != Vector2.zero && curBulletQuantity > 0)
        {
            GameObject projectile = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D projectileRigid = projectile.GetComponent<Rigidbody2D>();

            if (projectileRigid != null)
            {
                projectileRigid.velocity = fireDirection * projectileSpeed;
                Destroy(projectile, 0.5f);
                curBulletQuantity--;
                UpdateBulletUI();
            }
        }
        SoundManager.instance.SFXPlay(SFXType.shot);
    }

    private IEnumerator Reload()
    {
        isReload = true;
        reloadPanel.SetActive(true);

        float timer = 0f;

        while (timer < reloadSpeed)
        {
            float fillAmount = Mathf.Lerp(0f, 1f, timer / reloadSpeed);
            reloadFillImage.fillAmount = fillAmount;

            timer += Time.deltaTime;
            yield return null;
        }

        curBulletQuantity = maxBulletQuantity;
        UpdateBulletUI();

        reloadPanel.SetActive(false);
        isReload = false;
        reloadFillImage.fillAmount = 0f;  
    }
    public void UpdateBulletUI()
    {
        curBulletTxt.text = curBulletQuantity.ToString();
        maxBulletTxt.text = maxBulletQuantity.ToString();
    }
}