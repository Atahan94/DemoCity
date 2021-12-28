using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureManager : MonoBehaviour
{
    public StructurePrefabWeighted[] housePrefabs, specialPrefabs, bigStructuresPrefabs;
    public PlacemnetManager placementManger;

    private float[] houseWeights, specialWeights, bigstructureweights;

    private void Start()
    {
        houseWeights = housePrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        specialWeights = specialPrefabs.Select(prefabStats => prefabStats.weight).ToArray();
        bigstructureweights = bigStructuresPrefabs.Select(prefabStats => prefabStats.weight).ToArray();


        
    }
    public void PlaceHouse(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            int randomindex = GetRandomWieghtedIndex(houseWeights);
            placementManger.PlaceObjectOnTheMap(position, housePrefabs[randomindex].prefab, CellType.Structure);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    internal void PlaceBigStructure(Vector3Int position)
    {
        int width = 2;
        int height = 2;
        if(CheckBigStructure(position, width, height)) 
        {
            int randomindex = GetRandomWieghtedIndex(houseWeights);
            placementManger.PlaceObjectOnTheMap(position, bigStructuresPrefabs[randomindex].prefab, CellType.Structure, width, height);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    private bool CheckBigStructure(Vector3Int position, int width, int height)
    {
        bool nearRoad = false;
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                var newPosition = position + new Vector3Int(x, 0, z);
               
                if (DefaultCheck(newPosition) == false)
                {
                    return false;
                }
                if (nearRoad == false)
                {
                    nearRoad = RoadCheck(newPosition);
                }
                   
            }
        }
        return nearRoad;
    }

    public void PlaceSpecial(Vector3Int position)
    {
        if (CheckPositionBeforePlacement(position))
        {
            int randomindex = GetRandomWieghtedIndex(specialWeights);
            placementManger.PlaceObjectOnTheMap(position, specialPrefabs[randomindex].prefab, CellType.SpecialStructure);
            AudioPlayer.instance.PlayPlacementSound();
        }
    }

    private int GetRandomWieghtedIndex(float[] weights)
    {
        float sum = 0f;
        for (int i = 0; i <weights.Length; i++)
        {
            sum += weights[i];
        }
        float randomValue = UnityEngine.Random.Range(0, sum);
        float tempsum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if( randomValue >= tempsum && randomValue < tempsum + weights[i]) 
            {
                return i;
            }
            tempsum += weights[i];
        }
        return 0;
    }

    private bool CheckPositionBeforePlacement(Vector3Int position)
    {
        if(DefaultCheck(position) == false)
        {
            return false;
        }

        if(RoadCheck(position)) 
        {
            return false;
        }
       
        return true;
    }

    private bool RoadCheck(Vector3Int position)
    {
        if (placementManger.GetNeighbourOfTypesFor(position, CellType.Road).Count <= 0)
        {
            Debug.Log("NoRoad");
            return false;
        }
        return true;
    }

    private bool DefaultCheck(Vector3Int position)
    {
        if (placementManger.CheckPositionInBound(position) == false)
        {
            Debug.Log("OutOfBounds");
            return false;
        }
        if (placementManger.CheckPositionIsFree(position) == false)
        {
            Debug.Log("FullPosition");
            return false;
        }
        return true;
    }
}
[Serializable]
public struct StructurePrefabWeighted 
{
    public GameObject prefab;
    [Range(0,1)]
    public float weight;
}
