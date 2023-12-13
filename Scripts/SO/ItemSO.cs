using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ItemData", order = 2)]
public class ItemSO : ScriptableObject
{
    public enum ItmeType
    {
        TowerUpgrade,
        SkillUpgrade               
    }

    public ItmeType itemType;

    public string itemName;
    public Sprite itemIcon;
    public int itemPrice;
    
   
}
