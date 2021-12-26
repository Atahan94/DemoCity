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

    private void Start()
    {
        inputManger.OnMouseClick += HandleMouseClick;
    }

    private void HandleMouseClick(Vector3Int position)
    {
        //Debug.Log($"X:{position.x}, Y:{position.y}, Z:{position.z}");
        roadManager.PlaceRoad(position);
    }
    private void Update()
    {
        camreMovement.MoveCamera(new Vector3(inputManger.CameraMovementVector.x, 0, inputManger.CameraMovementVector.y));
    }
}
