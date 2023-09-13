using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carver 
{
    private SpatialArray<Tiles> _world;
    private int[] _surfaceLevels;
    private float _amplitude;
    public Tiles TileToSet { get; set; }

    private Vector2 _position;
    private float _orientation; // Radians. +z axis. 0 degrees is facing directly upwards, in the +y direction. + orientation is counterclockwise rotation

    public Vector2 Position  // MAP SPACE
    {
        get => _position;
        set => _position = value;
    }
    public float Orientation
    {
        get => _orientation;
        set => _orientation = value % (2 * Mathf.PI);
    }

    private readonly CarverParameters _carverParameters;
    private readonly CarverHeadParameters _carverHeadParameters;
    private readonly CarverHead _carverHead;

    public bool Alive { get; private set; }
    public int nSteps { get; private set; }
    public List<Vector2> Nodes { get; set; }

    private NoiseGenerator _noiseGen;
    private float[] _noiseMap;
    private float _expectedStepDistance;
    private const float _probabilityThreshold = 0.5f;
    private static int _nStepCyclesForNoiseGen = 50;
    private float _expectedNSteps;
    private float _expectedTotalDistance;

    public bool VaryCarverHeadPos = false;
    private Vector2 _carverHeadPos;
    private float _carverHeadOrientation;

    // for individual steps
    private int _nStepsReqdToNextNode;
    private Vector2 _directionVectorForCarver;

    public Carver(SpatialArray<Tiles> world, int[] surfaceLevels, float amplitude, Tiles tileToSet, Vector2 position, CarverParameters carverParameters, CarverHead head, CarverHeadParameters headParams)
    {
        _world = world;
        _surfaceLevels = surfaceLevels;
        _amplitude = amplitude;
        TileToSet = tileToSet;
        _carverParameters = carverParameters;
        Position = position;
        Orientation = Random.value * 2 * Mathf.PI;

        _carverHead = head;
        _carverHeadParameters = headParams;

        Alive = true;
        nSteps = 0;

        Nodes = new List<Vector2>();
        Nodes.Add(Position);

        _noiseGen = new NoiseGenerator(_carverHeadParameters.CarvingNoiseParameters);
        _expectedStepDistance = carverParameters.MeanNodeDistance * (1 - carverParameters.DeathProbability);
        _expectedNSteps = Mathf.Log10(_probabilityThreshold) / Mathf.Log10(1 - carverParameters.DeathProbability);
        _expectedTotalDistance = _expectedNSteps * carverParameters.MeanNodeDistance * _probabilityThreshold;

        _noiseMap = _noiseGen.Generate(Mathf.CeilToInt(_nStepCyclesForNoiseGen * _expectedNSteps));
        //Debug.Log(_noiseMap);
    }

    public Carver(SpatialArray<Tiles> world, int[] surfaceLevels, float amplitude, Tiles tileToSet, Vector2 position, float orientation, CarverParameters carverParameters, CarverHead head, CarverHeadParameters headParams)
    {
        _world = world;
        _surfaceLevels = surfaceLevels;
        _amplitude = amplitude;
        TileToSet = tileToSet;
        _carverParameters = carverParameters;
        Position = position;
        Orientation = orientation;

        _carverHead = head;
        _carverHeadParameters = headParams;

        Alive = true;
        nSteps = 0;

        Nodes = new List<Vector2>();
        Nodes.Add(Position);

        _noiseGen = new NoiseGenerator(_carverHeadParameters.CarvingNoiseParameters);
        _expectedStepDistance = carverParameters.MeanNodeDistance * (1 - carverParameters.DeathProbability);
        _expectedNSteps = Mathf.Log10(_probabilityThreshold) / Mathf.Log10(1 - carverParameters.DeathProbability);
        _expectedTotalDistance = _expectedNSteps * carverParameters.MeanNodeDistance * _probabilityThreshold;

        _noiseMap = _noiseGen.Generate(Mathf.CeilToInt(_nStepCyclesForNoiseGen * _expectedNSteps));
        Debug.Log(_noiseMap);
    }


    private int NextNode()
    {
        int newNodeDist = Mathf.RoundToInt(_carverParameters.MeanNodeDistance + (Random.value * 2 - 1) * _carverParameters.NodeDistanceSpread);
        int nStepsReqd = newNodeDist / _carverParameters.StepDistance;
        // Recalculate orientation at the start of each node step
        Orientation += (Random.value * 2 - 1) * _carverParameters.MeanTurningAngle + (Random.value * 2 - 1) * _carverParameters.TurningAngleSpread;

        return nStepsReqd;
    }

    private HashSet<Vector2Int> Step(Vector2 directionVectorForCarver)
    {
        // revise this to make it work with frontal carving
        _carverHeadPos = Position + (VaryCarverHeadPos ? Random.insideUnitCircle * _carverParameters.CarverHeadPositionSpread : Vector2.zero);
        _carverHeadOrientation = Orientation;  // yet to add variation here
        var res = Carve(_carverHeadPos, _carverHeadOrientation);

        Position += directionVectorForCarver;
        nSteps++;

        return res;
    }

    //private HashSet<Vector2Int> WalkToNextNode()
    //{
    //    var affectedTiles = new HashSet<Vector2Int>();

    //    //Nodes.Add(Position);

    //    //int nStepsReqd = NextNode();

    //    //Vector2 directionVectorForCarver = new Vector2(-Mathf.Sin(Orientation), Mathf.Cos(Orientation));

    //    for (int i = 0; i < _nStepsReqdToNextNode; i++)
    //    {
    //        affectedTiles.UnionWith(Step(_directionVectorForCarver));
    //    }

    //    return affectedTiles;
    //}

    public HashSet<Vector2Int> DoStep()
    {
        if (!Alive)
        {
            return new HashSet<Vector2Int>();
        }

        if (_nStepsReqdToNextNode == 0)
        {
            Nodes.Add(Position);
            _nStepsReqdToNextNode = NextNode();
            _directionVectorForCarver = new Vector2(-Mathf.Sin(Orientation), Mathf.Cos(Orientation));
        }

        var res = Step(_directionVectorForCarver);
        _nStepsReqdToNextNode--;

        if (_nStepsReqdToNextNode == 0)
        {
            // update alive state
            if (Position.x < 0 || Position.x > _world.Length - 1 || Position.y < 0 || Position.y > _world.Height - 1 || Random.value < _carverParameters.DeathProbability)
            {
                Kill();
            }
        }

        return res;
    }

    private void Kill()
    {
        Alive = false;
        //Debug.Log($"Number of nodes: {_nodes.Count}; Number of steps taken: {nSteps}");
    }

    private HashSet<Vector2Int> Carve(Vector2 carveAtPos, float orientation)
    {
        var noiseLevelUnmod = _noiseMap[nSteps % _noiseMap.Length];
        var noiseLevel = _carverHeadParameters.CarvingNoiseCurve.Evaluate(_noiseMap[nSteps % _noiseMap.Length]);
        var radiusAdj = _carverHeadParameters.CarvingNoiseCurve.Evaluate(_noiseMap[nSteps % _noiseMap.Length]) * _carverHeadParameters.CarvingRadiusMaxSpread;
        var radius = Mathf.RoundToInt(_amplitude * (_carverHeadParameters.MinCarvingRadius + radiusAdj));

        //Debug.Log($"Number of steps: {nSteps}");
        //Debug.Log($"unmoderated noise: {noiseLevelUnmod}; noise: {noiseLevel}; radius adjustment: {radiusAdj}; radius: {radius}");

        _carverHead.PrimarySize = radius;
        var affectedTiles = _carverHead.Carve(ChunkManager.ToWorldPosition(carveAtPos), _world, TileToSet, TileManager.AllPhysicalTiles);

        // TODO change to work with HashSet<(Vector2Int, Tiles)> return type
        var res = new HashSet<Vector2Int>();
        foreach ((var pos, var tile) in affectedTiles)
        {
            res.Add(pos);
        }
        return res;
    }

    public HashSet<Vector2Int> Walk()  // TODO: get caller to pass in hashset
    {
        if (Alive)
        {
            if (_nStepsReqdToNextNode == 0)
            {
                Nodes.Add(Position);
                _nStepsReqdToNextNode = NextNode();
                _directionVectorForCarver = new Vector2(-Mathf.Sin(Orientation), Mathf.Cos(Orientation));
            }

            var affectedTiles = new HashSet<Vector2Int>();
            for (int i = 0; i < _nStepsReqdToNextNode; i++)
            {
                affectedTiles.UnionWith(Step(_directionVectorForCarver));
            }
            _nStepsReqdToNextNode = 0;

            // check whether dead
            if (Position.x < 0 || Position.x > _world.Length - 1 || Position.y < 0 || Position.y > _world.Height - 1 || Random.value < _carverParameters.DeathProbability)
            {
                Kill();
            }

            return affectedTiles;
        }
        return new HashSet<Vector2Int>();
    }

    public void Run()
    {
        while (Alive)
        {
            Walk();
        }
    }
}

//public enum CarverMode
//{
//    Tunnel,
//    Chamber,
//    Terminator,
//    Mouth
//}
