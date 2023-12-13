using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISetTarget
{
    public void SetTarget(Transform target);

    public void SetTargetLayer(LayerMask TargetLayer);

    public void SetNexus(Transform Nexus);

    public void SetSpawendList(List<GameObject> list);
}
