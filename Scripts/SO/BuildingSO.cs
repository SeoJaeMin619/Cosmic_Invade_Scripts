using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObject/BuildingData", order = 4)]



public class BuildingSO : ScriptableObject
{
    [SerializeField]
    private Sprite buildingSprite;
    public Sprite BuildingSprite { get { return buildingSprite; } }

    // 건물 지어지는 속도
    [SerializeField]
    private float buildSpeed;
    public float BuildSpeed { get { return buildSpeed; } }
    // 건물 가격
    [SerializeField]
    private int buildCost;
    public int BuildCost { get { return buildCost; } }

    // 건물 체력
    [SerializeField]
    private int buildHP;
    public int BuildHP { get { return buildHP; } }

    // 건물 자원 추가 속도
    [SerializeField]
    private int addResource;
    public int AddResource{  get { return addResource; } }
}
