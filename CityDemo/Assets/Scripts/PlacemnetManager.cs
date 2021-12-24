using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacemnetManager : MonoBehaviour
{
    public int width, height;
    Grid placementGrid;

    private void Start()
    {
        placementGrid = new Grid(width, height);
    }


    internal bool CheckPositionInBound(Vector3Int position)
    {
        if (position.x >= 0 && position.y >= 0 && position.z >= 0 && position.z < height)
        {
            return true;
        }
        return false;
    }

    internal bool CheckPositionIsFree(Vector3Int position)
    {
        return CheckPositionIsOfType(position, CellType.Empty);
    }

    private bool CheckPositionIsOfType(Vector3Int position, CellType type)
    {
        return placementGrid[position.x, position.y] == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject roadStraight, CellType type)
    {
        placementGrid[position.x, position.y] = type;
        GameObject newStructure = Instantiate(roadStraight, position, Quaternion.identity);
    }
}
