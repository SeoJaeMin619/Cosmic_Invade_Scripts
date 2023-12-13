using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, IAttack
{
    public int Damage;
    public LayerMask TargetLayer;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Collider2D Collider2D;
    private Coroutine DestroyDelay;
   public void SetDamage(int Damage)
   {
       this.Damage = Damage;
   }

    public void SetDirection(Vector3 Direction)
    {
        rb2D.velocity = Direction;
        DestroyDelay = StartCoroutine(DestroyAfterDelay(3f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            Unit unit = collision.gameObject.GetComponent<Unit>();
            unit.Damaged(Damage);
            Collider2D.enabled = false;
            StopCoroutine(DestroyDelay);
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
