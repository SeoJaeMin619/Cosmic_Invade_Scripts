using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKey : MonoBehaviour
{
    public GameObject targetObject; // 오브젝트 참조를 연결하려면 Inspector에서 할당
    //public GameObject targetObject2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Tab 키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.K))
        {
            // targetObject의 활성화 상태를 반전시킴
            targetObject.SetActive(!targetObject.activeSelf);
            //targetObject2.SetActive(!targetObject2.activeSelf);
        }
    }
}
