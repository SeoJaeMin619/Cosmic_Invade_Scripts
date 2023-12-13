using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Drone_C : Unit, ISetTarget
{
    public enum State
    {
      Idle, Move, Attack, Dead
    }

    public State cuState;

    [SerializeField] private UnitBaseSO Data;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;

    private Vector3 NormalDirectionToTarget;
    private Vector3 DirectionToTarget;

    public Transform Nexus;
    public Transform Target;
    public LayerMask TargetLayer;

    //Animation Parameter
    private int AttackHash;
    private int MoveHash;
    private int DeadHash;

    private const string AttackString = "Attack";
    private const string MoveString = "Move";
    private const string DeadString = "Dead";

    //Stats
    public float MoveSpeed;
    public float AttackRange;
    public float AttackInterval;
    public int Damage;
    public int MaxHp;

    //Weapon
    [SerializeField] private Weapon Weapon;

    //Coroutine
    WaitForSeconds WaitForInterval;
    WaitForSeconds WaitFor30S = new WaitForSeconds(30f);
    WaitForSeconds WaitFor1S = new WaitForSeconds(1f);
    private Coroutine CUnit;
    private Coroutine CAttack;
    private Coroutine CMove;

    //Boids
    [SerializeField] float _weightSeparation;

    //For Remove From List in RoundManager
    private List<GameObject> SpawnedList;
    private void Awake()
    {
        this.MoveSpeed = Data.MoveSpeed;
        this.AttackRange = Data.AttackRange;
        this.AttackInterval = Data.AttackInterval;
        this.Damage = Data.Damage;
        this.MaxHp = Data.MaxHp;

        AttackHash = Animator.StringToHash(AttackString);
        MoveHash = Animator.StringToHash(MoveString);
        DeadHash = Animator.StringToHash(DeadString);

        MaxHP = Data.MaxHp;
        CurrnetHP = MaxHP;

        Weapon.Damage = this.Damage;
        Weapon.TargetLayer = this.TargetLayer;

        WaitForInterval = new WaitForSeconds(AttackInterval);
    }

    void Start()
    {
        CUnit= StartCoroutine(UnitAI());
    }

    private void Rotate()
    {
        float angle = Mathf.Atan2(NormalDirectionToTarget.y, NormalDirectionToTarget.x) * Mathf.Rad2Deg;
        angle += 90f;
        this.transform.eulerAngles = new Vector3(0, 0, angle);
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
                else
                {
                    Target = Nexus;
                }
            }

            CalDirection();

            if (DistanceToTarget() < AttackRange)
            {
                if (cuState != State.Attack)
                {
                    ChangeState(State.Attack);
                }
            }
            else
            {
                if (cuState != State.Move)
                {
                    ChangeState(State.Move);
                }

            }

            yield return null;
        }

    }

    private IEnumerator MoveCoroutine()
    {
        if (Target == null)
        {
            yield return WaitFor1S;
        }
        while (true)
        {
            if (cuState == State.Move)
            {
                rb2D.velocity = NormalDirectionToTarget * MoveSpeed;
            }
            else
            {
                rb2D.velocity = Vector2.zero;
            }
            Rotate();
            yield return WaitFor1S;
        }
    }

    private IEnumerator AttackCoroutine()
    {
        rb2D.velocity = Vector2.zero;
        while (cuState == State.Attack)
        {
            animator.SetTrigger(AttackHash);
            yield return WaitForInterval;
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
            Target = collision.transform;
       
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1<<collision.gameObject.layer) & TargetLayer.value) != 0 && cuState != State.Attack)
        {
            Target = collision.transform;
         
            CalDirection();
            Rotate();
            rb2D.velocity = NormalDirectionToTarget * MoveSpeed;
        }
    }
    #endregion

    //Change Animation
    #region
    private void ChangeState(State InputState)
    {
        switch (cuState)
        {
            case State.Move:
                ExitState(cuState);
                cuState = InputState;
                EnterState(InputState);
                break;
            case State.Attack:
                ExitState(cuState);
                cuState = InputState;
                EnterState(InputState);
                rb2D.velocity = Vector2.zero;
                break;
            case State.Dead:
                ExitState(cuState);
                cuState = InputState;
                EnterState(InputState);
                break;
            case State.Idle:
                cuState = InputState;
                EnterState(InputState);
                break;
        }
    }

    private void ExitState(State CuState)
    {
        if(CuState == State.Attack)
        {
            StopCoroutine(CAttack);
            return;
        }
        else
        {
            StopCoroutine(CMove);
            animator.SetBool(GetAnimHash(), false);
        }
    }

    private void EnterState(State InputState)
    {
        if (cuState == State.Attack)
        {
            CAttack = StartCoroutine(AttackCoroutine());
            animator.SetTrigger(GetAnimHash());
        }
        if(cuState == State.Move) 
        {
            CMove = StartCoroutine(MoveCoroutine());
            animator.SetBool(GetAnimHashFromInput(InputState), true);
        }
    }
    #endregion
    //Get Animation Hash
    #region
    private int GetAnimHash()
    {
        if (cuState == State.Move)
            return MoveHash;
        if (cuState == State.Attack)
            return AttackHash;
        if (cuState == State.Dead)
            return DeadHash;

        return DeadHash;
    }
    private int GetAnimHashFromInput(State Input)
    {
        if (Input == State.Move)
            return MoveHash;
        if (Input == State.Attack)
            return AttackHash;
        if (Input == State.Dead)
            return DeadHash;

        return DeadHash;
    }
    #endregion
  
    public override void Damaged(int dmg)
    {
        base.Damaged(dmg);

        if (CurrnetHP <= 0)
        {
            GameManager.goldcount += Data.gold;
            SpawnedList.Remove(this.gameObject);
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
