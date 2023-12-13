using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilBomb : Weapon
{
    private void Start()
    {
        StartCoroutine(CoDestroyTimer());
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
