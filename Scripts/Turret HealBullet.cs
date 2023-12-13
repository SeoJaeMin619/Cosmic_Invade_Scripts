using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurretHealBullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject impactEffect;
    private Transform target;
    private int damage;
    private static List<GameObject> effects = new List<GameObject>();
    private bool task = false;

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

        //GameObject effectIns = Instantiate(impactEffect, transform.position, Quaternion.identity);
        //Destroy(effectIns, 0.35f);
        //gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        //SoundManager.instance.SFXPlay("GunFire", 3);

        Debug.Log(target);

        if (TurretInfo.Load.ContainsKey(target.gameObject))
        {
            TurretInfo This = TurretInfo.Load[target.gameObject];
            TurretInfo.Load.Remove(target.gameObject);
            This.CurrentHP += damage;
        }

        Destroy(gameObject);

    }
}