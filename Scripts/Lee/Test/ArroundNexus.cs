using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArroundNexus : MonoBehaviour
{
    [SerializeField] private Transform Nexus;

    private WaitForSeconds WaitFor02S = new WaitForSeconds(0.2f);
    public float deg;
    public float objSpeed;
    public float circleR;
    public Transform SpriteTr;
    private void Start()
    {
        StartCoroutine(CMove());
    }
    IEnumerator CMove()
    {
        deg = 0;
        while (true)
        {
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
              
                SpriteTr.eulerAngles = new Vector3(0, 0, angle);
                circleR -= Time.deltaTime*0.5f;
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
    
}
