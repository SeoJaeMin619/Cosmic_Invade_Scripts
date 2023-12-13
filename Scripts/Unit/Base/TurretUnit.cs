using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class TurretUnit : MonoBehaviour
{

    public int maxhp;
    public int currenthp;

    public virtual void Damaged(int dmg)
    {
        int calHp = (currenthp -= dmg);
        if (calHp < 0)
            currenthp = 0;
        else
            currenthp = calHp;
    }
    //public virtual void Healed(int heal)
    //{
    //    int calHp = (currenthp += heal);
    //    if (calHp > maxHP)
    //        currnetHP = maxHP;
    //    else
    //        currnetHP = calHp;
    //}

}
