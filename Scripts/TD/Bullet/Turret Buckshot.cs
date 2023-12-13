using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuckshot : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float explosionRadius = 2f; // 데미지 범위
    [SerializeField] private GameObject impactEffect;
    private Transform target;
    private int damage;

    // Update is called once per frame
    void Update()
    {
        MoveBullet();
    }

    public void Seek(Transform newTarget, int newDamage)
    {
        target = newTarget;
        damage = newDamage;
    }

    void MoveBullet()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(effectIns, 0.85f);

        //SoundManager.instance.SFXPlay("GunFire", 3);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var collider in colliders)
        {

            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Unit unit = collider.GetComponent<Unit>();
                if (unit != null)
                {
                    unit.Damaged(damage);
                }
            }
        }

        Destroy(gameObject);
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}