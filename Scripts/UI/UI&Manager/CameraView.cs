using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{
    [Header("ī�޶� ����")]

    [Space]

    [SerializeField] private float moveSpeed = 10.0f;  // ī�޶� �̵� �ӵ�
    [SerializeField] private float zoomSpeed = 15.0f;  // ī�޶� �� �ӵ�
    [SerializeField] private float minOrthographicSize = 5.0f;  // ī�޶� �� �ּ� �þ߰�
    [SerializeField] private float maxOrthographicSize = 20.0f;  // ī�޶� �� �ִ� �þ߰�

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
            // ���콺 ��ũ�� �� �Է°��� ����
            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            float newOrthographicSize = mainCamera.orthographicSize - scrollWheelInput * zoomSpeed;
            mainCamera.orthographicSize = Mathf.Clamp(newOrthographicSize, minOrthographicSize, maxOrthographicSize);

            // Ű���� �Է����� ī�޶� �̵�
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // ī�޶� �̵� ����
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