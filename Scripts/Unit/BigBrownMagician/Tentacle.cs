using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : Weapon
{
    [SerializeField] private Collider2D Collider;
    [SerializeField] private Transform ColliderPosition;
    [SerializeField] private LineRenderer LineRenderer;
    private float sign = -1;

    private void Awake()
    {
        if (LineRenderer == null)
        {
            LineRenderer = GetComponent<LineRenderer>();
        }

        LineRenderer.positionCount = 3;
        LineRenderer.enabled = false;
        Collider.enabled = false;
    }


    public void Play(Vector3 from , Vector3 to)
    {
        LineRenderer.enabled = true;
        Vector3 mid = from + to;
        mid *= 0.5f;
        mid += Vector3.up * sign;
        sign *= -1;
        LineRenderer.SetPosition(0, from);
        LineRenderer.SetPosition(1, mid);
        LineRenderer.SetPosition(2, to);
        ColliderPosition.position = to;
        Collider.enabled = true;
    }

    public void Stop()
    {
        Collider.enabled = false;
        LineRenderer.enabled=false;
    }
}
