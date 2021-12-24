using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacemnetManager placementManager;

    List<Vector3Int> temporaryPlacementPosition = new List<Vector3Int>();

    public GameObject roadStraight;

    public void PlaceRoad(Vector3Int position)
    {
        if (placementManager.CheckPositionInBound(position) == false)
        {
            Debug.Log("OutBound");
            return;
        }
           
        if (placementManager.CheckPositionIsFree(position) == false)
        {
            return;
        }
        

        placementManager.PlaceTemporaryStructure(position, roadStraight, CellType.Road);
    }

}
