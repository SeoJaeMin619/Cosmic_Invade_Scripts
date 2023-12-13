using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    [Header("카메라 셋팅")]

    [Space]

    [SerializeField] private float moveSpeed = 10.0f;  // 카메라 이동 속도
    [SerializeField] private float zoomSpeed = 15.0f;  // 카메라 줌 속도
    [SerializeField] private float minOrthographicSize = 5.0f;  // 카메라 줌 최소 시야각
    [SerializeField] private float maxOrthographicSize = 20.0f;  // 카메라 줌 최대 시야각

    private Camera mainCamera;
    public static bool isCameraFollowing = false;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (isCameraFollowing)
        {
            // 마우스 스크롤 휠 입력값을 받음
            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            float newOrthographicSize = mainCamera.orthographicSize - scrollWheelInput * zoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(newOrthographicSize, minOrthographicSize, maxOrthographicSize);

            // 키보드 입력으로 카메라 이동
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // 카메라 이동 벡터
            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * moveSpeed * Time.deltaTime;
            Vector3 mousePosition = Input.mousePosition;

            float edgeDistance = 25.0f;

            if (mousePosition.x <= edgeDistance)
            {
                movement.x = -moveSpeed * Time.deltaTime;
            }
            else if (mousePosition.x >= Screen.width - edgeDistance)
            {
                movement.x = moveSpeed * Time.deltaTime;
            }

            if (mousePosition.y <= edgeDistance)
            {
                movement.y = -moveSpeed * Time.deltaTime;
            }
            else if (mousePosition.y >= Screen.height - edgeDistance)
            {
                movement.y = moveSpeed * Time.deltaTime;
            }

            transform.Translate(movement);
        }
    }
}