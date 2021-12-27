using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class at1 
{
    public CellType[] am;
    public at1(int i)
    {
            am = new CellType[i];
    }
}

public class PlacemnetManager : MonoBehaviour
{

    public int width, height;
    Grid placementGrid;


    private Dictionary<Vector3Int, StructureModel> temporaryRoadObjects = new Dictionary<Vector3Int, StructureModel>();
    private Dictionary<Vector3Int, StructureModel> structureDic = new Dictionary<Vector3Int, StructureModel>();



    [SerializeField]
    private at1[] at;

    private void Start()
    {
        placementGrid = new Grid(width, height);

        at = new at1[width];

        for (int i = 0; i < 15; i++)
        {
            at[i] = new at1(height);
            for (int j = 0; j < 15; j++)
            {

                at[i].am[j] = (CellType)placementGrid[i, j];
            }
        }
    }
    internal CellType[] GetNeighbourTypesFor(Vector3Int position)
    {
        CellType[] an = placementGrid.GetAllAdjacentCellTypes(position.x, position.z);
        foreach (var item in placementGrid.GetAllAdjacentCellTypes(position.x, position.z))
        {
            Debug.Log($"Type:{item}" );
        }

        return placementGrid.GetAllAdjacentCellTypes(position.x,position.z);
    }

    internal List<Vector3Int> GetPathBetween(Vector3Int starPos, Vector3Int endPos)
    {
        var result = GridSearch.AStarSearch(placementGrid, new Point(starPos.x, starPos.z), new Point(endPos.x, endPos.z));
        List<Vector3Int> path = new List<Vector3Int>();
        foreach (Point point in result) 
        {
            path.Add(new Vector3Int(point.X, 0, point.Y));
        }
        return path;
    }
    internal void RemoveAllTemporaryStructures()
    {
        foreach (var structure in temporaryRoadObjects.Values)
        {
            var position = Vector3Int.RoundToInt(structure.transform.position);
            placementGrid[position.x, position.z] = CellType.Empty;
            Destroy(structure.gameObject);
        }
        temporaryRoadObjects.Clear();
    }
    internal void AddStructuresToStructureDictionary()
    {
        foreach (var structure in temporaryRoadObjects)
        {
            structureDic.Add(structure.Key, structure.Value);
        }
        temporaryRoadObjects.Clear();
    }
    internal List<Vector3Int> GetNeighbourOfTypesFor(Vector3Int item, CellType road)
    {
        var neighbourVerticies = placementGrid.GetAdjacentCellsOfType(item.x, item.z, road);
        List<Vector3Int> neighbours = new List<Vector3Int>();
        foreach (var point in neighbourVerticies)
        {
            neighbours.Add(new Vector3Int(point.X,0,point.Y));
        }
        return neighbours;
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
        //Debug.Log(placementGrid[position.x, position.y]);
        return placementGrid[position.x, position.z] == type;
    }

    

    internal void PlaceTemporaryStructure(Vector3Int position, GameObject structurePrefab, CellType type)
    {
        at[position.x].am[position.z] = type;//To Visualize

        placementGrid[position.x, position.z] = type;
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
        else if (structureDic.ContainsKey(position))
        {
            structureDic[position].SwapModel(newModel, rotation);
        }
    }
}
