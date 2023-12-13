using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private Transform Nexus;
    [SerializeField] private LayerMask TargetLayer;

    [SerializeField] private GameObject ShipPrefab;

    private WaitForSeconds WaitFor1S = new WaitForSeconds(1);
    public float MoveSpeed;

    private float circleR;
    private float deg;
    private float objSpeed;

    public void StartUnit()
    {
        StartCoroutine(CUnit());
    }

    private IEnumerator CUnit()
    {
        while (true)
        {
            rb2D.velocity = CalDirection();
            deg += Time.deltaTime * objSpeed;
            if (deg < 360)
            {
                var rad = Mathf.Deg2Rad * (deg);
                var x = circleR * Mathf.Sin(rad);
                var y = circleR * Mathf.Cos(rad);
                Vector3 DirToNexeus = Nexus.position + new Vector3(x, y);
                transform.position = DirToNexeus;
                DirToNexeus = Nexus.position - DirToNexeus;
                var angle = Mathf.Atan2(DirToNexeus.y, DirToNexeus.x) * Mathf.Rad2Deg;

            
                circleR -= Time.deltaTime * 0.5f;
                if (circleR <= 0)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                deg = 0;
            }



            yield return null;
        }
    }
    

    private IEnumerator CAttack()
    {
        while (true)
        {
            yield return WaitFor1S;
        }
    }
    private Vector3 CalDirection()
    {
        return (transform.position - Nexus.position).normalized * MoveSpeed;
    }
    public void SetNexus(Transform Nexus)
    {
       this.Nexus = Nexus;
    }

    public void SetTargetLayer(LayerMask TargetLayer)
    {
       this.TargetLayer = TargetLayer;
    }
}
