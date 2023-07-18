using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModifyWorldController : ModifyWorldController
{
    private Camera _camera { get; set; }
    private ModifyOverlay _overlay { get; set; }

    //private float _currScrollDelta { get; set; }
    //[field: SerializeField] public float ScrollDeltaForOneStep { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _camera = GetComponentInChildren<Camera>();
        _overlay = _camera.GetComponent<ModifyOverlay>();
    }

    protected override void Update()
    {
        base.Update();

        _overlay.UpdatePosition(_camera.ScreenToWorldPoint(InputManager.Actions.gameplay.mouse_position.ReadValue<Vector2>()));

        if (InputManager.Actions.gameplay.secondary.IsPressed())
        {
            //Debug.Log($"tool: {BuildTool}");
            TryBuild(_camera.ScreenToWorldPoint(InputManager.Actions.gameplay.mouse_position.ReadValue<Vector2>()), GeneratorManager.TileLayerIndices.Principal);
        }
        else if (InputManager.Actions.gameplay.primary.IsPressed())
        {
            //Debug.Log($"tool: {DestructTool}");
            TryDestruct(_camera.ScreenToWorldPoint(InputManager.Actions.gameplay.mouse_position.ReadValue<Vector2>()), GeneratorManager.TileLayerIndices.Principal);
        }

        if (InputManager.Actions.ui.next.WasPressedThisFrame())
        {
            if ((int)TileToBuildWith == (int)TileManager.AllPhysicalTilesList[TileManager.AllPhysicalTilesList.Count - 1])
            {
                TileToBuildWith = (Tiles)1;
            }
            else
            {
                TileToBuildWith++;
            }
        }
        else if (InputManager.Actions.ui.previous.WasPressedThisFrame())
        {
            if ((int)TileToBuildWith == 1)
            {
                TileToBuildWith = TileManager.AllPhysicalTilesList[TileManager.AllPhysicalTilesList.Count - 1];
            }
            else
            {
                TileToBuildWith--;
            }
        }

        //Debug.Log(Input.GetAxisRaw("Mouse ScrollWheel"));
        //_currScrollDelta += Input.GetAxisRaw("Mouse ScrollWheel");
        //if (_currScrollDelta > ScrollDeltaForOneStep)
        //{
        //    if ((int)TileToBuildWith == 7)
        //    {
        //        TileToBuildWith = (Tiles)1;
        //    }
        //    else
        //    {
        //        TileToBuildWith++;
        //    }
        //    _currScrollDelta = 0;
        //}
        //else if (_currScrollDelta < -ScrollDeltaForOneStep)
        //{
        //    if ((int)TileToBuildWith == 1)
        //    {
        //        TileToBuildWith = (Tiles)7;
        //    }
        //    else
        //    {
        //        TileToBuildWith--;
        //    }
        //    _currScrollDelta = 0;
        //}
        
    }
}
