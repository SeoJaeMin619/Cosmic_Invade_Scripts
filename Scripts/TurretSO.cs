using UnityEngine;

[CreateAssetMenu(fileName = "TurretBaseSO", menuName = "ScriptableObject/TurretBaseSO", order = 4)]
public class TurretSO : ScriptableObject
{
    [Header(" # Turret ")]
    [Space]
    [SerializeField] private string turretName; //�ͷ� �̸�
    [SerializeField] private float turretRange; //Ÿ��ȸ��
    //[SerializeField] private float attackRange; //����
    [SerializeField] private float turretFireSpeed; //����

    [Header(" # Option ")]
    [Space]
    [SerializeField] private int turretCost; // �ͷ� ����
    [SerializeField] private float turretUpgradProbability; //��ȭ Ȯ��
    [SerializeField] private float turretBuildTime; //�Ǽ� �ӵ�
    [SerializeField] private int attackDamage; // ������
    [SerializeField] private int maxHP; //ü��

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
