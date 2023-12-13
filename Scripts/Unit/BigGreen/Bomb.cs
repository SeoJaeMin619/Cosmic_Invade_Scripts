using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IAttack
{
    private LayerMask TargetLayer;
    private int Damage;

    private void Start()
    {
        StartCoroutine(CoDestroyTimer());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            if (collision.gameObject.TryGetComponent<Unit>(out Unit unit))
            {
                unit.Damaged(Damage);

                if(TurretInfo.Load.TryGetValue(collision.gameObject, out TurretInfo Turret))
                {
                    Turret.CurrentHP -= Damage;
                    if (Turret.CurrentHP <= 0)
                    {
                        TurretManager.check.Remove(Turret.Cell);
                        if (collision.TryGetComponent<HpSlider>(out HpSlider hpslider))
                        {
                            Destroy(hpslider.hpslider);
                        }
                        Destroy(collision.gameObject);
                     
                    }
                }
            }
        }
    }

    private IEnumerator CoDestroyTimer()
    {

        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }


    public void SetTargetLayer(LayerMask TargetLayer) => this.TargetLayer = TargetLayer;

    public void SetDirection(Vector3 Direction)
    {
       
    }

    public void SetDamage(int Damage) => this.Damage = Damage;
}

  
