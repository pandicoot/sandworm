using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeCarver : MonoBehaviour
{
    public Carver Carver { get; private set; }

    public ChunkManager ChunkManager { get; set; }

    private SpriteRenderer _sr { get; set; }
    public LineRenderer lr { get; private set; }

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    private void Start()
    {
        lr.positionCount = 1;
        lr.SetPosition(lr.positionCount - 1, transform.position);
    }

    public void InitialiseCarver(SpatialArray<Tiles> world, int[] surfaceLevels, float amplitude, Tiles tileToSet, Vector2 position, CarverParameters carverParameters, CarverHeadParameters headParams, CarverHead head)
    {
        Carver = new Carver(world, surfaceLevels, amplitude, tileToSet, ChunkManager.ToMapPosition(position), carverParameters, head, headParams);
    }

    public void Step()
    {
        Debug.Assert(Carver != null, "Carver is null!");

        if (Carver.Alive)
        {
            var affectedTiles = Carver.DoStep();

            var affectedChunks = new HashSet<Vector2Int>();
            foreach (Vector2Int tile in affectedTiles)
            {
                affectedChunks.Add(ChunkManager.ConvertToChunkPositionInBounds(ChunkManager.ToWorldPosition(tile)));
            }
            // regenerate affected chunks
            foreach (Vector2Int chunkCoord in affectedChunks)
            {
                ChunkManager.RegenerateChunkLayerAt(chunkCoord, GeneratorManager.TileLayerIndices.Principal);
            }

            transform.position = ChunkManager.ToWorldPosition(Carver.Position);

            // draw trail
            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, transform.position);
        }
        else
        {
            Debug.Log("Carver is dead!");
            _sr.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    public void ProgressToNextNode()
    {
        Debug.Assert(Carver != null, "Carver is null!");

        if (Carver.Alive)
        {
            var affectedTiles = Carver.Walk();

            var affectedChunks = new HashSet<Vector2Int>();
            foreach (Vector2Int tile in affectedTiles)
            {
                affectedChunks.Add(ChunkManager.ConvertToChunkPositionInBounds(ChunkManager.ToWorldPosition(tile)));
            }
            // regenerate affected chunks
            foreach (Vector2Int chunkCoord in affectedChunks)
            {
                ChunkManager.RegenerateChunkLayerAt(chunkCoord, GeneratorManager.TileLayerIndices.Principal);
            }

            transform.position = ChunkManager.ToWorldPosition(Carver.Position);

            // draw trail
            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, transform.position);
        }
        else
        {
            Debug.Log("Carver is dead!");
            _sr.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }


}
