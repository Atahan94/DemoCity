using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadFixer : MonoBehaviour
{
    public GameObject deadEnd, roadStraight, corner, threeWay, fourWay;

    public void FixeRoadAtPosition(PlacemnetManager placemnetManager, Vector3Int temporaryPosition)
    {
        var result = placemnetManager.GetNeighbourTypesFor(temporaryPosition);
        int roadCount = 0;
        roadCount = result.Where(x => x == CellType.Road).Count();
        Debug.Log(result);
        if (roadCount == 0 || roadCount == 1) 
        {
            CreateDeadEnd(placemnetManager, result, temporaryPosition);
        }
        else if(roadCount == 2) 
        {
            if (CreateStraightRoad(placemnetManager, result, temporaryPosition))
                return;

            CreateCorner(placemnetManager, result, temporaryPosition);
        }else if (roadCount == 3) 
        {
            CreateThreeWay(placemnetManager, result, temporaryPosition);
        }
        else 
        {
            CreateFourWay(placemnetManager, result, temporaryPosition);
        }

    }

    private void CreateFourWay(PlacemnetManager placemnetManager, CellType[] result, Vector3Int temporaryPosition)
    {
        placemnetManager.ModifyStructureModel(temporaryPosition, fourWay, Quaternion.identity);
    }

    // CellType[] result == [Left, Up, Right, Down]
    private void CreateThreeWay(PlacemnetManager placemnetManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if(result[1] == CellType.Road &&  result[2] == CellType.Road && result[3] == CellType.Road) 
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.identity);
        }
        else if (result[2] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0,90,0));
        }
        else if (result[1] == CellType.Road && result[3] == CellType.Road && result[0] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 180, 0));
        }
        else if (result[0] == CellType.Road && result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, threeWay, Quaternion.Euler(0, 180, 0));
        }
    }

    private void CreateCorner(PlacemnetManager placemnetManager, CellType[] result, Vector3Int temporaryPosition)
    {

        if (result[1] == CellType.Road && result[2] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 90, 0));
        }
        else if (result[2] == CellType.Road && result[3] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 180, 0));
        }
        else if (result[0] == CellType.Road && result[3] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.Euler(0, 270, 0));
        }
        else if (result[0] == CellType.Road && result[1] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, corner, Quaternion.identity);
        }
    }

    private bool CreateStraightRoad(PlacemnetManager placemnetManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[0] == CellType.Road && result[2] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, roadStraight, Quaternion.identity);
            return true;
        }
        else if(result[1] == CellType.Road && result[3] == CellType.Road) 
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, roadStraight, Quaternion.Euler(0,90,0));
            return false;
        }
        return false;
    }

    private void CreateDeadEnd(PlacemnetManager placemnetManager, CellType[] result, Vector3Int temporaryPosition)
    {
        if (result[1] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 270, 0));
        }
        else if (result[2] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.identity);
        }
        else if (result[3] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 90, 0));
        }
        else if (result[0] == CellType.Road)
        {
            placemnetManager.ModifyStructureModel(temporaryPosition, deadEnd, Quaternion.Euler(0, 180, 0));
        }
    }
}
