using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurretBullet : MonoBehaviour
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

    //GameObject Geteffect()
    //{
    //    if (effects.Count > 0)
    //    {   
    //        Debug.Log("¤±¤¤¤·¤«¤¤¤·¤©¤·¤±¤¤¤©¤·¤¤¤±");
    //        GameObject re = effects.First();
    //        re.SetActive(true);
    //        re.transform.position = transform.position;
    //        effects.Remove(re);
    //        return re;
    //    }

    //    return Instantiate(impactEffect, transform.position, Quaternion.identity);
    //}

    //IEnumerator Trash(GameObject effect)
    //{
    //    yield return new WaitForSeconds(0.35f);
    //    effect.SetActive(false);
    //    effects.Add(effect);
    //    Debug.Log("¤©¤¤¤·¤©¸ç¤Ç¤¤98¤µ‘¢4¤Ä89¤µ4¤Ë3¤¸9¤µ¤±ÁÒ3¤§09¤µ¤¾¤¸");
    //    Destroy(gameObject);
    //}

    void HitTarget()
    {
        //if (!task)
        //{
        //    task = true;
        //    GameObject effectIns = Geteffect();
        //    StartCoroutine(Trash(effectIns));
        //    gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        //    //SoundManager.instance.SFXPlay("GunFire", 3);

        //    Unit unit = target.GetComponent<Unit>();
        //    if (unit != null)
        //    {
        //        unit.Damaged(damage);
        //    }
        //}
        GameObject effectIns = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(effectIns, 0.35f);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        //SoundManager.instance.SFXPlay("GunFire", 3);

        Unit unit = target.GetComponent<Unit>();
        if (unit != null)
        {
            unit.Damaged(damage);
        }

        Destroy(gameObject);
    }
}