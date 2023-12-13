using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ObjectDta", menuName = "ScriptableObject/ObjectData", order = 5)]

public class ObjectSO : ScriptableObject
{
    public enum ObjectType
    {
        trap,
        reward
    }

    [SerializeField]
    private Sprite objectSprite;
    public Sprite ObjectSprite { get { return objectSprite; } }
    [SerializeField]
    private string objectName;
    public string ObjectName {  get { return objectName; } }

    [SerializeField]
    private int objectHp;
    public int ObjectHp { get; set; }

    [SerializeField]
    private GameObject dropItem;
    public GameObject DropItem { get { return dropItem;} }
}
