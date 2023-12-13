using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField] float _forwardSpeed;
    [SerializeField] float _turnSpeed;
    public List<GameObject> _boidList;

    [SerializeField] float _weightforward;
    [SerializeField] float _weightCohesion;
    [SerializeField] float _weightSeparation;
    [SerializeField] float _weightAlignment;

    private Rigidbody2D _rigidbody2D;
    private Transform _transform;

    public Transform Target;
    public LayerMask TargetLayer;

    public enum UnitState
    {
        Idle, Attack
    }
    public UnitState State; 

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _transform = transform;
        State= UnitState.Idle;
    }

    private void FixedUpdate()
    {
        //apply velocity
        _rigidbody2D.velocity = CalculateVelocity();

        //rotate toward velocity direction
        FaceFront();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            this.State = UnitState.Attack;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & TargetLayer.value) != 0)
        {
            this.State = UnitState.Idle;
        }
    }

    Vector2 CalculateVelocity()
    {
        if (this.State == UnitState.Idle)
        {
            Vector2 ToTarget = (Target.position - _transform.position).normalized;
            //adding all velocity of all rules
            Vector2 velocity = (
                _weightforward * ToTarget
                + _weightCohesion * Rule1(_boidList)
                + _weightSeparation * Rule2(_boidList)
            //   + _weightAlignment * Rule3(_boidList)
            ).normalized * _forwardSpeed;

            // Debugging information
            Debug.DrawLine(_transform.position, (Vector2)_transform.position + ToTarget, Color.green); // ToTarget
            Debug.DrawLine(_transform.position, (Vector2)_transform.position + Rule1(_boidList), Color.blue); // Rule1
            Debug.DrawLine(_transform.position, (Vector2)_transform.position + Rule2(_boidList), Color.red); // Rule2
        //    Debug.DrawLine(_transform.position, (Vector2)_transform.position + Rule3(_boidList), Color.yellow); // Rule3

            return velocity;
        }
        return Vector2.zero;
    }

    //rotate the boid to current velocity
    void FaceFront()
    {
        float step = Time.fixedDeltaTime * _turnSpeed;
        Vector3 newDir = Vector3.RotateTowards(-_transform.up, _rigidbody2D.velocity, step, 0);

        float zOffset = Vector2.SignedAngle(-_transform.up, newDir);
        _transform.Rotate(Vector3.forward, zOffset);
    }

    #region Cohesion

    Vector2 Rule1(List<GameObject> _boidList)
    {
        Vector2 direction;

        //get centrol position
        Vector2 centerPos = Vector2.zero;
        foreach (var boid in _boidList.Where(boid => boid != this))
        {
            centerPos += (Vector2)boid.transform.position;
        }
        if (_boidList.Count != 0)
        {
            centerPos /= _boidList.Count;
        }
        else
        {
            centerPos = _transform.position;
        }

        //get direction
        direction = (centerPos - (Vector2)this.transform.position).normalized;


        return direction;
    }

    #endregion

    #region Avoidance
  
    Vector2 Rule2(List<GameObject> _boidList)
    {
        Vector2 direction = Vector2.zero;
        float found = 0;

        foreach (var boid in _boidList.Where(boid => boid != this))
        {
            Vector2 awayboidVec = (Vector2)_transform.position - (Vector2)boid.transform.position;
            //the closer the bigger weight it get
            found++;
            direction += awayboidVec.normalized;
        }
       
        if(found > 0)
        {
            direction /= found;
        }


        return direction;
    }
    #endregion

    #region Alignment
    Vector2 Rule3(List<GameObject> _boidList)
    {
        Vector2 direction = new Vector2();

        Vector2 centrolVelocity = Vector2.zero;
        foreach (var boid in _boidList.Where(boid => boid != this))
        {
            centrolVelocity += boid.GetComponent<Rigidbody2D>().velocity;
        }
        if (_boidList.Count != 0)
        {
            centrolVelocity /= _boidList.Count;
        }
        else
        {
            centrolVelocity = _rigidbody2D.velocity;
        }
        direction = centrolVelocity.normalized;

        return direction;
    }
    #endregion
}