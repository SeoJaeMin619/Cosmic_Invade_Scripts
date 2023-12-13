using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigBrownMagician : Unit, ISetTarget
{
    [SerializeField] private UnitBaseRangedSO Data;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;

    private Vector3 NormalDirectionToTarget;
    private Vector3 DirectionToTarget;

    public Transform Nexus;
    public Transform Target;
    public LayerMask TargetLayer;


    //Stats
    public float MoveSpeed;
    public float AttackRange;
    public float AttackInterval;
    public int Damage;
    public int MaxHp;

    //Coroutine
    WaitForSeconds WaitForInterval;
    WaitForSeconds WaitFor30S = new WaitForSeconds(30f);
    WaitForSeconds WaitFor1S = new WaitForSeconds(1f);
    WaitForSeconds WaitFor2S = new WaitForSeconds(2f);
    private Coroutine CUnit;
    private Coroutine CAttack;
    private Coroutine CMove;

    //For Remove From List in RoundManager
    private List<GameObject> SpawnedList;

    //Attack Target
    private List<Transform> UnitsInRange;

    //DetectCollider
    [SerializeField] private Collider2D DetectCollider;

    //Tentacle
    [SerializeField] private GameObject TentaclePrefab;
    [SerializeField] private Tentacle[] Tentacles;

    private void Awake()
    {
        this.MoveSpeed = Data.MoveSpeed;
        this.AttackRange = Data.AttackRange;
        AttackRange *= AttackRange;
        this.AttackInterval = Data.AttackInterval;
        this.Damage = Data.Damage;
        this.MaxHp = Data.MaxHp;

        MaxHP = Data.MaxHp;
        CurrnetHP = MaxHP;

        WaitForInterval = new WaitForSeconds(AttackInterval);

        if (TentaclePrefab == null)
            TentaclePrefab = Data.ProjectilePrefab;

        Tentacles = new Tentacle[Data.NumberOfProjectile];

        UnitsInRange = new List<Transform>();
        UnitsInRange.Clear();

        for (int i = 0; i < Data.NumberOfProjectile; i++)
        {
            GameObject obj = Instantiate(TentaclePrefab);
            Tentacle tentacle = obj.GetComponent<Tentacle>();
            tentacle.Damage = Damage;
            tentacle.TargetLayer = TargetLayer;
            Tentacles[i] = tentacle;
        }
    }

    void Start()
    {
        CUnit = StartCoroutine(UnitAI());

        Target = Nexus;
        CalDirection();
        rb2D.velocity = NormalDirectionToTarget * MoveSpeed;
    }

    //Coroutine
    private IEnumerator UnitAI()
    {
        while (true)
        {
            if (Target == null)
            {
                if (Nexus == null)
                {
                    rb2D.velocity = Vector2.zero;
                    yield return WaitFor30S;
                }
            }

            //Tentacle Spawn
            if (UnitsInRange.Count > 0)
            {
                rb2D.velocity = Vector2.zero;

                if (UnitsInRange.Count != 0)
                {
                    int i = 0;
                    foreach (var te in Tentacles)
                    {
                        if (i < UnitsInRange.Count)
                        {
                            te.Play(transform.position, UnitsInRange[i].position);
                            i++;
                        }
                    }

                    yield return WaitFor2S;

                    foreach (var te in Tentacles)
                    {
                        te.Stop();
                    }

                    rb2D.velocity = NormalDirectionToTarget * MoveSpeed;


                    yield return WaitFor1S;
                }
            }

            yield return null;
        }

    }

    //Direction , Distance To Target
    #region
    public void CalDirection()
    {
        DirectionToTarget = Target.position - this.transform.position;
        NormalDirectionToTarget = DirectionToTarget.normalized;
    }

    public Vector3 GetDirctionToTarget()
    {
        return NormalDirectionToTarget.normalized;
    }
    public float DistanceToTarget()
    {
        return DirectionToTarget.sqrMagnitude;
    }
    #endregion

    //Collider
    #region
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            rb2D.velocity = Vector2.zero;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            CalDirection();
            rb2D.velocity = NormalDirectionToTarget * MoveSpeed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            UnitsInRange.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            UnitsInRange.Remove(collision.transform);
        }
    }

    #endregion

    public override void Damaged(int dmg)
    {
        base.Damaged(dmg);

        if (CurrnetHP <= 0)
        {
            if (Tentacles.Length > 0)
            {
                foreach (var te in Tentacles)
                {
                    te.Stop();
                }
            }

            GameManager.goldcount += Data.gold;

            SpawnedList.Remove(this.gameObject);
            StopCoroutine(CUnit);
            Destroy(gameObject.GetComponent<HpSlider>().hpslider);
            Destroy(this.gameObject);
        }
    }


    //Interface
    #region
    public void SetTarget(Transform target)
    {
        this.Target = target;
    }

    public void SetTargetLayer(LayerMask TargetLayer)
    {
        this.TargetLayer = TargetLayer;
    }

    public void SetNexus(Transform Nexus)
    {
        this.Nexus = Nexus;
    }
    #endregion
    public void SetDamage(int damage)
    {
        this.Damage = damage;
    }

    // SetList

    public void SetSpawendList(List<GameObject> list)
    {
        SpawnedList = list;
    }
}
