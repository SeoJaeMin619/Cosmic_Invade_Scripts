using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StringGameObject : SerializableDictionary<string, GameObject> { }

public class WaveTest : MonoBehaviour
{
    public Transform Nexus;
    public LayerMask TargetLayer;

    public StringGameObject NameToPrefab;
    public TextAsset Data;
    private AllWaves AllWaves;

    [SerializeField] private List<UnitInfo> UnitInfos;
    [SerializeField] private List<List<UnitInfo>> Wave;

    //UI
    [SerializeField] private Button UpButton;
    [SerializeField] private Button DownButton;
    [SerializeField] private Button SpawnButton;
    [SerializeField] private Button ResetButton;



    public TextMeshProUGUI WaveCursor;
    private int Cursor;

    public List<GameObject> CurrentUnits;

    public WaveTest wavetest;
    public static WaveTest wavetestst;

    private WaitForSeconds WaitFor1S = new WaitForSeconds(1f);
    void Start()
    {
        WaveTest.wavetestst = wavetest;
        AllWaves = JsonUtility.FromJson<AllWaves>(Data.text);
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

        Cursor = 0;
        UpButton.onClick.AddListener(() => Cursor++);
        UpButton.onClick.AddListener(() => WaveCursor.text = Cursor.ToString());
        DownButton.onClick.AddListener(() => Cursor--);
        DownButton.onClick.AddListener(() => WaveCursor.text = Cursor.ToString());
        SpawnButton.onClick.AddListener(() => SpawnFromWave(Wave[Cursor]));
        ResetButton.onClick.AddListener(() => ResetUnit());

    }

    public void SpawnWave(int index)
    {
        if (index < Wave.Count)
        {
            foreach (var data in Wave[index])
            {
                SpawnFromUnitInfo(data);
            }
        }
    }


    public void SpawnFromWave(List<UnitInfo> unitInfos)
    {
        foreach (var data in unitInfos)
        {
            StartCoroutine(CoAttack(data));
        }
    }
   
    public void SpawnFromUnitInfo(UnitInfo unitInfo)
    {
        if (NameToPrefab.TryGetValue(unitInfo.UnitName, out GameObject prefab))
        {
            for (int i = 0; i < unitInfo.NumberOfUnit; i++)
            {
                float spawnX = unitInfo.SpawnPositionX;
                float spawnY = unitInfo.SpawnPositionY;

                int RandX = UnityEngine.Random.Range(-5, 6);
                int RandY = UnityEngine.Random.Range(-5, 6);

                spawnX += RandX;
                spawnY += RandY;
                GameObject obj = Instantiate(prefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity);
                obj.AddComponent<HpSlider>();
                ISetTarget setTarget = obj.GetComponent<ISetTarget>();
                setTarget.SetTarget(Nexus);
                setTarget.SetNexus(Nexus);
                setTarget.SetTargetLayer(TargetLayer);

                CurrentUnits.Add(obj);
            }
        }
        else
        {
            Debug.LogError("Prefab not found for unit: " + unitInfo.UnitName);
        }
    }

    public void ResetUnit()
    {
        foreach (var data in CurrentUnits)
        {
            Unit unit = data.GetComponent<Unit>();
            unit.Damaged(1000);
        }

        CurrentUnits.Clear();
    }

    private IEnumerator CoAttack(UnitInfo unitInfo)
    {
        if (NameToPrefab.TryGetValue(unitInfo.UnitName, out GameObject prefab))
        {
            for (int i = 0; i < unitInfo.NumberOfUnit; i++)
            {
                float spawnX = unitInfo.SpawnPositionX;
                float spawnY = unitInfo.SpawnPositionY;

                int RandX = UnityEngine.Random.Range(-5, 6);
                int RandY = UnityEngine.Random.Range(-5, 6);

                spawnX += RandX;
                spawnY += RandY;
                GameObject obj = Instantiate(prefab, new Vector3(spawnX, spawnY, 0f), Quaternion.identity);
                obj.AddComponent<HpSlider>();
                ISetTarget setTarget = obj.GetComponent<ISetTarget>();
                setTarget.SetTarget(Nexus);
                setTarget.SetNexus(Nexus);
                setTarget.SetTargetLayer(TargetLayer);

                CurrentUnits.Add(obj);

                yield return WaitFor1S;
            }
        }
        else
        {
            Debug.LogError("Prefab not found for unit: " + unitInfo.UnitName);
        }
    }
}
[Serializable]
public struct UnitInfo
{
    public string UnitName;
    public int NumberOfUnit;
    public float SpawnPositionX;
    public float SpawnPositionY;

    public UnitInfo(string unitName, int numberOfUnit, float spawnPositionX, float spawnPositionY)
    {
        this.UnitName = unitName;
        this.NumberOfUnit = numberOfUnit;
        this.SpawnPositionX = spawnPositionX;
        this.SpawnPositionY = spawnPositionY;
    }
}


[System.Serializable]
public class AllWaves
{
    public WaveData[] WaveData;
}
[System.Serializable]
public class WaveData
{
    public int Wave;
    public string UnitName;
    public int NumberOfUnit;
    public float SpawnPositionX;
    public float SpawnPositionY;
}
