using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Point
{
    public readonly int x, y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override int GetHashCode()
    {
        return x.GetHashCode() + y.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        Point other = (Point)obj;
        return (other.x == x && other.y == y);
    }

    public override string ToString()
    {
        return $"({x}, {y})";
    }
}


public class TileMeshGenerator : MonoBehaviour
{
    //public enum MeshMode
    //{
    //    Conjoined,
    //    Disjoint
    //}

    //public MeshMode meshMode;

    private static int _length = ChunkManager.ChunkSize.x;
    private static int _height = ChunkManager.ChunkSize.y;

    public static MeshData GenerateTileMesh(SpatialArray<Tiles> chunkTileMap)
    {
        // Generates a conjoined mesh, i.e. no overlapping vertices
        // Count number of blocks
        int nBlocks = 0;
        for (int t = 0; t < _height; t++)
        {
            for (int s = 0; s < _length; s++)
            {
                if (chunkTileMap.Get(s, t) != 0)
                {
                    nBlocks++;
                }
            }
        }

        var meshVertexIndices = new Dictionary<Vector2, int>();

        // Row by row
        int index = 0;
        for (int t = 0; t < _height; t++)
        {
            for (int s = 0; s < _length; s++)
            {
                if (chunkTileMap.Get(s, t) != 0)
                {
                    var x = ChunkManager.ToWorldPosition(s);
                    var y = ChunkManager.ToWorldPosition(t);

                    // creating vertices 
                    if (meshVertexIndices.TryAdd(new Vector2(x, y + ChunkManager.ToWorldPosition(1)), index))  // (spatial) tl
                    {

                        index++;
                    }
                    if (meshVertexIndices.TryAdd(new Vector2(x + ChunkManager.ToWorldPosition(1), y + ChunkManager.ToWorldPosition(1)), index))  // (spatial) tr
                    {

                        index++;
                    }
                    if (meshVertexIndices.TryAdd(new Vector2(x + ChunkManager.ToWorldPosition(1), y), index))  // (spatial) br
                    {

                        index++;
                    }
                    if (meshVertexIndices.TryAdd(new Vector2(x, y), index))  // (spatial) bl
                    {

                        index++;
                    }

                }
            }
        }

        MeshData meshData = new MeshData(meshVertexIndices.Count, nBlocks);

        // Setting vertices and uvs
        Vector3 chunkLocalOffset = ChunkManager.ToWorldPosition(new Vector3(-ChunkManager.ChunkSize.x / 2f, -ChunkManager.ChunkSize.y / 2f));  // bottom left of chunk
        foreach (KeyValuePair<Vector2, int> entry in meshVertexIndices)
        {
            var vertexWorldSpacePos = entry.Key;
            var vertexIndex = entry.Value;
            meshData.SetVertexAtIndex(chunkLocalOffset + new Vector3(vertexWorldSpacePos.x, vertexWorldSpacePos.y), vertexIndex);

            // uv
            Vector2 uvCoord = new Vector2((vertexWorldSpacePos.x) / (float)(ChunkManager.ChunkSize.x * ChunkManager.TileSize),
                (vertexWorldSpacePos.y) / (float)(ChunkManager.ChunkSize.y * ChunkManager.TileSize));
            meshData.SetUVAtIndex(uvCoord, vertexIndex);
        }

        // Triangles
        for (int t = 0; t < _height; t++)
        {
            for (int s = 0; s < _length; s++)
            {
                if (chunkTileMap.Get(s, t) != 0)
                {
                    var x = ChunkManager.ToWorldPosition(s);
                    var y = ChunkManager.ToWorldPosition(t);

                    int topLeftVertexIndex = meshVertexIndices[new Vector2(x, y + ChunkManager.ToWorldPosition(1))];
                    int topRightVertexIndex = meshVertexIndices[new Vector2(x + ChunkManager.ToWorldPosition(1), y + ChunkManager.ToWorldPosition(1))];
                    int bottomRightVertexIndex = meshVertexIndices[new Vector2(x + ChunkManager.ToWorldPosition(1), y)];
                    int bottomLeftVertexIndex = meshVertexIndices[new Vector2(x, y)];

                    // clockwise winding ... confusing! We have two different coordinate systems here...
                    // The reason the winding order is clockwise (which is correct!) in this case is because
                    // the grid-space y-values are correctly mapped to world-space y values (a sign flip)
                    // unlike in the Procedural Generation tutorials
                    meshData.AddTriangle(topLeftVertexIndex, topRightVertexIndex, bottomRightVertexIndex);
                    meshData.AddTriangle(topLeftVertexIndex, bottomRightVertexIndex, bottomLeftVertexIndex);
                }
            }
        }

        return meshData;
    }

    /*public static MeshData GenerateTileMesh(SpatialArray<Tiles> chunkTileMap)
    {
        // Generates a conjoined mesh, i.e. no overlapping vertices
        // Count number of blocks
        int nBlocks = 0;
        for (int t = 0; t < _height; t++)
        {
            for (int s = 0; s < _length; s++)
            {
                if (chunkTileMap.Get(s, t) != 0)
                {
                    nBlocks++;
                }
            }
        }

        var meshVertexIndices = new Dictionary<Vector2, int>();

        // Row by row
        int index = 0;
        for (int t = 0; t < _height; t++)
        {
            for (int s = 0; s < _length; s++)
            {
                if (chunkTileMap.Get(s, t) != 0)
                {
                    var x = s * ChunkManager.TileSize;
                    var y = t * ChunkManager.TileSize;

                    // creating vertices 
                    if (meshVertexIndices.TryAdd(new Vector2(x - ChunkManager.TileSize/2f, y + ChunkManager.TileSize/2f), index))  // (spatial) tl
                    {

                        index++;
                    }
                    if (meshVertexIndices.TryAdd(new Vector2(x + ChunkManager.TileSize/2f, y + ChunkManager.TileSize/2f), index))  // (spatial) tr
                    {

                        index++;
                    }
                    if (meshVertexIndices.TryAdd(new Vector2(x + ChunkManager.TileSize/2f, y - ChunkManager.TileSize/2f), index))  // (spatial) br
                    {

                        index++;
                    }
                    if (meshVertexIndices.TryAdd(new Vector2(x - ChunkManager.TileSize/2f, y - ChunkManager.TileSize/2f), index))  // (spatial) bl
                    {

                        index++;
                    }

                }
            }
        }

        MeshData meshData = new MeshData(meshVertexIndices.Count, nBlocks);

        // Setting vertices and uvs
        foreach (KeyValuePair<Vector2, int> entry in meshVertexIndices)
        {
            var vertexWorldSpacePos = entry.Key;  
            var vertexIndex = entry.Value;
            Vector3 chunkLocalOffset = ChunkManager.TileSize * new Vector3(-ChunkManager.ChunkSize.x / 2f, -ChunkManager.ChunkSize.y / 2f);  // bottom left of chunk
            meshData.SetVertexAtIndex(chunkLocalOffset + new Vector3(vertexWorldSpacePos.x, vertexWorldSpacePos.y), vertexIndex);

            // uv
            Vector2 uvCoord = new Vector2((vertexWorldSpacePos.x + ChunkManager.TileSize / 2f) / (float)(ChunkManager.ChunkSize.x * ChunkManager.TileSize), 
                (vertexWorldSpacePos.y + ChunkManager.TileSize / 2f) / (float)(ChunkManager.ChunkSize.y * ChunkManager.TileSize));
            meshData.SetUVAtIndex(uvCoord, vertexIndex);
        }

        // Triangles
        for (int t = 0; t < _height; t++)
        {
            for (int s = 0; s < _length; s++)
            {
                if (chunkTileMap.Get(s, t) != 0)
                {
                    var x = s * ChunkManager.TileSize;
                    var y = t * ChunkManager.TileSize;

                    int topLeftVertexIndex = meshVertexIndices[new Vector2(x - ChunkManager.TileSize/2f, y + ChunkManager.TileSize/2f)];
                    int topRightVertexIndex = meshVertexIndices[new Vector2(x + ChunkManager.TileSize/2f, y + ChunkManager.TileSize/2f)];
                    int bottomRightVertexIndex = meshVertexIndices[new Vector2(x + ChunkManager.TileSize/2f, y - ChunkManager.TileSize/2f)];
                    int bottomLeftVertexIndex = meshVertexIndices[new Vector2(x - ChunkManager.TileSize/2f, y - ChunkManager.TileSize/2f)];

                    // clockwise winding ... confusing! We have two different coordinate systems here...
                    // The reason the winding order is clockwise (which is correct!) in this case is because
                    // the grid-space y-values are correctly mapped to world-space y values (a sign flip)
                    // unlike in the Procedural Generation tutorials
                    meshData.AddTriangle(topLeftVertexIndex, topRightVertexIndex, bottomRightVertexIndex);
                    meshData.AddTriangle(topLeftVertexIndex, bottomRightVertexIndex, bottomLeftVertexIndex);
                }
            }
        }

        return meshData;
    }*/

    //public static MeshData GenerateTileMesh(SpatialArray<Tiles> chunkTileMap)
    //{
    //    // Generates a conjoined mesh, i.e. no overlapping vertices
    //    // Count number of blocks
    //    int nBlocks = 0;
    //    for (int y = 0; y < _height; y++)
    //    {
    //        for (int x = 0; x < _length; x++)
    //        {
    //            if (chunkTileMap.Get(x, y) != 0)
    //            {
    //                nBlocks++;
    //            }
    //        }
    //    }

    //    var meshVertexIndices = new Dictionary<Vector2, int>();

    //    // Row by row
    //    int index = 0;
    //    for (int y = 0; y < _height; y++)
    //    {
    //        for (int x = 0; x < _length; x++)
    //        {
    //            if (chunkTileMap.Get(x, y) != 0)
    //            {

    //                // creating vertices 
    //                if (meshVertexIndices.TryAdd(new Vector2(2 * x - 1, 2 * y + 1), index))  // (spatial) tl
    //                {

    //                    index++;
    //                }
    //                if (meshVertexIndices.TryAdd(new Vector2(2 * x + 1, 2 * y + 1), index))  // (spatial) tr
    //                {

    //                    index++;
    //                }
    //                if (meshVertexIndices.TryAdd(new Vector2(2 * x + 1, 2 * y - 1), index))  // (spatial) br
    //                {

    //                    index++;
    //                }
    //                if (meshVertexIndices.TryAdd(new Vector2(2 * x - 1, 2 * y - 1), index))  // (spatial) bl
    //                {

    //                    index++;
    //                }
                    
    //            }
    //        }
    //    }

    //    MeshData meshData = new MeshData(meshVertexIndices.Count, nBlocks);

    //    // Setting vertices and uvs
    //    foreach (KeyValuePair<Vector2, int> entry in meshVertexIndices)
    //    {
    //        var pt = entry.Key;  // pt is a factor of 2 larger than its world-space equivalent
    //        int vertexIndex = entry.Value;
    //        Vector3 offset = new Vector3(-ChunkManager.ChunkSize.x / 2f, -ChunkManager.ChunkSize.y / 2f);  // bottom left of chunk
    //        meshData.SetVertexAtIndex(offset + new Vector3(pt.x / 2f, pt.y / 2f), vertexIndex);

    //        // uv
    //        Vector2 uvCoord = new Vector2((pt.x + 1)/(2f * ChunkManager.ChunkSize.x), (pt.y + 1)/(2f * ChunkManager.ChunkSize.y));
    //        meshData.SetUVAtIndex(uvCoord, vertexIndex);
    //    }

    //    // Triangles
    //    for (int y = 0; y < _height; y++)
    //    {
    //        for (int x = 0; x < _length; x++)
    //        {
    //            if (chunkTileMap.Get(x, y) != 0)
    //            {
    //                int topLeftVertexIndex = meshVertexIndices[new Vector2(2 * x - 1, 2 * y + 1)];
    //                int topRightVertexIndex = meshVertexIndices[new Vector2(2 * x + 1, 2 * y + 1)];
    //                int bottomRightVertexIndex = meshVertexIndices[new Vector2(2 * x + 1, 2 * y - 1)];
    //                int bottomLeftVertexIndex = meshVertexIndices[new Vector2(2 * x - 1, 2 * y - 1)];

    //                // clockwise winding ... confusing! We have two different coordinate systems here...
    //                // The reason the winding order is clockwise (which is correct!) in this case is because
    //                // the grid-space y-values are correctly mapped to world-space y values (a sign flip)
    //                // unlike in the Procedural Generation tutorials
    //                meshData.AddTriangle(topLeftVertexIndex, topRightVertexIndex, bottomRightVertexIndex);
    //                meshData.AddTriangle(topLeftVertexIndex, bottomRightVertexIndex, bottomLeftVertexIndex);
    //            }
    //        }
    //    }

    //    return meshData;
    //}

}

public class MeshData
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;

    private int triangleIndex = 0;

    public MeshData(int nVertices, int nBlocks)
    {
        vertices = new Vector3[nVertices];
        uvs = new Vector2[nVertices];
        triangles = new int[nBlocks * 2 * 3];
    }

    public void SetVertexAtIndex(Vector3 vertex, int index)
    {
        vertices[index] = vertex;
    }

    public void SetUVAtIndex(Vector2 uv, int index)
    {
        uvs[index] = uv;
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;

        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }
}

