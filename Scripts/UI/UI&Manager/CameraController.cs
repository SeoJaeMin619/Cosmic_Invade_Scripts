using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController I;
    public Transform playerCamera;
    public Transform targetObject;
    private Vector3 originalPosition;
    [SerializeField] private GameObject playerInfoUi;
    [SerializeField] private GameObject nexusHP;
    public void Awake()
    {
        I = this;
    }
    private void Start()
    {
        originalPosition = transform.position;
    }


    public void PlayerCamera()
    {
        Vector3 newPosition = playerCamera.position;
        newPosition.z -= 10;
        transform.position = newPosition;
        TopDownPlayerMove.isCameraFollowing = true;
        CameraView.isCameraFollowing = false;
        playerInfoUi.gameObject.SetActive(true);
        nexusHP.gameObject.SetActive(false);

    }

    public void DefenceCamera()
    {

        Vector3 newPosition = targetObject.position;
        newPosition.z -= 10;
        transform.position = newPosition;
        TopDownPlayerMove.isCameraFollowing = false;
        CameraView.isCameraFollowing = true;
        playerInfoUi.gameObject.SetActive(false);
        nexusHP.gameObject.SetActive(true);
    }
}