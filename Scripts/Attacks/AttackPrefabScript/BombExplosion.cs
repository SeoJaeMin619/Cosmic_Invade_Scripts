using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    public LayerMask TargetLayer;
    public int Damage;

    public void SetTargetAndDamage(LayerMask TargetLayer, int Damage)
    {
        this.TargetLayer = TargetLayer;
        this.Damage = Damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer) != 0)
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();
            unit.Damaged(Damage);
            Invoke(nameof(DestroyObject), 0.3f);
        }
    }

    private void DestroyObject() => Destroy(this.gameObject);
}
