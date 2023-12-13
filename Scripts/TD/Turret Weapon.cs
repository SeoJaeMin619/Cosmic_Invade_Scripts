using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class TurretWeapon : MonoBehaviour
{
    [SerializeField] private TurretSO Data;

    private Transform target;

    [Header("# Turret")]
    [Space]
    private float turretRange;
    private float turretFireSpeed;
    private int attackDamage;

    private float fireCountdown = 0f;

    [Header("# Setup")]
    [Space]
    public string enemyTag = "Enemy";
    [SerializeField] private float trunSpeed = 10f;
    [SerializeField] private Transform rotatePart;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    private void Awake()
    {
        //스크럽터블 

        turretRange = Data.TurretRange();
        turretFireSpeed = Data.TurretFireSpeed();
        attackDamage = Data.AttackDamage();

    }
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= turretRange)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.Euler(0, 0, angle);
        rotatePart.rotation = Quaternion.Lerp(rotatePart.rotation, lookRotation, Time.deltaTime * trunSpeed);

        if (fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f / turretFireSpeed;
        }

        fireCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        GameObject bullets = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        TurretBullet turretBullet = bullets.GetComponent<TurretBullet>();
        if (turretBullet != null)
        {
            turretBullet.Seek(target, attackDamage);
        }
        TurretBuckshot turretBuckshot = bullets.GetComponent<TurretBuckshot>();
        if (turretBuckshot != null)
        {
            turretBuckshot.Seek(target, attackDamage);
        }
        TurretIceShot turretIceshot = bullets.GetComponent<TurretIceShot>();
        if (turretIceshot != null)
        {
            turretIceshot.Seek(target, attackDamage);
        }
    }

    // 타워 레인지 범위
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, turretRange);
    }

    public void AddBuff(int plus)
    {
        attackDamage += plus;
        TurretInfo This =  TurretInfo.Temp;
        This.Atk = This.Atk + plus;
    }

    public void RemoveBuff(int plus)
    {
        attackDamage = Data.AttackDamage();
        TurretInfo This = TurretInfo.Temp;
        This.Atk = This.Atk - plus;
    }
}

