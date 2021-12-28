using SVS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public CameraMovement camreMovement;
    public RoadManager roadManager;
    public InputManager inputManger;

    public UIController uiController;

    public StructureManager structureManager;

    private void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler; 
        uiController.OnHousePlacement += HousePlacementHandler; 
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
        uiController.OnBigStructurePlacement += BigStructurePlacementHandler;
      
    }

    private void BigStructurePlacementHandler()
    {
        ClearInputActions();
        inputManger.OnMouseClick += structureManager.PlaceBigStructure;
    }

    private void SpecialPlacementHandler()
    {
        ClearInputActions();
        inputManger.OnMouseClick += structureManager.PlaceSpecial;
       
    }

    private void HousePlacementHandler()
    {
        ClearInputActions();
        inputManger.OnMouseClick += structureManager.PlaceHouse;
    }

    private void RoadPlacementHandler()
    {
        ClearInputActions();

        inputManger.OnMouseClick += roadManager.PlaceRoad;
        inputManger.OnMouseHold += roadManager.PlaceRoad;
        inputManger.OnMouseUp += roadManager.FinishPlacingRoad;
    }

    private void ClearInputActions()
    {
        inputManger.OnMouseClick = null;
        inputManger.OnMouseHold = null;
        inputManger.OnMouseUp = null;
    }

    private void Update()
    {
        camreMovement.MoveCamera(new Vector3(inputManger.CameraMovementVector.x, 0, inputManger.CameraMovementVector.y));
    }
}
