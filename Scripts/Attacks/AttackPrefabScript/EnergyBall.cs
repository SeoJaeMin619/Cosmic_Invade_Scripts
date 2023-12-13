using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour , IAttack
{
    private int Damage;
    private LayerMask TargetLayer;
    private float ProjectileSpeed;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Collider2D Collider2D;
    private Coroutine DestroyDelay;

    public void SetDamage(int Damage)
    {
        this.Damage = Damage;
    }

    public void SetDirection(Vector3 Direction)
    {
        rb2D.velocity = Direction * ProjectileSpeed;
        DestroyDelay = StartCoroutine(DestroyAfterDelay(5f));
    }

    public void SetProjectileSpeed(float ProjectileSpeed)
    {
        this.ProjectileSpeed = ProjectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            if (collision.TryGetComponent<Unit>(out Unit unit))
            {
                unit.Damaged(Damage);
            }
            Collider2D.enabled = false;
            StopCoroutine(DestroyDelay);

            if (TurretInfo.Load.TryGetValue(collision.gameObject, out TurretInfo Turret))
            {
                Turret.CurrentHP = Turret.CurrentHP - Damage;
                if (Turret.CurrentHP <= 0)
                {
                    TurretManager.check.Remove(Turret.Cell);
                  
                    if (collision.gameObject.TryGetComponent<HpSlider>(out var hpSlider))
                    {
                        Destroy(hpSlider.hpslider);
                    }
                    Destroy(collision.gameObject);
                }
            }
            Destroy(gameObject);
        }

    }

    public void SetTargetLayer(LayerMask TargetLayer)
    {
        this.TargetLayer = TargetLayer;
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
