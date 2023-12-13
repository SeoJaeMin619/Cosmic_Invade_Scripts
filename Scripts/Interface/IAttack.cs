using UnityEngine;

public interface IAttack
{
    public void SetTargetLayer(LayerMask TargetLayer);

    public void SetDirection(Vector3 Direction);

    public void SetDamage(int Damage);

}
