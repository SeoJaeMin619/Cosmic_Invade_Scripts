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
        // BoxCollider2D 컴포넌트를 가져옴
        weaponCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 좌향 입력
        if (horizontalInput < 0)
        {
            weaponPivot.localPosition = new Vector3(-3f, 0f, 0f);
            // 스프라이트를 좌측으로 회전
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
        // 우향 입력
        else if (horizontalInput > 0)
        {
            weaponPivot.localPosition = new Vector3(3f, 0f, 0f);
            // 스프라이트를 우측으로 회전
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        // 상향 입력
        else if (verticalInput > 0)
        {
            weaponPivot.localPosition = new Vector3(0f, 3f, 0f);
            // 스프라이트를 위로 회전
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        // 하향 입력
        else if (verticalInput < 0)
        {
            weaponPivot.localPosition = new Vector3(0f, -3f, 0f);
            // 스프라이트를 아래로 회전
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


        if (TopDownPlayerMove.isCameraFollowing)
        {
            // 스페이스 키를 누를 때 무기 작동
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
            // 플레이어가 드릴 작업을 수행하지 않으면 타이머를 리셋
            drillTimer = 0f;
        }
    }

    void HitGround()
    {
        // 무기의 현재 위치를 타일맵 좌표로 변환
        Vector3Int weaponTilePos = tilemap.WorldToCell(weaponPivot.position);

        // 주위 1만큼 위치에 있는 타일들의 좌표를 얻기
        int roundedX = Mathf.RoundToInt(weaponTilePos.x);
        int roundedY = Mathf.RoundToInt(weaponTilePos.y);

        Vector3Int tilePos = new Vector3Int(roundedX, roundedY, weaponTilePos.z);

        // MapGenerator에서 WrackTile 호출하여 타일 체력 깎기
        MapGenerator.Instance.WrackTile(tilePos);
    }

}

