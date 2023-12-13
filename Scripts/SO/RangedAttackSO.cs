using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackData", menuName = "ScriptableObject/AttackData",order =5)]

public class RangedAttackSO : ScriptableObject
{
    [Header("Ranged Attack Data")]
    public string bulletName;
    public float shootDuration;
    public float shootSpeed;
    public Color BulletColor;

}
