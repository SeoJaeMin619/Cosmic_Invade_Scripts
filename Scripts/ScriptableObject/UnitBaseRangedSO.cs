using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitBaseRangedSO", menuName = "ScriptableObject/UnitBaseRangedSO", order = 2)]
public class UnitBaseRangedSO : UnitBaseSO
{
    [Header("Ranged Data")]
    public float ProjectileSpeed;
    public int NumberOfProjectile;
    public GameObject ProjectilePrefab;
}
