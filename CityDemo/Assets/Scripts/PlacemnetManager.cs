using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class at1 
{
   public CellType[] am = new CellType[15];
}

public class PlacemnetManager : MonoBehaviour
{

    public int width, height;
    Grid placementGrid;


    private Dictionary<Vector3Int, StructureModel> temporaryRoadObjects = new Dictionary<Vector3Int, StructureModel>();



    [SerializeField]
    private at1[] at;

    private void Start()
    {
        placementGrid = new Grid(width, height);

        at = new at1[width];

        //for (int i = 0; i < 15; i++)
        //{
        //    for (int j = 0; j < 15; j++)
        //    {

        //        at[i].am[j] = (CellType)placementGrid[i,j];
        //    }
        //}
    }
    internal CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        return placementGrid.GetAllAdjacentCellTypes(position.x,position.z);
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
        Debug.Log(placementGrid[position.x, position.y]);
        return placementGrid[position.x, position.z] == type;
    }

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        placementGrid[position.x, position.y] = type;
       // at[position.x].am[position.z] = type;
        StructureModel structure = CreateANewStructureModel(position, structurePrefab, type);
        temporaryRoadObjects.Add(position, structure);
    }

    private StructureModel CreateANewStructureModel(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        GameObject structure = new GameObject(type.ToString());
        structure.transform.SetParent(transform); 
        structure.transform.localPosition = position;
        var structureModel = structure.AddComponent<StructureModel>();
        structureModel.CreateModel(structurePrefab);
        return structureModel;
    }
    public void ModifyStructureModel(Vector3Int position, GameObject newModel, Quaternion rotation) 
    {
        if (temporaryRoadObjects.ContainsKey(position)) 
        {
            temporaryRoadObjects[position].SwapModel(newModel, rotation);
        }
    }
}
