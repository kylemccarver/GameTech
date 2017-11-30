﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour {

    public IntVector2 coordinates;
    public MazeRoom room;
    [Tooltip("The cost it takes to move across this cell. Higher costs = greater avoidance")]
    public int cost = 1;

    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
    private int initializedEdgeCount;

    public void Initialize(MazeRoom room)
    {
        room.Add(this);
        transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;
    }

    #region Getters
    public MazeCellEdge GetEdge (MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public MazeCellEdge[] GetEdges()
    {
        return edges;
    }
    #endregion

    #region Setters
    public void SetEdge(MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initializedEdgeCount += 1;
    }

    public void SetMaterial(Material m)
    {
        transform.GetChild(0).GetComponent<Renderer>().material = m;
    }
    #endregion

    public bool IsFullyInitialized
    {
        get
        {
            return initializedEdgeCount == MazeDirections.Count;
        }
    }

    public MazeDirection RandomUninitializedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount) - 1;
            for (int i = 0; i < MazeDirections.Count; ++i)
            {
                if(edges[i] == null)
                {
                    if(skips <= 0)
                    {
                        return (MazeDirection)i;
                    }
                    skips -= 1;
                }
            }
            throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
        }
    }

    public void OnPlayerEntered()
    {
        for(int i = 0; i < edges.Length; ++i)
        {
            edges[i].OnPlayerEntered();
        }
    }

    public void OnPlayerExited()
    {
        for(int i = 0; i < edges.Length; ++i)
        {
            edges[i].OnPlayerExited();
        }
    }
}
