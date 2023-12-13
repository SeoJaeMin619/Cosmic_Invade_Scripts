using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int MaxHP;
    public int CurrnetHP;

    public virtual void Damaged(int dmg)
    {
        int calHp = (CurrnetHP -= dmg);
        if (calHp < 0)
            CurrnetHP = 0;
        else
            CurrnetHP = calHp;
    }
    public virtual void Healed(int heal)
    {
        int calHp = (CurrnetHP += heal);
        if(calHp > MaxHP)
            CurrnetHP = MaxHP;
        else
            CurrnetHP = calHp;
    }
}
