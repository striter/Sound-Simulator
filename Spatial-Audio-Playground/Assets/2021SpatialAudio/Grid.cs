﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector3 gridWorldSize;
    public float nodeRadius;
    Node[,,] grid;

    private Vector3 offsetPosition;

    float nodeDiameter;
    int gridSizeX, gridSizeY, gridSizeZ;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        gridSizeZ = Mathf.RoundToInt(gridWorldSize.z / nodeDiameter);
        CreateGrid();
    }
    
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY, gridSizeZ];
        offsetPosition = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2 - Vector3.forward * gridWorldSize.z / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 worldPoint = offsetPosition + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius) + Vector3.forward * (z * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius/2,unwalkableMask));
                    grid[x, y, z] = new Node(walkable, worldPoint);
                }
            }
        }
    }

    public Node NodeFromWorldPosition(Vector3 worldPosition){

        float percentX = (worldPosition.x - offsetPosition.x) / gridWorldSize.x;
        //Debug.Log("percentX = "+ percentX);
        float percentY = (worldPosition.y- offsetPosition.y) / gridWorldSize.y;
        //Debug.Log("percentY = "+ percentY);
        float percentZ = (worldPosition.z- offsetPosition.z) / gridWorldSize.z;
        //Debug.Log("percentZ = "+ percentZ);
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        percentZ = Mathf.Clamp01(percentZ);

        int x = Mathf.RoundToInt((gridSizeX)* percentX);
        int y = Mathf.RoundToInt((gridSizeY)* percentY);
        int z = Mathf.RoundToInt((gridSizeZ)* percentZ);
        
        x =  Mathf.RoundToInt(Mathf.Clamp(x,0,gridSizeX-1));
        y =  Mathf.RoundToInt(Mathf.Clamp(y,0,gridSizeY-1));
        z =  Mathf.RoundToInt(Mathf.Clamp(z,0,gridSizeZ-1));

        return grid[x,y,z];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, gridWorldSize.z));
        if (grid != null)
        {
            Node playerNode = NodeFromWorldPosition(player.position);
            Debug.Log("Player =" + player.position.x+","+player.position.y+","+player.position.z +"   Point = "+playerNode.worldPosition.x+","+playerNode.worldPosition.y+","+playerNode.worldPosition.z);
            Debug.Log("Distance="+ Vector3.Distance(player.position,playerNode.worldPosition));
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (playerNode ==n)
                {
                    Gizmos.color= Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter /5));
            }
        }
    }
}
