using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BigDevil : Unit , ISetTarget
{
    [SerializeField] private GameObject BigDevilPrefab;
    [SerializeField] private UnitBaseSO Data;
    [SerializeField] private Transform UnitTransform;
    [SerializeField] private Transform Nexus;
    [SerializeField] private Transform Target;
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private GameObject BombPrefab;

    private WaitForSeconds WaitFor2S = new WaitForSeconds(2);
    private Coroutine CoUnit;
    public int NumberOfRespawn = 3;
    public int NumberOfUnit =1;

    //For Remove From List in RoundManager
    private List<GameObject> SpawnedList;

    private void Awake()
    {
        MaxHP = Data.MaxHp;
        CurrnetHP = MaxHP;
        if (UnitTransform == null) UnitTransform = gameObject.GetComponent<Transform>();

    }

    private void Start()
    {
        UnitTransform.localScale = new Vector3(NumberOfRespawn, NumberOfRespawn, 1);

      CoUnit= StartCoroutine(UnitAI());
     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            GameObject obj= Instantiate (BombPrefab, UnitTransform.position, Quaternion.identity);
            if(obj.TryGetComponent<Weapon>(out Weapon weapon))
            {
                weapon.Damage = Data.Damage;
                weapon.TargetLayer= TargetLayer;
            }
            obj.transform.localScale = new Vector3(NumberOfRespawn, NumberOfRespawn, 1);
            Damaged(100);
        }
    }

    private IEnumerator UnitAI()
    {
        while (true)
        {
            if (Nexus != null)
            {
                Vector3 dir = (Nexus.position - UnitTransform.position).normalized;
                rb2D.velocity = dir * Data.MoveSpeed;
            }
            yield return WaitFor2S;
        }
    }

    public override void Damaged(int dmg)
    {
        base.Damaged(dmg);
        if (CurrnetHP <= 0)
        {
            if (NumberOfRespawn > 1) 
            {
                SpawnedList.Remove(this.gameObject);
                SpawnClone();
            }
            else
            {
                GameManager.goldcount += Data.gold;
            }
            StopCoroutine(CoUnit);
            
            Destroy(gameObject);
        }
    }

    private void SpawnClone()
    {
        float sign = -1;

        for (int i = 0; i <= NumberOfUnit; i++)
        {
            Vector3 SpawnPosition = UnitTransform.position;
            SpawnPosition += Vector3.up * sign;
            if(i %2 == 0)  sign *= -1;
            SpawnPosition += Vector3.right * sign;
         

            GameObject obj = Instantiate(BigDevilPrefab, SpawnPosition, Quaternion.identity);
            BigDevil bigDevil = obj.GetComponent<BigDevil>();
            bigDevil.SetRespawn(NumberOfRespawn);
            bigDevil.SetNexus(Nexus);
            bigDevil.SetTargetLayer(TargetLayer);

            SpawnedList.Add(obj);
        }
    }

    public void SetRespawn(int CurrentCount)
    {
        NumberOfRespawn = CurrentCount -1;
        NumberOfUnit++;
    }

    public void SetTarget(Transform target)
    {
       this.Target = target;
    }

    public void SetTargetLayer(LayerMask TargetLayer)
    {
        this.TargetLayer= TargetLayer;
    }

    public void SetNexus(Transform Nexus)
    {
        this.Nexus = Nexus;
    }

    public void SetSpawendList(List<GameObject> list)
    {
        SpawnedList = list;
    }
}
