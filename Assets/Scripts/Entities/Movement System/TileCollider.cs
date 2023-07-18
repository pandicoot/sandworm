using UnityEngine;

public class TileCollider : MonoBehaviour
{
    private SpatialArray<Tiles> _map;
    public const float CollisionBuffer = 0.01f;
    public bool HasReceivedMap { get; private set; }

    [field: SerializeField] public int StepHeight { get; set; }

    private void Awake()
    {
        HasReceivedMap = false;
    }

    private void AssignMap(SpatialArray<Tiles> map)
    {
        _map = map;
        HasReceivedMap = true;
    }

    private void OnEnable()
    {
        GeneratorManager.WorldLoaded += AssignMap;
    }

    private void OnDisable()
    {
        GeneratorManager.WorldLoaded -= AssignMap;
    }

    /// <summary>
    /// Gets the next valid position in the environment given the target position, current velocity and size of the object.
    /// May adjust the velocity of the object depending on whether a collision occurs.
    /// </summary>
    /// <param name="targetPos">The position that would be reached assuming collisions were disabled.</param>
    /// <param name="oPos">The original position of the object.</param>
    /// <param name="velocity">The current velocity of the object.</param>
    /// <param name="extents">The extents of the object's movement bounding box.</param>
    /// <param name="canStep">Whether the object can step up/down pixels.</param>
    /// <returns>The actual position that the object reaches, accounting for collisions with the environment.</returns>
    public Vector2 GetPositionAccessible(Vector2 targetPos, Vector2 oPos, ref Vector2 velocity, Vector2 extents, bool canStep)
    {
        if (!HasReceivedMap)
        {
            return oPos;
        }

        //Vector2 fPos = new Vector2(oPos.x, oPos.y);
        var fPos = oPos;
        //bool tryStep = false;
        fPos += StepInAxis(true, oPos.x, oPos.y, targetPos.x, ref velocity.x, extents.x, extents.y, canStep);  // decouple to work with collider instead
        targetPos.y += fPos.y - oPos.y;
        //Debug.Log($"oPosY pre Y: {oPos.y}, feetYMap: {ChunkManager.ToMapPosition(oPos.y - extents.y)}");
        fPos += StepInAxis(false, fPos.y, fPos.x, targetPos.y, ref velocity.y, extents.y, extents.x, canStep);
        //Debug.Log($"fPosY post Y: {fPos.y}, feetYMap: {ChunkManager.ToMapPosition(fPos.y - extents.y)}");
        return fPos;
    }

    /// <summary>
    /// Calculates the resulting change in the y-coordinate due to stepping in the direction of movement.
    /// Stepping means stepping up pixel(s).
    /// </summary>
    /// <param name="feetYMap">The position of the object's base, scaled to map-space.</param>
    /// <param name="headYMap">The position of the object's top, scaled to map-space.</param>
    /// <param name="currTileXMap">The block x-coordinate of the currently occupied column of tiles.</param>
    /// <param name="targetTileXMap">The x-coordinate of the column of tiles desired to be reached.</param>
    /// <param name="dir">The direction of movement. 1 is rightwards or stationary, -1 is leftwards.</param>
    /// <returns>The resulting change in the y-coordinate due to any steps capable of being performed.</returns>
    private int TryStep(int feetYMap, int headYMap, int currTileXMap, int targetTileXMap, int dir)
    {
        int heightInMapUnits = headYMap - feetYMap + 1;
        int steppedHeight = 0;

        // Scan through each column of tiles starting at the currently occupied,
        // in the direction of movement until the desired position, and step if possible.
        for (int xMap = currTileXMap; Mathf.Abs(xMap - currTileXMap) <= Mathf.Abs(targetTileXMap - currTileXMap); xMap += dir)
        {
            if (_map.Get(xMap, feetYMap + steppedHeight) == 0)  
            {
                continue;
            }

            int heightToStepTo = 0;
            bool primed = true;  // not sure what this is for?
            // Determine prospective height to step to.
            // Scan up from base tile height (+ 1 because...a step should already be made?)
            // up to total steppable height and take maximum valid step-up height.
            for (int yMap = feetYMap + 1; yMap - (feetYMap + 1) < StepHeight; yMap++)
            {
                if (primed && _map.Get(xMap, yMap) == 0)
                {
                    heightToStepTo = yMap - feetYMap;
                    primed = false;
                    continue;
                }

                if (_map.Get(xMap, yMap) != 0)
                {
                    primed = true;
                }

            }

            // step validity check: check whether character can fit (by vertical extents)
            // TODO: CHECK at PREV X value the same way
            for (int yMap = feetYMap + heightToStepTo; yMap - (feetYMap + heightToStepTo) < heightInMapUnits; yMap++)
            {
                if (_map.Get(xMap, yMap) != 0)
                {
                    return steppedHeight;
                }
            }

            steppedHeight += heightToStepTo;
        }
        return steppedHeight;
    }

    private Vector2 StepInAxis(bool isX, float oCoord, float oCoordOrthogonal, float targetCoord, ref float vel, float extent, float extentOrthogonal, bool canStep)
    {
        var oCoordMap = ChunkManager.ToMapPosition(oCoord);
        var oCoordOrthogonalMap = ChunkManager.ToMapPosition(oCoordOrthogonal);
        var extentMap = ChunkManager.ToMapPosition(extent);
        var extentOrthogonalMap = ChunkManager.ToMapPosition(extentOrthogonal);
        var velMap = ChunkManager.ToMapPosition(vel);

        float delta, deltaOrthogonal = 0f;
        int dir = (vel >= 0f) ? 1 : -1;
        //if (!isX) Debug.Log($"dir: {dir}; vel: {vel}");
        //int contactIdx = GetContactIndex(isX, dir);
        //if (contactIdx != -1)
        //{
        //    if (_contacts[contactIdx])
        //    {
        //        vel = 0;
        //        return oCoord;
        //    }
        //}
        //else
        //{
        //    Debug.Log("stationary");
        //    vel = 0;
        //    return oCoord;
        //} 

        float targetFwdEdgeCoordMap = oCoordMap + dir * extentMap + velMap * Time.fixedDeltaTime;

        

        int minPosMap = Mathf.RoundToInt(oCoordOrthogonalMap - extentOrthogonalMap);
        int maxPosMap = Mathf.RoundToInt(oCoordOrthogonalMap + extentOrthogonalMap);

        int currTileCoord = Mathf.RoundToInt(oCoordMap);
        int furthestTileCoord = Mathf.RoundToInt(targetFwdEdgeCoordMap);

        //if (isX)
        //{
            //Debug.Log($"YExtent(M): {extentOrthogonalMap}");
            //Debug.Log($"YCoordCentre(M): {oCoordOrthogonalMap}");
            //Debug.Log($"OCoord(M): {oCoordMap}; feet(M): {minPosMap}; head(M): {maxPosMap}");
            //Debug.Log($"CurrTileCoord(M): {currTileCoord}; furthestTileCoord(M): {furthestTileCoord}");
        //}
        // stepping logic
        if (isX && canStep) {
            int yUp = TryStep(minPosMap, maxPosMap, currTileCoord, furthestTileCoord, dir);
            if (yUp > 0)
            //Debug.Log($"stepped up {yUp}!");
            minPosMap += yUp;
            maxPosMap += yUp;
            deltaOrthogonal = ChunkManager.ToWorldPosition(yUp);
        }

        int ortho, para;

        //if (ebug)
        //{
        //    Debug.Log($"targetfwdedgeMap: {targetFwdEdgeCoordMap}, currtileCoord: {currTileCoord}, furthestTileCoord: {furthestTileCoord}"); ;
        //}

        // get closest obstacle
        int closestObstacleCoord = int.MinValue / 2;
        for (ortho = minPosMap; ortho <= maxPosMap; ortho++)
        {
            for (para = currTileCoord; Mathf.Abs(para - currTileCoord) < Mathf.Abs(furthestTileCoord - currTileCoord) + 1; para += dir)
            {
                Tiles t = isX ? _map.Get(para, ortho) : _map.Get(ortho, para);

                if (t != Tiles.Air)
                {
                    if (Mathf.Abs(para - currTileCoord) <= Mathf.Abs(closestObstacleCoord - currTileCoord))
                    {
                        closestObstacleCoord = para;
                    }
                }
            }
        }

        float targetDist = Mathf.Max(0, Mathf.Abs(targetCoord - oCoord));
        float colliderDist;

        //int oppContactIdx = GetContactIndex(isX, -dir);
        //_contacts[contactIdx] = false;
        //_contacts[oppContactIdx] = false;
        if (closestObstacleCoord < -1)
        {
            colliderDist = targetDist;
        }
        else if (closestObstacleCoord == currTileCoord)  // in block
        {
            colliderDist = 0;
            //for (int i = 0; i < 4; i++)
            //{
            //    _contacts[i] = true;
            //}
        }
        else  // collided
        {
            if (dir > 0)
            {
                colliderDist = ChunkManager.ToWorldPosition((closestObstacleCoord - 0.5f - extentMap - CollisionBuffer)) - oCoord;
            }
            else
            {
                colliderDist = oCoord - ChunkManager.ToWorldPosition(closestObstacleCoord + 0.5f + extentMap + CollisionBuffer);
            }
            vel = 0;

            //_contacts[contactIdx] = true;
        }

        //if (ebug)
        //{
        //    Debug.Log($"targetdist: {targetDist}, colliderdist: {colliderDist}"); ;
        //}

        float dist = Mathf.Min(targetDist, colliderDist);
        delta = dir * dist;
        //if (!isX) Debug.Log(delta);
        return (isX) ? new Vector2(delta, deltaOrthogonal) : new Vector2(deltaOrthogonal, delta);
    }
}
