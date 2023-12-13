using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretIceShot : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject impactEffect;
    private Transform target;
    private int damage;

    public void Seek(Transform target, int damage)
    {
        this.target = target;
        this.damage = damage;
    }

    // Update is called once per frame
    void Update()
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
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(effectIns, 0.35f);
        //SoundManager.instance.SFXPlay("GunFire", 3);

        Unit unit = target.GetComponent<Unit>();
        if (unit != null)
        {

            Renderer renderer = unit.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(0.5f, 0.5f, 1f);
            }
         
            //stats.SlowDown();

            //unit.Damaged(damage);
        }

        Destroy(gameObject);
    }
}