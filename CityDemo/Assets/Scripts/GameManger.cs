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

    private void Start()
    {
        uiController.OnRoadPlacement += RoadPlacementHandler; 
        uiController.OnRoadPlacement += HousePlacementHandler; 
        uiController.OnSpecialPlacement += SpecialPlacementHandler;
      
    }

    private void SpecialPlacementHandler()
    {
        throw new NotImplementedException();
    }

    private void HousePlacementHandler()
    {
        throw new NotImplementedException();
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
        inputManger.OnMouseClick += null;
        inputManger.OnMouseHold += null;
        inputManger.OnMouseUp += null;
    }

    private void Update()
    {
        camreMovement.MoveCamera(new Vector3(inputManger.CameraMovementVector.x, 0, inputManger.CameraMovementVector.y));
    }
}
