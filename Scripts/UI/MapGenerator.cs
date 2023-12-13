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
    public int oreRatio = 10; // ���� Ÿ���� ����

    // �� oreTile�� ���� Ƚ���� �����ϴ� ������
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
                    // �� ���ʷ����Ͱ� ���� ���ٸ� ���� ����
                    GameObject obj = new GameObject("MapGenerator");
                    instance = obj.AddComponent<MapGenerator>();
                }
            }
            return instance;
        }
    }

    // Ÿ�� ü���� ������ ��ųʸ�
    private Dictionary<Vector3Int, int> tileHealthMap = new Dictionary<Vector3Int, int>();

    void Start()
    {
        GenerateMap();
        // ������ �� oreTile�� Ƚ���� ���
        Debug.Log($"oreTile1 Count: {oreTile1Count}");
        Debug.Log($"oreTile2 Count: {oreTile2Count}");
        Debug.Log($"oreTile3 Count: {oreTile3Count}");


        SetCenterTilesNull();
    }

    void Update()
    {
        // ����� �Է� ���� ó���� �� ����
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

        // Ŭ���� ��ġ�� Ÿ���� ü���� �����ִ��� Ȯ��
        if (tileHealthMap.ContainsKey(clickPos) && tileHealthMap[clickPos] > 0)
        {
            // ü�� ����
            tileHealthMap[clickPos]--;//�ε��� ������ ����.list�� ������? Ÿ�� �������� index�� ����, ���߹迭

            // ü���� 0�� �Ǿ��� ��
            if (tileHealthMap[clickPos] == 0)
            {
                // �μ��� Ÿ���� oreTile1�� ���
                if (tilemap.GetTile(clickPos) == oreTile1)
                {
                    // 0�� ������ ������ ����
                    Instantiate(ItemPrefabs[0], tilemap.GetCellCenterWorld(clickPos), Quaternion.identity);
                    DestroyTile(clickPos);
                }
                // �μ��� Ÿ���� oreTile2�� ���
                else if (tilemap.GetTile(clickPos) == oreTile2)
                {
                    // 1�� ������ ������ ����
                    Instantiate(ItemPrefabs[1], tilemap.GetCellCenterWorld(clickPos), Quaternion.identity);
                    DestroyTile(clickPos);
                }
                else if (tilemap.GetTile(clickPos) == oreTile3)
                {
                    // 2�� ������ ������ ����
                    Instantiate(ItemPrefabs[2], tilemap.GetCellCenterWorld(clickPos), Quaternion.identity);
                    DestroyTile(clickPos);
                }


                else
                {
                    DestroyTile(clickPos);
                }
            }
        }
        yield return new WaitForSeconds(2f);// ���ð�
    }

    void DestroyTile(Vector3Int position)
    {
        // Ÿ�� �ʿ��� ����
        tilemap.SetTile(position, null);
        // ��ųʸ����� ����
        tileHealthMap.Remove(position);
        SoundManager.instance.SFXPlay(SFXType.crash);
    }

    void SetCenterTilesNull()
    {
        // Ÿ�ϸ��� ũ�� ���
        BoundsInt bounds = tilemap.cellBounds;

        // Ÿ�ϸ��� �߾� ��ġ ���
        Vector3Int center = new Vector3Int(bounds.x + bounds.size.x / 2, bounds.y + bounds.size.y / 2, bounds.position.z);

        // 3x3 ������ ��ȸ�ϸ� Ÿ���� null�� ����
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
