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
            skill.SetActive(true); // 스크립트를 활성화
        }
        else
        {
            skill.SetActive(false); // 스크립트를 비활성화
        }
    }
}
