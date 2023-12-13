using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGrid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    private ANode[,] grid;

    private float nodeDiameter;
    private int girdSizeX;
    private int girdSizeY;

    private void Start()
    {
        
    }

    private void CreateGrid()
    {

    }

    private void OnDrawGizmos()
    {
        
    }

    public List<ANode> GetNeighbours(ANode node)
    {
        return null ;
    }

    public ANode GetNodeFromWorldPoint(Vector3 worldPoint)
    {
        return null;
    }
}
