using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class RoundManager : MonoBehaviour
{
    //Data From Json
    [SerializeField] private List<UnitInfo> UnitInfos;
    [SerializeField] private List<List<UnitInfo>> Wave;
    [SerializeField] private TextAsset Data;

    public StringGameObject NameToPrefab;
    public CryptoManager cryptoManagerInstance;
    private AllWaves AllWaves;

    //Data for Unit
    public Transform Nexus;
    public LayerMask TargetLayer;

    //Round Data
    [SerializeField] int totalRounds = 10;
    [SerializeField] float roundDurationSeconds = 120f;
    [SerializeField] float breakDurationSeconds = 10f;
    [SerializeField] float bossWarningTime = 4f;

    //UI
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TextMeshProUGUI roundText;
    [SerializeField] private GameObject bossWarningPanel;
    [SerializeField] private TextMeshProUGUI roundCursor;
    [SerializeField] private TextFadeOut textfadeout;

    //Locked Turret Open

    [SerializeField] private List<Button> lockedTurret;
    [SerializeField] private List<TextMeshProUGUI> lockedText;
    [SerializeField] private List<GameObject> lockedImage;
    [SerializeField] private List<GameObject> releaseText;

    //Button
    [SerializeField] private Button camera1Btn;
    [SerializeField] private Button roundStartBtn;
    [SerializeField] private Button RoundUp;
    [SerializeField] private Button RoundDown;
    private float waitTime;

    public int currentRound = 0;

    //Wait For Second
    private WaitForSeconds WaitFor2S = new WaitForSeconds(2f);
    private WaitForSeconds WaitFor5S = new WaitForSeconds(5f);

    //Unit List
    public List<GameObject> SpawnedUnitsList;
    private bool Crypto = true;

    //Round Cheat
    public Action OnClickRound;

    //
    [SerializeField] private Transform[] SpawnPositions;
    private void Awake()
    {
        //Read Data From Json
        AllWaves = JsonUtility.FromJson<AllWaves>(Data.text);

        //Add Data To List
        Wave = new List<List<UnitInfo>>();
        Wave.Clear();
        int CurrentWave = 0;
        foreach (var data in AllWaves.WaveData)
        {
            if (data.Wave != CurrentWave)
            {
                List<UnitInfo> copy = new List<UnitInfo>(UnitInfos);
                Wave.Add(copy);
                UnitInfos.Clear();
                CurrentWave = data.Wave;
            }
            UnitInfo unitInfo = new UnitInfo(data.UnitName, data.NumberOfUnit, data.SpawnPositionX, data.SpawnPositionY);
            UnitInfos.Add(unitInfo);
        }

        //Initalize List
        SpawnedUnitsList = new List<GameObject>();

        RoundDown.onClick.AddListener(RoundDOWN);
        RoundUp.onClick.AddListener(RoundUP);
    }

    private void Start()
    {
       
        roundStartBtn.onClick.AddListener(StartRound);
        StartCoroutine(RunRounds());
        for (int i = 0; i < lockedTurret.Count; i++)
        {
            lockedTurret[i].interactable = false;            
        }

        //Cheat
        OnClickRound += () => roundCursor.text = currentRound.ToString();
    }  

    IEnumerator RunRounds()
    {
        while (currentRound < totalRounds)
        {

            roundText.text = FormatTime(60f);
            timeSlider.value = 1f;
            waitTime = 60;


            while (waitTime > 0f)
            {
                waitTime -= Time.deltaTime;
                timeSlider.value = waitTime / 10f;


                if (waitTime <= 10f && waitTime > 9f)
                {
                    roundText.text = "웨이브 10초 전!";
                    textfadeout.DisplayErrorMessage("10초 뒤에 자동으로 디펜스 화면으로 전환됩니다.");
                }
                else if (waitTime < 60f && waitTime > 0f)
                {
                    roundText.text = FormatTime(waitTime);
                }

                yield return null;
            }

            OpenButton(currentRound);


            // 라운드 시작
            currentRound++;



            roundText.text = "라운드 " + currentRound;

            // 게이지 초기화
            timeSlider.value = 1f;

            // 경과 시간
            float elapsedTime = 0f;
            //Spawn Wave
            SpawnFromWave(Wave[currentRound]);
            while (elapsedTime < roundDurationSeconds)
            {
                camera1Btn.interactable = false;
                CameraController.I.DefenceCamera();
                // 라운드 진행 중 게이지 갱신
                elapsedTime += Time.deltaTime;
                timeSlider.value = 1 - elapsedTime / roundDurationSeconds;

                if (cryptoManagerInstance != null && Crypto)
                {
                    for (int i = 0; i < cryptoManagerInstance.cryptocurrencies.Length; i++)
                    {
                        cryptoManagerInstance.UpdateCryptoPrices(cryptoManagerInstance.cryptocurrencies[i]);
                        Crypto = false;
                    }
            
                }
                

                // Boss 몬스터 등장 경고 표시
                // if (roundDurationSeconds - elapsedTime <= bossWarningTime && !bossWarningPanel.activeSelf)
                //  {
                //   bossWarningPanel.SetActive(true);
                //  }

                yield return null; // 한 프레임 기다림
            }
            // 휴식 시간
            camera1Btn.interactable = true;
            Crypto = true;
            if (cryptoManagerInstance != null && Crypto)
            {
                for (int i = 0; i < cryptoManagerInstance.cryptocurrencies.Length; i++)
                {
                    cryptoManagerInstance.UpdateCryptoPrices(cryptoManagerInstance.cryptocurrencies[i]);
                }

            }
            roundText.text = "대기 시간";
            bossWarningPanel.SetActive(false);
            yield return new WaitForSeconds(breakDurationSeconds);
        }
        // 모든 라운드가 종료되면 종료 메시지 출력
        roundText.text = "All Rounds Completed";
        timeSlider.gameObject.SetActive(false); // 라운드 종료 후 게이지 숨김
    }


    public void SpawnFromWave(List<UnitInfo> unitInfos)
    {
        foreach (var data in unitInfos)
        {
            StartCoroutine(CoSpawn(data));
        }
    }
    private IEnumerator CoSpawn(UnitInfo unitInfo)
    {
        if (NameToPrefab.TryGetValue(unitInfo.UnitName, out GameObject prefab))
        {
            if (unitInfo.UnitName.StartsWith("Boss"))
            {
                bossWarningPanel.SetActive(true);
                yield return WaitFor5S;
                bossWarningPanel.SetActive(false);
            }

            for (int i = 0; i < unitInfo.NumberOfUnit; i++)
            {
                Vector3 Position = SpawnPositions[CalPositionIndex(unitInfo.SpawnPositionX)].position;

                int RandX = UnityEngine.Random.Range(-5, 6);
                int RandY = UnityEngine.Random.Range(-5, 6);

                Position.x += RandX;
                Position.y += RandY;
                GameObject obj = Instantiate(prefab, Position, Quaternion.identity);
                SpawnedUnitsList.Add(obj);
                obj.AddComponent<HpSlider>();
                ISetTarget setTarget = obj.GetComponent<ISetTarget>();
                setTarget.SetTarget(Nexus);
                setTarget.SetNexus(Nexus);
                setTarget.SetTargetLayer(TargetLayer);
                setTarget.SetSpawendList(SpawnedUnitsList);

                yield return WaitFor2S;
            }
        }
        else
        {
            Debug.LogError("Prefab not found for unit: " + unitInfo.UnitName);
        }


    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void DeathUnits()
    {
        foreach (GameObject unit in SpawnedUnitsList)
        {
            Destroy(unit);

            if (unit.TryGetComponent<HpSlider>(out var hpSlider))
            {
                Destroy(hpSlider.hpslider);
                Destroy(hpSlider);
            }
        }

        SpawnedUnitsList.Clear();

    }

    public void StopUnit()
    {
        StartCoroutine(FreezeUnitsForDuration(10f));
    }

    private IEnumerator FreezeUnitsForDuration(float duration)
    {
        foreach (GameObject unit in SpawnedUnitsList)
        {
            
            Rigidbody2D rb = unit.GetComponent<Rigidbody2D>();


            if (rb == null)
            {
                rb = unit.AddComponent<Rigidbody2D>();
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
            else
            {
          
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }

       
        yield return new WaitForSeconds(duration);

        
        foreach (GameObject unit in SpawnedUnitsList)
        {
            Rigidbody2D rb = unit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.None;
            }
        }
    }

    private void OpenButton(int index)
    {
        lockedTurret[index].interactable = true;        
        lockedText[index].text = "";
        releaseText[index].SetActive(true);
        lockedImage[index].SetActive(false);
        
    }

    private void StartRound()
    {
        waitTime = 0f;
    }

    private int CalPositionIndex(float num)
    {
        if(num >= SpawnPositions.Length)
        {
            Debug.Log("CalPosition Index out of range");
            return 0;
        }

        if(num < 1) return 0;
        if(num < 2) return 1;
        if(num < 3) return 2;
        if(num < 4) return 3;
        if(num < 5) return 4;
        if(num < 6) return 5;

        return 0;
    }

    //Cheat

    private void RoundUP() {currentRound++; OnClickRound?.Invoke(); }

    private void RoundDOWN() { currentRound--; OnClickRound?.Invoke(); }
}
