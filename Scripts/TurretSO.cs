using UnityEngine;

[CreateAssetMenu(fileName = "TurretBaseSO", menuName = "ScriptableObject/TurretBaseSO", order = 4)]
public class TurretSO : ScriptableObject
{
    [Header(" # Turret ")]
    [Space]
    [SerializeField] private string turretName; //터렛 이름
    [SerializeField] private float turretRange; //타워회전
    //[SerializeField] private float attackRange; //범위
    [SerializeField] private float turretFireSpeed; //공속

    [Header(" # Option ")]
    [Space]
    [SerializeField] private int turretCost; // 터렛 가격
    [SerializeField] private float turretUpgradProbability; //강화 확률
    [SerializeField] private float turretBuildTime; //건설 속도
    [SerializeField] private int attackDamage; // 데미지
    [SerializeField] private int maxHP; //체력

    public string TurretName() { return turretName; }
    public float TurretUpgradProbability() { return turretUpgradProbability; }
    public float TurretRange() {  return turretRange; }
    //public float AttackRange() { return attackRange;}
    public float TurretFireSpeed() { return turretFireSpeed;}
    public float TurretBuildTime() { return turretBuildTime; }
    public int AttackDamage() {  return attackDamage;}
    public int TurretCost() { return turretCost;}
    public int MaxHP() { return maxHP; }
}
