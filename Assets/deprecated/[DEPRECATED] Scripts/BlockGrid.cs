using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockGrid : MonoBehaviour
{
    public bool InEditMode;

    [field: SerializeField] public List<GameObject> BlockInventory { get; private set; }
    private Dictionary<Vector2Int, GameObject> _instantiatedBlocks = new Dictionary<Vector2Int, GameObject>(); // TODO: Save state when unity is quit

    private void Start()
    {
        InEditMode = false;
    }

    public bool PlaceBlock(Vector2Int coord, GameObject block)
    {
        if (coord == null || block == null)
        {
            Debug.Log("null argument");
            return false;
        }

        if (!_instantiatedBlocks.ContainsKey(coord))
        {
            _instantiatedBlocks[coord] = Instantiate(block, ((Vector3Int)coord), Quaternion.identity, this.transform);
            return true;
        }
        return false;
        
    }

    public void ForcePlaceBlock(Vector2Int coord, GameObject block)
    {
        if (coord == null || block == null)
        {
            Debug.Log("null argument");
            return;
        }

        DestroyBlock(coord);
        PlaceBlock(coord, block);
    }

    public bool DestroyBlock(Vector2Int coord)
    {
        if (coord == null)
        {
            Debug.Log("null argument");
            return false;
        }

        GameObject block;
        if (_instantiatedBlocks.TryGetValue(coord, out block))
        {
            _instantiatedBlocks.Remove(coord);

            if (InEditMode)
            {
                DestroyImmediate(block);
            }
            else
            {
                Destroy(block);
            }
            return true;
        }
        return false;
    }

    public GameObject GetBlock(Vector2Int coord)
    {
        if (coord == null)
        {
            Debug.Log("null argument");
            return null;
        }

        GameObject block;
        _instantiatedBlocks.TryGetValue(coord, out block);
        return block;
    }

    public void DestroyAllBlocks()
    {
        Debug.Log(_instantiatedBlocks.Count);

        //while (transform.childCount > 0)
        //{
        //    DestroyImmediate(transform.GetChild(0).gameObject);
        //}

        int i = 0;
        GameObject[] blocksToRemove = new GameObject[_instantiatedBlocks.Count];

        foreach (GameObject block in _instantiatedBlocks.Values)
        {
            blocksToRemove[i] = block;
            i++;
        }

        _instantiatedBlocks.Clear();

        for (int j = 0; j < i; j++)
        {
            if (InEditMode)
            {
                DestroyImmediate(blocksToRemove[j]);
            }
            else
            {
                Destroy(blocksToRemove[j]);
            }
        }


        // Debugging
        if (transform.childCount > 0)
        {
            Debug.Log("Blocks not captured in dict");
        }
    }

}
