using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKey : MonoBehaviour
{
    public GameObject targetObject; // ������Ʈ ������ �����Ϸ��� Inspector���� �Ҵ�
    //public GameObject targetObject2;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Tab Ű�� ������ ��
        if (Input.GetKeyDown(KeyCode.K))
        {
            // targetObject�� Ȱ��ȭ ���¸� ������Ŵ
            targetObject.SetActive(!targetObject.activeSelf);
            //targetObject2.SetActive(!targetObject2.activeSelf);
        }
    }
}
