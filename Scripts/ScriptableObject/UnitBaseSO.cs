using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBaseSO", menuName = "ScriptableObject/UnitBaseSO", order = 1)]
public class UnitBaseSO : ScriptableObject
{
    [Header("Base Data")]
    public float MoveSpeed;
    public float AttackRange;
    public float AttackInterval;
    public int Damage;
    public int MaxHp;

    [Header("Gold")]
    public int gold;
}
