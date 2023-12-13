using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static SoundManager;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase groundTile;
    public TileBase oreTile1;
    public TileBase oreTile2;
    public TileBase oreTile3;

    public TileBase NonBreakable;

    public GameObject[] ItemPrefabs;

    public int width = 10;
    public int height = 10;
    public int oreRatio = 10; // 광석 타일의 비율

    // 각 oreTile의 생성 횟수를 저장하는 변수들
    private int oreTile1Count;
    private int oreTile2Count;
    private int oreTile3Count;


    private static MapGenerator instance;
    public static MapGenerator Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MapGenerator>();

                if (instance == null)
                {
                    // 맵 제너레이터가 씬에 없다면 새로 생성
                    GameObject obj = new GameObject("MapGenerator");
                    instance = obj.AddComponent<MapGenerator>();
                }
            }
            return instance;
        }
    }

    // 타일 체력을 저장할 딕셔너리
    private Dictionary<Vector3Int, int> tileHealthMap = new Dictionary<Vector3Int, int>();

    void Start()
    {
        GenerateMap();
        // 생성된 각 oreTile의 횟수를 출력
        Debug.Log($"oreTile1 Count: {oreTile1Count}");
        Debug.Log($"oreTile2 Count: {oreTile2Count}");
        Debug.Log($"oreTile3 Count: {oreTile3Count}");


        SetCenterTilesNull();
    }

    void Update()
    {
        // 사용자 입력 등을 처리할 수 있음
    }

    void GenerateMap()
    {
        Vector3Int startGridPosition = new Vector3Int(0, 0, 0);

        for (int i = 0; i <= height; i++)
        {
            for (int j = 0; j <= width; j++)
            {
                TileBase tile;

                if (i == 0 || j == 0 || i == height || j == width)
                {
                    tile = NonBreakable;
                    tilemap.SetTile(startGridPosition + new Vector3Int(j, -i, 0), tile);
                }
                else
                {
                    int randomValue = Random.Range(0, 4000);

                    if (randomValue < 100)
                    {
                        tile = oreTile1;
                        int initialTileHealth = 12;
                        CreateBreakableTile(tile, startGridPosition + new Vector3Int(j, -i, 0), initialTileHealth);
                        oreTile1Count++;
                    }
                    else if (randomValue < 200)
                    {
                        tile = oreTile2;
                        int initialTileHealth = 8;
                        CreateBreakableTile(tile, startGridPosition + new Vector3Int(j, -i, 0), initialTileHealth);
                        oreTile2Count++;
                    }
                    else if (randomValue < 300)
                    {
                        tile = oreTile3;
                        int initialTileHealth = 8;
                        CreateBreakableTile(tile, startGridPosition + new Vector3Int(j, -i, 0), initialTileHealth);
                        oreTile3Count++;
                    }

                    else
                    {
                        tile = groundTile;
                        int initialTileHealth = 3;
                        CreateGroundTile(tile, startGridPosition + new Vector3Int(j, -i, 0), initialTileHealth);
                    }
                }
            }
        }
    }

    void CreateBreakableTile(TileBase tile, Vector3Int position, int initialHealth)
    {
        tilemap.SetTile(position, tile);
        tileHealthMap[position] = initialHealth;
    }

    void CreateGroundTile(TileBase tile, Vector3Int position, int initialHealth)
    {
        tilemap.SetTile(position, tile);
        tileHealthMap[position] = initialHealth;
    }

    public void WrackTile(Vector3Int tilePos)
    {
        StartCoroutine(WrackTileWithDelay(tilePos));
    }

    IEnumerator WrackTileWithDelay(Vector3Int tilePos)
    {
        Vector3Int clickPos = tilePos;

        // 클릭한 위치의 타일이 체력이 남아있는지 확인
        if (tileHealthMap.ContainsKey(clickPos) && tileHealthMap[clickPos] > 0)
        {
            // 체력 감소
            tileHealthMap[clickPos]--;//인덱스 접근이 좋다.list로 쓸려면? 타일 포지션을 index로 변경, 이중배열

            // 체력이 0이 되었을 때
            if (tileHealthMap[clickPos] == 0)
            {
                // 부서진 타일이 oreTile1인 경우
                if (tilemap.GetTile(clickPos) == oreTile1)
                {
                    // 0번 프리팹 아이템 생성
                    Instantiate(ItemPrefabs[0], tilemap.GetCellCenterWorld(clickPos), Quaternion.identity);
                    DestroyTile(clickPos);
                }
                // 부서진 타일이 oreTile2인 경우
                else if (tilemap.GetTile(clickPos) == oreTile2)
                {
                    // 1번 프리팹 아이템 생성
                    Instantiate(ItemPrefabs[1], tilemap.GetCellCenterWorld(clickPos), Quaternion.identity);
                    DestroyTile(clickPos);
                }
                else if (tilemap.GetTile(clickPos) == oreTile3)
                {
                    // 2번 프리팹 아이템 생성
                    Instantiate(ItemPrefabs[2], tilemap.GetCellCenterWorld(clickPos), Quaternion.identity);
                    DestroyTile(clickPos);
                }


                else
                {
                    DestroyTile(clickPos);
                }
            }
        }
        yield return new WaitForSeconds(2f);// 대기시간
    }

    void DestroyTile(Vector3Int position)
    {
        // 타일 맵에서 제거
        tilemap.SetTile(position, null);
        // 딕셔너리에서 제거
        tileHealthMap.Remove(position);
        SoundManager.instance.SFXPlay(SFXType.crash);
    }

    void SetCenterTilesNull()
    {
        // 타일맵의 크기 계산
        BoundsInt bounds = tilemap.cellBounds;

        // 타일맵의 중앙 위치 계산
        Vector3Int center = new Vector3Int(bounds.x + bounds.size.x / 2, bounds.y + bounds.size.y / 2, bounds.position.z);

        // 3x3 영역을 순회하며 타일을 null로 설정
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                Vector3Int tilePosition = center + new Vector3Int(i, j, 0);
                tilemap.SetTile(tilePosition, null);
            }
        }
    }
}
