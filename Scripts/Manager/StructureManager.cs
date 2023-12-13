using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
public class StructureClick : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (TurretManager.build < 0)
        {

            TurretInfo That = TurretInfo.Load[gameObject];

            Vector3 cellCenterWorld = TurretManager.tileMap.GetCellCenterWorld(That.Cell);
            RectTransform popupRectTransform = TurretManager.popupUIst.GetComponent<RectTransform>();
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, cellCenterWorld);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(popupRectTransform.parent as RectTransform, screenPos, null, out Vector2 localPos);
            popupRectTransform.anchoredPosition = localPos;
            TurretManager.popupUIst.SetActive(true);
            Invoke("PopUI", 3f);
            TurretInfo.Temp = That;
        }
    }
    void PopUI()
    {
        TurretManager.popupUIst.SetActive(false);
    }


    public static void StructureState()
    {
        TurretInfo That = TurretInfo.Temp;

        TurretManager.nameTxtst.text = That.Name;
        TurretManager.stateTxtst.text = " [건물 HP]  ::  " + That.CurrentHP + " / " + That.HP +
            "\n\n [건물 판매가격]  ::  " + (int)(That.Cost * 0.8f) + "\n [건물 업그레이드 가격]  ::  " + That.Cost;
    }
    public void StructureSell()
    {
        TurretInfo That = TurretInfo.Temp;
        if (That.Structure != null)
        {
            Destroy(That.Structure);
            Destroy(That.Structure.gameObject.GetComponent<HpSlider>().hpslider);
            TurretManager.check.Remove(That.Cell);
            GameManager.goldcount += (int)(That.Cost * 0.8f);
        }
        else
            TurretManager.I.textfadeout.DisplayErrorMessage("건물이 존재하지 않습니다.");
    }
    public void Structureinfo()
    {
        TurretManager.popupStatest.SetActive(true);
    }

}

public class Upgrades
{

    public void TurretUpgrad()
    {
        TurretInfo That = TurretInfo.Temp;

        //TurretManager.turretUpgradButtonst[0].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        if (That.Level < 6)
        {
            if (GameManager.goldcount >= That.Cost)
            {
                if (That.Structure != null)
                {

                    GameManager.goldcount -= That.Cost;
                    TurretClick.Destroy(That.Structure);
                    TurretManager.I.turretList.Remove(That.Structure);
                    TurretClick.Destroy(That.Structure.gameObject.GetComponent<HpSlider>().hpslider);
                    That.Level++;

                    switch (That.Level)
                    {
                        case 1:
                            That.Structure = TurretClick.Instantiate(TurretManager.turretLV2[(int)That.Number], TurretManager.tileMap.GetCellCenterWorld(That.Cell), Quaternion.identity);
                            //That.HP = StructureManager.I.Lv2[That.Number].MaxHP();

                            //This.Atk = TurretManager.I.Lv2[This.Number].AttackDamage();
                            //This.TurretRange = TurretManager.I.Lv2[This.Number].TurretRange();
                            //This.TurretFireSpeed = TurretManager.I.Lv2[This.Number].TurretFireSpeed();
                            //This.TurretProbability = TurretManager.I.Lv2[This.Number].TurretUpgradProbability();
                            //This.Cost = TurretManager.I.Lv2[This.Number].TurretCost();
                            //This.Name = TurretManager.I.Lv2[This.Number].TurretName();
                            //This.Turret.AddComponent<TurretClick>();
                            ////TurretManager.popupUIst.SetActive(false);
                            break;
                        case 2:

                            break;
                        case 3:
     
                            break;
                        case 4:
 
                            break;
                        case 5:

                            break;
                        case 6:
     
                            break;
                    }

                    That.Turret.AddComponent<HpSlider>();
                    TurretInfo.Load[That.Structure] = That;
                    That.CurrentHP = That.HP;


            }
                else
                    TurretManager.I.textfadeout.DisplayErrorMessage("타워가 존재하지 않거나, 최대 레벨입니다.");
            }
            else
                TurretManager.I.textfadeout.DisplayErrorMessage("돈이 부족합니다.");
        }
        else
            TurretManager.I.textfadeout.DisplayErrorMessage("타워가 최대 레벨입니다.");
    }


}

public class StructureManager : MonoBehaviour
{
    public static StructureManager I;

    [Header("Structure Set")]
    [Space]
    [SerializeField] private GameObject[] Structure;
    [SerializeField] private GameObject[] StructurePreview;
    public static GameObject structurePreview;  // 수정된 부분
    [SerializeField] private Button[] StructureCreateButton;
    [SerializeField] private GameObject turretGridPrefab;
    public static int level;

    [Header("Upgrad UI")]
    [Space]
    [SerializeField] private Tilemap tilemap;
    public static Tilemap tileMap;
    private static GameObject grid;
    public TextFadeOut textfadeout;

    public static int build;
    public static int buildCost;
    public Vector3Int cell;

    public BuildingSO[] Lv1;
    public BuildingSO[] Lv2;
    public BuildingSO[] Lv3;
    public BuildingSO[] Lv4;
    public BuildingSO[] Lv5;
    public BuildingSO[] Lv6;
    public BuildingSO[] Lv7;

    [Header("Nexus")]
    public NexusSO data;

    private void Awake()
    {
        I = this;

        build = -1;
        grid = Instantiate(turretGridPrefab);
        grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        StructureManager.tileMap = tilemap;

        

        for (int i = 0; i < StructureCreateButton.Length; i++)
        {
            int index = i;
            StructureCreateButton[i].onClick.AddListener(() => ClickBtn(index ));
        }
    }

    private void Update()
    {
        if (build >= 0)
        {
            if (build == 0)
                buildCost = 1000;
            else if (build == 1)
                buildCost = 10;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
            TurretManager.cell = tilemap.WorldToCell(mousePosition);
            this.cell = TurretManager.cell;
            TileBase tile = tilemap.GetTile(cell);
            mousePosition.z = 10;

            if (tile != null && (tile.name == "Tileset" || System.Text.RegularExpressions.Regex.IsMatch(tile.name, @"^Tile[0-9]$")))
            {
                grid.transform.position = tilemap.GetCellCenterWorld(cell);
                structurePreview.transform.position = tilemap.GetCellCenterWorld(cell);
                if (!TurretManager.check.ContainsKey(cell))
                {
                    grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    structurePreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
                }
                else
                {
                    grid.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
                    structurePreview.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
                }
            }
            else
            {
                grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                structurePreview.transform.position = mousePosition;
                structurePreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (tile != null && (tile.name == "Tileset" || System.Text.RegularExpressions.Regex.IsMatch(tile.name, @"^Tile[0-9]$")))
                {
                    if (!TurretManager.check.ContainsKey(cell))
                    {
                        if (GameManager.goldcount >= buildCost)
                        {
                            TurretManager.check[cell] = true;
                            Destroy(structurePreview);
                            //GameObject guides = Instantiate(turretGridPrefab);
                            //guides.transform.position = tilemap.GetCellCenterWorld(cell);
                            StartCoroutine(Buildstructure( cell, build));
                            grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                            GameManager.goldcount -= buildCost;
                            build = -1;

                        }
                        else
                        {
                            textfadeout.DisplayErrorMessage("돈없어서 못지어요!");
                        }
                    }
                    else
                    {
                        textfadeout.DisplayErrorMessage("건물을 중복된 위치에 지을 수 없어요!");
                    }
                }
                else
                {
                    //textfadeout.DisplayErrorMessage("건설이 불가능한 타일이에요, 오른쪽 마우스 버튼을 눌러 취소해주세요");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                Destroy(structurePreview);
                build = -1;
            }
        }
    }

    IEnumerator Buildstructure( Vector3Int cell, int build)
    {
        TurretInfo That = new TurretInfo();
        That.Structure = Instantiate(StructurePreview[build], tilemap.GetCellCenterWorld(cell), Quaternion.identity);
        That.StructureBuild = Lv1[build].BuildSpeed;
        That.HP = Lv1[build].BuildHP;

        float elapsedTime = 0f;

        while (elapsedTime < That.StructureBuild)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / That.StructureBuild);
            That.Structure.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(That.Structure);


        That.Structure = Instantiate(Structure[build], tilemap.GetCellCenterWorld(cell), Quaternion.identity);
        That.Structure.AddComponent<StructureClick>();
        That.Number = build;
        That.HP = Lv1[build].BuildHP;
        That.CurrentHP = That.HP;
        That.Cell = cell;
        TurretInfo.Load[That.Structure] = That;
        grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        That.Structure.AddComponent<HpSlider>();
        //HpSlider.I.fill.color = Color.yellow;
        GameManager.goldcount -= buildCost;


    }
    public void ClickBtn(int l )
    {
        build = l;

        if (build >= 0)
        {
            Destroy(structurePreview);
            structurePreview = Instantiate(StructurePreview[build]);
            //TurretManager.I.ClickBtn(build);
        }
    }
}
