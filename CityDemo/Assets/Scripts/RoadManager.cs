using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacemnetManager placementManager;

    List<Vector3Int> temporaryPlacementPosition = new List<Vector3Int>();

    public GameObject roadStraight;

    public RoadFixer roadFixer;

    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();
    }

    public void PlaceRoad(Vector3Int position)
    {
        if (placementManager.CheckPositionInBound(position) == false)
            return;           
        if (placementManager.CheckPositionIsFree(position) == false)
            return;
        temporaryPlacementPosition.Clear();
        temporaryPlacementPosition.Add(position);
        placementManager.PlaceTemporaryStructure(position, roadStraight, CellType.Road);
        FixedRoadPrefab();
    }

    private void FixedRoadPrefab()
    {
       foreach(var item in temporaryPlacementPosition)
       {
            
            roadFixer.FixeRoadAtPosition(placementManager, item);
       }
    }
}
