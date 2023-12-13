using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TurretInfo
{
    public static Dictionary<GameObject, TurretInfo> Load = new Dictionary<GameObject, TurretInfo>();

    public static TurretInfo Temp;
    public static TurretInfo Info;

    public GameObject HpSlider;
    public GameObject Turret;
    public Vector3Int Cell;
    public int Number;
    public int Level;
    public int Atk;
    public int HP;
    public int CurrentHP;

    public float TurretFireSpeed;
    public float TurretRange;
    public int Cost;
    public String Name;
    public Text UpgradCostTxt;
    public int ATKBuff;

    public float TurretProbability;
    public float TurretBuild;

    // 건물

    public GameObject Structure;
    public float StructureBuild;

    // 넥서스

    public int NexusMP;


    // public string name; // 타워 이름
    // public string description; // 타워 설명
    // public int level; // 타워 레벨
    // // 다른 정보들도 추가할 수 있습니다.

    void Awake()
    {
        Temp = this;
    }
}
public class TurretClick : MonoBehaviour
{
    private void OnMouseDown()
    {

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (TurretManager.build < 0)
        {

            TurretInfo This = TurretInfo.Load[gameObject];

            Vector3 cellCenterWorld = TurretManager.tileMap.GetCellCenterWorld(This.Cell);
            RectTransform popupRectTransform = TurretManager.popupUIst.GetComponent<RectTransform>();
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, cellCenterWorld);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(popupRectTransform.parent as RectTransform, screenPos, null, out Vector2 localPos);
            popupRectTransform.anchoredPosition = localPos;
            //Turretoption();
            StartCoroutine(DisablePopup());

            TurretInfo.Temp = This;
        }

    }
    private void Update()
    {
        if (TurretManager.popupUIst.activeSelf == true)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                TurretManager.popupUIst.SetActive(false);
            }

            if (Input.GetMouseButtonDown(1))
            {
                TurretManager.popupUIst.SetActive(false);
            }

        }

        if (TurretManager.popupStatest.activeSelf == true)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                TurretManager.popupStatest.SetActive(false);
            }
            if (Input.GetMouseButtonDown(1))
            {
                TurretManager.popupStatest.SetActive(false);
            }
        }
    }
    void PopUI()
    {
        TurretManager.popupUIst.SetActive(false);
    }

    IEnumerator DisablePopup()
    {
        TurretManager.popupUIst.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        TurretManager.popupUIst.SetActive(true);
    }
    public static void TurretState()
    {
        TurretInfo This = TurretInfo.Temp;

        //TurretManager.upgradCostTxtst.text = "타워 업그레이드 하는데 소모하는 비용 : " + This.UpgradCost;
        string Text = (This != TurretInfo.Info) ? "[터렛 강화 가격]" : "[터렛 구매 가격]";

        if (This.Number != 5)
        {
            TurretManager.nameTxtst.text = This.Name;
            TurretManager.stateTxtst.text = " [터렛 HP]  ::  " + This.CurrentHP + " / " + This.HP +
                                           "\n [터렛 공격]  ::  " + This.Atk +
                                           "\n [터렛 범위]  ::  " + This.TurretRange +
                                           "\n [터렛 속도]  ::  " + This.TurretFireSpeed +
                                           "\n\n " + Text + "  ::  " + This.Cost +
                                           "\n [터렛 판매 가격]  ::  " + (int)(This.Cost * 0.8f) +
                                           "\n\n [터렛 강화 성공 확률]  ::  " + This.TurretProbability * 100f + "%";

        }
        else
        {
            TurretManager.nameTxtst.text = This.Name;
            TurretManager.stateTxtst.text = " [터렛 HP]  ::  " + This.CurrentHP + " / " + This.HP +
                                           "\n [터렛 자원 획득]  ::  " + This.Atk +
                                           "\n\n " + Text + "  ::  " + This.Cost +
                                           "\n [터렛 판매 가격]  ::  " + (int)(This.Cost * 0.8f) +
                                           "\n\n [터렛 강화 성공 확률]  ::  " + This.TurretProbability * 100f + "%";
        }

    }
    public void TurretSell()
    {
        TurretInfo This = TurretInfo.Temp;
        if (This.Turret != null)
        {
            Destroy(This.Turret);
            Destroy(This.Turret.gameObject.GetComponent<HpSlider>().hpslider);
            TurretManager.check.Remove(This.Cell);
            GameManager.goldcount += (int)(This.Cost * 0.8f);
            TurretManager.popupStatest.SetActive(false);
        }
        else
            TurretManager.I.textfadeout.DisplayErrorMessage("타워가 존재하지 않습니다.");
    }
    public void Turretinfo()
    {
        TurretManager.popupStatest.SetActive(true);
    }
    public void Turretoption()
    {
        TurretManager.popupUIst.SetActive(false);
        TurretManager.popupUIst.SetActive(true);
    }

}

public class TurretUpgrades
{

    public void TurretUpgrad()
    {
        TurretInfo This = TurretInfo.Temp;

        //TurretManager.turretUpgradButtonst[0].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        if (This.Level < 6)
        {
            if (GameManager.goldcount >= This.Cost)
            {
                if (This.Turret != null)
                {

                    if (UnityEngine.Random.Range(0.0f, 1.0f) <= This.TurretProbability)
                    {
                        GameManager.goldcount -= This.Cost;
                        TurretClick.Destroy(This.Turret);
                        TurretManager.I.turretList.Remove(This.Turret);
                        TurretClick.Destroy(This.Turret.gameObject.GetComponent<HpSlider>().hpslider);
                        This.Level++;


                        GameObject LvEPT = TurretClick.Instantiate(TurretManager.I.lvupEpt, TurretManager.tileMap.GetCellCenterWorld(This.Cell), Quaternion.identity);
                        TurretClick.Destroy(LvEPT, 0.85f);


                        switch (This.Level)
                        {
                            case 1:

                                This.Turret = TurretClick.Instantiate(TurretManager.turretLV2[(int)This.Number], TurretManager.tileMap.GetCellCenterWorld(This.Cell), Quaternion.identity);
                                This.HP = TurretManager.I.Lv2[This.Number].MaxHP();
                                This.Atk = TurretManager.I.Lv2[This.Number].AttackDamage();
                                This.TurretRange = TurretManager.I.Lv2[This.Number].TurretRange();
                                This.TurretFireSpeed = TurretManager.I.Lv2[This.Number].TurretFireSpeed();
                                This.TurretProbability = TurretManager.I.Lv2[This.Number].TurretUpgradProbability();
                                This.Cost = TurretManager.I.Lv2[This.Number].TurretCost();
                                This.Name = TurretManager.I.Lv2[This.Number].TurretName();
                                This.Turret.AddComponent<TurretClick>();
                                //TurretManager.popupUIst.SetActive(false);
                                break;
                            case 2:
                                This.Turret = TurretClick.Instantiate(TurretManager.turretLV3[(int)This.Number], TurretManager.tileMap.GetCellCenterWorld(This.Cell), Quaternion.identity);
                                This.HP = TurretManager.I.Lv3[This.Number].MaxHP();
                                This.Atk = TurretManager.I.Lv3[This.Number].AttackDamage();
                                This.TurretRange = TurretManager.I.Lv3[This.Number].TurretRange();
                                This.TurretFireSpeed = TurretManager.I.Lv3[This.Number].TurretFireSpeed();
                                This.TurretProbability = TurretManager.I.Lv3[This.Number].TurretUpgradProbability();
                                This.Cost = TurretManager.I.Lv3[This.Number].TurretCost();
                                This.Name = TurretManager.I.Lv3[This.Number].TurretName();
                                This.Turret.AddComponent<TurretClick>();
                                //TurretManager.popupUIst.SetActive(false);
                                break;
                            case 3:
                                This.Turret = TurretClick.Instantiate(TurretManager.turretLV4[(int)This.Number], TurretManager.tileMap.GetCellCenterWorld(This.Cell), Quaternion.identity);
                                This.HP = TurretManager.I.Lv4[This.Number].MaxHP();
                                This.Atk = TurretManager.I.Lv4[This.Number].AttackDamage();
                                This.TurretRange = TurretManager.I.Lv4[This.Number].TurretRange();
                                This.TurretFireSpeed = TurretManager.I.Lv4[This.Number].TurretFireSpeed();
                                This.TurretProbability = TurretManager.I.Lv4[This.Number].TurretUpgradProbability();
                                This.Cost = TurretManager.I.Lv4[This.Number].TurretCost();
                                This.Name = TurretManager.I.Lv4[This.Number].TurretName();
                                This.Turret.AddComponent<TurretClick>();
                                //TurretManager.popupUIst.SetActive(false);
                                break;
                            case 4:
                                This.Turret = TurretClick.Instantiate(TurretManager.turretLV5[(int)This.Number], TurretManager.tileMap.GetCellCenterWorld(This.Cell), Quaternion.identity);
                                This.HP = TurretManager.I.Lv5[This.Number].MaxHP();
                                This.Atk = TurretManager.I.Lv5[This.Number].AttackDamage();
                                This.TurretRange = TurretManager.I.Lv5[This.Number].TurretRange();
                                This.TurretFireSpeed = TurretManager.I.Lv5[This.Number].TurretFireSpeed();
                                This.TurretProbability = TurretManager.I.Lv5[This.Number].TurretUpgradProbability();
                                This.Cost = TurretManager.I.Lv5[This.Number].TurretCost();
                                This.Name = TurretManager.I.Lv5[This.Number].TurretName();
                                This.Turret.AddComponent<TurretClick>();
                                //TurretManager.popupUIst.SetActive(false);
                                break;
                            case 5:
                                This.Turret = TurretClick.Instantiate(TurretManager.turretLV6[(int)This.Number], TurretManager.tileMap.GetCellCenterWorld(This.Cell), Quaternion.identity);
                                This.HP = TurretManager.I.Lv6[This.Number].MaxHP();
                                This.Atk = TurretManager.I.Lv6[This.Number].AttackDamage();
                                This.TurretRange = TurretManager.I.Lv6[This.Number].TurretRange();
                                This.TurretFireSpeed = TurretManager.I.Lv6[This.Number].TurretFireSpeed();
                                This.TurretProbability = TurretManager.I.Lv6[This.Number].TurretUpgradProbability();
                                This.Cost = TurretManager.I.Lv6[This.Number].TurretCost();
                                This.Name = TurretManager.I.Lv6[This.Number].TurretName();
                                This.Turret.AddComponent<TurretClick>();
                                //TurretManager.popupUIst.SetActive(false);
                                break;
                            case 6:
                                This.Turret = TurretClick.Instantiate(TurretManager.turretLV7[(int)This.Number], TurretManager.tileMap.GetCellCenterWorld(This.Cell), Quaternion.identity);
                                This.HP = TurretManager.I.Lv7[This.Number].MaxHP();
                                This.Atk = TurretManager.I.Lv7[This.Number].AttackDamage();
                                This.TurretRange = TurretManager.I.Lv7[This.Number].TurretRange();
                                This.TurretFireSpeed = TurretManager.I.Lv7[This.Number].TurretFireSpeed();
                                This.TurretProbability = TurretManager.I.Lv7[This.Number].TurretUpgradProbability();
                                This.Cost = TurretManager.I.Lv7[This.Number].TurretCost();
                                This.Name = TurretManager.I.Lv7[This.Number].TurretName();
                                This.Turret.AddComponent<TurretClick>();
                                //TurretManager.popupUIst.SetActive(false);
                                break;
                        }

                        This.Turret.AddComponent<HpSlider>();
                        TurretInfo.Load[This.Turret] = This;
                        This.CurrentHP = This.HP;
                    }
                    else
                    {
                        TurretManager.I.textfadeout.DisplayErrorMessage("강화 실패!");
                        GameManager.goldcount -= This.Cost;
                        UpgradeFailedAction(This);
                    }

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

    private void UpgradeFailedAction(TurretInfo turretInfo)
    {
        TurretInfo This = TurretInfo.Temp;


        if (UnityEngine.Random.Range(0.0f, 1.0f) <= 0.1f)
        {
            TurretClick.Destroy(This.Turret);
            TurretManager.I.turretList.Remove(This.Turret);
            TurretClick.Destroy(This.Turret.gameObject.GetComponent<HpSlider>().hpslider);
            TurretManager.I.textfadeout.DisplayErrorMessage("강화 실패 타워 파괴!");
            TurretManager.check.Remove(This.Cell);
        }
    }
}

public class TurretManager : MonoBehaviour
{
    public static TurretManager I;

    [Header("Turret Set")]
    [Space]

    [SerializeField] private GameObject[] turretLv1;
    [SerializeField] private GameObject[] turretLv2;
    public static GameObject[] turretLV2;
    [SerializeField] private GameObject[] turretLv3;
    public static GameObject[] turretLV3;
    [SerializeField] private GameObject[] turretLv4;
    public static GameObject[] turretLV4;
    [SerializeField] private GameObject[] turretLv5;
    public static GameObject[] turretLV5;
    [SerializeField] private GameObject[] turretLv6;
    public static GameObject[] turretLV6;
    [SerializeField] private GameObject[] turretLv7;
    public static GameObject[] turretLV7;
    [SerializeField] private GameObject[] turretPreviewPrefab;
    public static GameObject turretPreview;
    [SerializeField] private Button[] turretCreateButton;
    [SerializeField] private GameObject turretGridPrefab;
    public static int level;

    [Header("Upgrad UI")]
    [Space]

    [SerializeField] private Button[] turretButtonpv;
    public static Button[] turretButtonst;

    [SerializeField] private GameObject popupUIpv;
    public static GameObject popupUIst;

    [SerializeField] private GameObject popupStatepv;
    public static GameObject popupStatest;
    [SerializeField] private Text upgradCostTxtpv;
    public static Text upgradCostTxtst;

    [SerializeField] private Text stateTxtpv;
    public static Text stateTxtst;
    [SerializeField] private Text nameTxtpv;
    public static Text nameTxtst;

    [Header("Extra UI")]
    [Space]

    public GameObject lvupEpt;
    [SerializeField] private Tilemap tilemap;
    public static Tilemap tileMap;
    private static GameObject grid;

    public TextFadeOut textfadeout;
    public static Dictionary<Vector3Int, bool> check;
    public static int build;
    public static int buildCost;
    public static int upgradCost;
    public static Vector3Int cell;

    //public TurretSO nexusstate;
    public TurretSO[] Lv1;
    public TurretSO[] Lv2;
    public TurretSO[] Lv3;
    public TurretSO[] Lv4;
    public TurretSO[] Lv5;
    public TurretSO[] Lv6;
    public TurretSO[] Lv7;

    public List<GameObject> turretList = new List<GameObject>();

    private void Awake()
    {
        I = this;
        build = -1;
        check = new Dictionary<Vector3Int, bool>();
        grid = Instantiate(turretGridPrefab);
        grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        TurretManager.turretButtonst = turretButtonpv;
        TurretManager.upgradCostTxtst = upgradCostTxtpv;
        TurretManager.stateTxtst = stateTxtpv;
        TurretManager.popupUIst = popupUIpv;
        TurretManager.popupStatest = popupStatepv;
        TurretManager.nameTxtst = nameTxtpv;
        TurretManager.turretLV2 = turretLv2;
        TurretManager.turretLV3 = turretLv3;
        TurretManager.turretLV4 = turretLv4;
        TurretManager.turretLV5 = turretLv5;
        TurretManager.turretLV6 = turretLv6;
        TurretManager.turretLV7 = turretLv7;
        TurretManager.tileMap = tilemap;
        TurretInfo.Info = new TurretInfo();

        for (int i = 0; i < turretCreateButton.Length; i++)
        {
            int index = i;
            turretCreateButton[i].onClick.AddListener(() => ClickBtn(index));
        }

        TurretUpgrades turretA = new TurretUpgrades();
        turretButtonpv[0].onClick.AddListener(() => turretA.TurretUpgrad());
        TurretClick turretB = new TurretClick();
        turretButtonpv[1].onClick.AddListener(() => turretB.TurretSell());
        turretButtonpv[2].onClick.AddListener(() => turretB.Turretinfo());

        for (int i = 0; i < turretCreateButton.Length; i++)
        {
            int buttonIndex = i;


            turretCreateButton[i].onClick.AddListener(() => ShowPopup(buttonIndex));

            EventTrigger trigger = turretCreateButton[i].gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerExit = new EventTrigger.Entry();
            pointerExit.eventID = EventTriggerType.PointerExit;
            pointerExit.callback.AddListener((data) => { HidePopup(); });

            trigger.triggers.Add(pointerExit);
        }
    }
    void ShowPopup(int buttonIndex)
    {
        TurretInfo.Info.Name = Lv1[buttonIndex].TurretName();
        TurretInfo.Info.Atk = Lv1[buttonIndex].AttackDamage();
        TurretInfo.Info.HP = Lv1[buttonIndex].MaxHP();
        TurretInfo.Info.CurrentHP = Lv1[buttonIndex].MaxHP();
        TurretInfo.Info.TurretRange = Lv1[buttonIndex].TurretRange();
        TurretInfo.Info.TurretFireSpeed = Lv1[buttonIndex].TurretFireSpeed();
        TurretInfo.Info.Cost = Lv1[buttonIndex].TurretCost();
        TurretInfo.Info.TurretProbability = Lv1[buttonIndex].TurretUpgradProbability();
        TurretInfo.Temp = TurretInfo.Info;
        popupStatest.SetActive(true);
    }

    void HidePopup()
    {
        popupStatest.SetActive(false); ;
    }

    [Obsolete]
    private void Update()
    {

        if (build >= 0)
        {
            buildCost = Lv1[build].TurretCost();

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cell = tilemap.WorldToCell(mousePosition);
            TileBase tile = tilemap.GetTile(cell);
            mousePosition.z = 10;

            if (tile != null && System.Text.RegularExpressions.Regex.IsMatch(tile.name, "Tile0"))
            {
                grid.transform.position = tilemap.GetCellCenterWorld(cell);
                turretPreview.transform.position = tilemap.GetCellCenterWorld(cell);
                if (!check.ContainsKey(cell))
                {
                    grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                    turretPreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
                }
                else
                {
                    grid.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
                    turretPreview.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.3f);
                }
            }
            else
            {
                grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                turretPreview.transform.position = mousePosition;
                turretPreview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (tile != null && System.Text.RegularExpressions.Regex.IsMatch(tile.name, "Tile0"))
                {
                    if (!check.ContainsKey(cell))
                    {
                        if (GameManager.goldcount >= buildCost)
                        {
                            check[cell] = true;
                            GameObject guide = Instantiate(turretGridPrefab);
                            guide.transform.position = tilemap.GetCellCenterWorld(cell);
                            StartCoroutine(Buildturret(guide, cell, build));
                            grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                            GameManager.goldcount -= buildCost;
                            if (!Input.GetKey(KeyCode.LeftShift))
                            {
                                Destroy(turretPreview);
                                build = -1;
                            }

                        }
                        else
                        {
                            textfadeout.DisplayErrorMessage("돈없어서 못지어요!");
                        }
                    }
                    else
                    {
                        textfadeout.DisplayErrorMessage("타워를 중복된 위치에 지을 수 없어요!");
                    }
                }
                else
                {
                    //textfadeout.DisplayErrorMessage("지을 수 있는 타일이 아니에요, 오른쪽 마우스 버튼을 눌러 취소해주세요");
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                grid.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                Destroy(turretPreview);
                build = -1;
            }

        }

        if (popupStatest.active == true)
        {
            TurretClick.TurretState();
        }


    }
    IEnumerator Buildturret(GameObject guide, Vector3Int cell, int build)
    {
        TurretInfo This = new TurretInfo();
        This.Turret = Instantiate(turretPreviewPrefab[build], tilemap.GetCellCenterWorld(cell), Quaternion.identity);
        This.TurretBuild = Lv1[build].TurretBuildTime();
        This.HP = Lv1[build].MaxHP();
        // 건설시간 추후수정
        float elapsedTime = 0f;

        while (elapsedTime < This.TurretBuild)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / This.TurretBuild);
            This.Turret.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(This.Turret);
        Destroy(guide);


        This.Turret = Instantiate(turretLv1[build], tilemap.GetCellCenterWorld(cell), Quaternion.identity);
        This.Turret.AddComponent<TurretClick>();
        This.Number = build;
        This.Level = level;
        This.Cell = cell;
        This.HP = Lv1[build].MaxHP();
        This.CurrentHP = This.HP;
        This.Atk = Lv1[build].AttackDamage();
        This.TurretFireSpeed = Lv1[build].TurretFireSpeed();
        This.TurretRange = Lv1[build].TurretRange();
        This.TurretProbability = Lv1[build].TurretUpgradProbability();
        This.ATKBuff = Buff.damagebuff;
        This.Cost = Lv1[build].TurretCost();
        This.Name = Lv1[build].TurretName();
        TurretInfo.Load[This.Turret] = This;
        This.Turret.AddComponent<HpSlider>();
        ////HpSlider.I.fill.color = Color.blue;
        //turretList.Add(This.Turret);

    }
    public void ClickBtn(int l)
    {
        build = l;

        if (build >= 0)
        {
            Destroy(turretPreview);
            turretPreview = Instantiate(turretPreviewPrefab[build]);
 
        }

    }
}
