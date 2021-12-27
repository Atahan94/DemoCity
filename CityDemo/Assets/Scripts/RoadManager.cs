using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacemnetManager placementManager;

    List<Vector3Int> temporaryPlacementPosition = new List<Vector3Int>();
    List<Vector3Int> roadPositionToRecheck = new List<Vector3Int>();

    private Vector3Int starPos;
    public bool placementMode = false;

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
        if(placementMode == false) 
        {
            temporaryPlacementPosition.Clear();
            roadPositionToRecheck.Clear();

            placementMode = true;
            starPos = position;

            temporaryPlacementPosition.Add(position);
            placementManager.PlaceTemporaryStructure(position, roadFixer.deadEnd, CellType.Road);
            Debug.Log("Placed");
            
        }
        else 
        {
            placementManager.RemoveAllTemporaryStructures();
            temporaryPlacementPosition.Clear();

            foreach (var positionToFix in roadPositionToRecheck)
            {
                roadFixer.FixeRoadAtPosition(placementManager, positionToFix);
            }

            roadPositionToRecheck.Clear();

            temporaryPlacementPosition = placementManager.GetPathBetween(starPos, position);

            foreach (var pos in temporaryPlacementPosition) 
            {
                if (placementManager.CheckPositionIsFree(pos) == false)
                    return;
                placementManager.PlaceTemporaryStructure(pos, roadFixer.deadEnd, CellType.Road);
            }

        }
        FixedRoadPrefab();
    }

    private void FixedRoadPrefab()
    {
       foreach(var item in temporaryPlacementPosition)
       {
            Debug.Log($"F:{item}");
            roadFixer.FixeRoadAtPosition(placementManager, item);
            var neighbours = placementManager.GetNeighbourOfTypesFor(item, CellType.Road);
            foreach (var roadPos in neighbours)
            {
                if(roadPositionToRecheck.Contains(roadPos) == false) 
                {
                    roadPositionToRecheck.Add(roadPos);
                }
                
            }
       }
        foreach (var item in roadPositionToRecheck)
        {
            roadFixer.FixeRoadAtPosition(placementManager, item);
        }
    }
    public void FinishPlacingRoad() 
    {
        placementMode = false;
        placementManager.AddStructuresToStructureDictionary();
        if (temporaryPlacementPosition.Count > 0) 
        {
            AudioPlayer.instance.PlayPlacementSound();
        }
        temporaryPlacementPosition.Clear();
        starPos = Vector3Int.zero;
    }
}
