using UnityEngine;
using UnityEngine.Tilemaps;
using static SoundManager;

public class PlayerDrill : MonoBehaviour
{


    public Tilemap tilemap;
    public Transform weaponPivot;
    private BoxCollider2D weaponCollider;

    private bool isLoopSFXPlaying = false;

    public static bool IsDrilling = false;
    private float drillTimer = 0f;
    private float drillCooldown = 0.25f;

    void Start()
    {
        // BoxCollider2D ������Ʈ�� ������
        weaponCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // ���� �Է�
        if (horizontalInput < 0)
        {
            weaponPivot.localPosition = new Vector3(-3f, 0f, 0f);
            // ��������Ʈ�� �������� ȸ��
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        // ���� �Է�
        else if (horizontalInput > 0)
        {
            weaponPivot.localPosition = new Vector3(3f, 0f, 0f);
            // ��������Ʈ�� �������� ȸ��
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        // ���� �Է�
        else if (verticalInput > 0)
        {
            weaponPivot.localPosition = new Vector3(0f, 3f, 0f);
            // ��������Ʈ�� ���� ȸ��
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        // ���� �Է�
        else if (verticalInput < 0)
        {
            weaponPivot.localPosition = new Vector3(0f, -3f, 0f);
            // ��������Ʈ�� �Ʒ��� ȸ��
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


        if (TopDownPlayerMove.isCameraFollowing)
        {
            // �����̽� Ű�� ���� �� ���� �۵�
            if (Input.GetKey(KeyCode.Z))
            {
                if (drillTimer >= drillCooldown)
                {
                    drillTimer = 0f;
                    HitGround();
                    IsDrilling = true;
                    SoundManager.instance.DrillSFXPlay(SFXType.drill);
                }
                else
                {
                    drillTimer += Time.deltaTime;
                }
            }
            else
            {
                IsDrilling = false;
                isLoopSFXPlaying = false;
                SoundManager.ExistDrill = false;
            }
        }
        else
        {
            // �÷��̾ �帱 �۾��� �������� ������ Ÿ�̸Ӹ� ����
            drillTimer = 0f;
        }
    }

    void HitGround()
    {
        // ������ ���� ��ġ�� Ÿ�ϸ� ��ǥ�� ��ȯ
        Vector3Int weaponTilePos = tilemap.WorldToCell(weaponPivot.position);

        // ���� 1��ŭ ��ġ�� �ִ� Ÿ�ϵ��� ��ǥ�� ���
        int roundedX = Mathf.RoundToInt(weaponTilePos.x);
        int roundedY = Mathf.RoundToInt(weaponTilePos.y);

        Vector3Int tilePos = new Vector3Int(roundedX, roundedY, weaponTilePos.z);

        // MapGenerator���� WrackTile ȣ���Ͽ� Ÿ�� ü�� ���
        MapGenerator.Instance.WrackTile(tilePos);
    }

}

