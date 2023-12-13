using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObject/BuildingData", order = 4)]



public class BuildingSO : ScriptableObject
{
    [SerializeField]
    private Sprite buildingSprite;
    public Sprite BuildingSprite { get { return buildingSprite; } }

    // �ǹ� �������� �ӵ�
    [SerializeField]
    private float buildSpeed;
    public float BuildSpeed { get { return buildSpeed; } }
    // �ǹ� ����
    [SerializeField]
    private int buildCost;
    public int BuildCost { get { return buildCost; } }

    // �ǹ� ü��
    [SerializeField]
    private int buildHP;
    public int BuildHP { get { return buildHP; } }

    // �ǹ� �ڿ� �߰� �ӵ�
    [SerializeField]
    private int addResource;
    public int AddResource{  get { return addResource; } }
}
