using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeCarverManager : MonoBehaviour
{
    private GeneratorManager _genManager;
    private ChunkManager _chunkManager;

    [field: SerializeField] public CaveBiome CaveBiome { get; private set; }
    [field: SerializeField] public int IndexOfCarver { get; set; }

    private bool _inCarvingMode;
    public bool EnterCarvingMode;

    [field: SerializeField] public RuntimeCarver RuntimeCarver { get; private set; }

    private List<RuntimeCarver> _spawnedCarvers = new List<RuntimeCarver>();
    public RuntimeCarver Selected { get; private set; }
    public int SelectedIdx { get; private set; }

    private Camera _camera;

    public bool RenderLines { get; private set; }

    private void Awake()
    {
        _genManager = GetComponent<GeneratorManager>();
        _chunkManager = GetComponent<ChunkManager>();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        InputManager.Actions.visualiser.spawn_carver.performed += Spawn_carver_performed;
        InputManager.Actions.visualiser.step_to_next_node.performed += Step_to_next_node_performed;
        InputManager.Actions.visualiser.step.performed += Step_performed;
        InputManager.Actions.visualiser.select_next.performed += Select_next_performed;
        InputManager.Actions.visualiser.select_prev.performed += Select_prev_performed;
        InputManager.Actions.visualiser.toggle_line.performed += Toggle_line_performed;
    }

    private void Toggle_line_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        RenderLines = !RenderLines;
        foreach (RuntimeCarver c in _spawnedCarvers)
        {
            c.lr.enabled = RenderLines;
        }
    }

    private void Select_prev_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        SelectedIdx = SelectedIdx - 1 % _spawnedCarvers.Count;
    }

    private void Select_next_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        SelectedIdx = SelectedIdx + 1 % _spawnedCarvers.Count;
    }

    private void Step_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Selected)
        {
            Selected.Step();
        }
    }

    private void Step_to_next_node_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Selected)
        {
            Selected.ProgressToNextNode();
        }
    }

    private void Spawn_carver_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var pointerWorldPos = _camera.ScreenToWorldPoint(InputManager.Actions.visualiser.mouse_position.ReadValue<Vector2>());
        SpawnCarver(pointerWorldPos);
    }

    private void OnDisable()
    {
        InputManager.Actions.visualiser.spawn_carver.performed -= Spawn_carver_performed;
        InputManager.Actions.visualiser.step_to_next_node.performed -= Step_to_next_node_performed;
        InputManager.Actions.visualiser.step.performed -= Step_performed;
        InputManager.Actions.visualiser.select_next.performed -= Select_next_performed;
        InputManager.Actions.visualiser.select_prev.performed -= Select_prev_performed;
        InputManager.Actions.visualiser.toggle_line.performed -= Toggle_line_performed;
    }

    private void Update()
    {
        if (!_inCarvingMode && EnterCarvingMode)
        {
            _inCarvingMode = true;

            InputManager.SetMode(InputMode.Visualiser);
        }
        if (_inCarvingMode && !EnterCarvingMode)
        {
            _inCarvingMode = false;

            InputManager.SetMode(InputMode.Gameplay);
        }

        if (_spawnedCarvers.Count > 0)
        Selected = _spawnedCarvers[SelectedIdx];
    }

    public void SpawnCarver(Vector2 worldPosition)
    {
        var newRuntimeCarver = Instantiate<RuntimeCarver>(RuntimeCarver, worldPosition, Quaternion.identity, transform);
        newRuntimeCarver.ChunkManager = _chunkManager;
        _spawnedCarvers.Add(newRuntimeCarver);

        var carverParams = CaveBiome.CarverWeightings[IndexOfCarver].CarverParameters;
        var carverHeadParams = CaveBiome.CarverWeightings[IndexOfCarver].CarverHeadParameters;
        var carverHead = (CarverHead)CaveBiome.CarverWeightings[IndexOfCarver].CarverHead.Clone();
        newRuntimeCarver.InitialiseCarver(_genManager.World, _genManager.SurfaceLevels, CaveBiome.MeanCaveAmplitude, Tiles.Air, worldPosition, carverParams, carverHeadParams, carverHead);
        SelectedIdx = _spawnedCarvers.Count - 1;

    }


}
