using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Onoff : MonoBehaviour
{
    public GameObject skill;

    bool pause = false;
    public void onoff()
    {
        if (skill.activeSelf == false)
        {
            skill.SetActive(true); // ��ũ��Ʈ�� Ȱ��ȭ
        }
        else
        {
            skill.SetActive(false); // ��ũ��Ʈ�� ��Ȱ��ȭ
        }
    }
}
