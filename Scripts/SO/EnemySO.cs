using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableData", order = int.MinValue)]
public class EnemySO : ScriptableObject
{
    [SerializeField]
    private Sprite enemySprite;
    public Sprite EnemySprite { get { return enemySprite; } }

    [SerializeField]
    private string enemyName;
    public string EnemyName { get { return enemyName; } }   

    [SerializeField]
    private int enemyHp;
    public int EnemyHp{  get { return enemyHp; } }

    [SerializeField]
    private int enemyDmg;
    public int EnemyDmg {  get { return enemyDmg; } }

    [SerializeField]
    private float enemySpeed;
    public float EnemySpeed {  get { return enemySpeed; } }

    [SerializeField]
    private float attackRange;
    public float AttackRange { get { return attackRange; } }

}
