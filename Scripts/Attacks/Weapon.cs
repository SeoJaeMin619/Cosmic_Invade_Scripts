using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public LayerMask TargetLayer;
    public int Damage;

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
        
            if (collision.gameObject.TryGetComponent<Unit>(out var unit))
                unit.Damaged(Damage);

            if (TurretInfo.Load.TryGetValue(collision.gameObject, out TurretInfo Turret))
            {
                Turret.CurrentHP -= Damage;
                if (Turret.CurrentHP <= 0)
                {
                   if(collision.gameObject.TryGetComponent<HpSlider>(out var hpSlider))
                   {
                        Destroy(hpSlider.hpslider);
                      
                   }
                    TurretManager.check.Remove(Turret.Cell);
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}
