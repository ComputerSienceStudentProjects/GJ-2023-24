using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Character Controller Settings")]
    [SerializeField] private float castSize = 0.1f;
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private LayerMask walkableLayer;
    [SerializeField] private FloorStatScriptableObject floorStatScriptableObject;
    
    private GameObject _boxBeingHeld;
    private Camera _mainCamera;
    private Queue<GameObject> _path = new ();
    private bool _timeToMove;
    
    
    private void Awake()
    {
        // In the awake we get all the references that we will be using on the script, allowing us
        // to only find the reference once at the start, instead of everytime the reference is needed
        _mainCamera = Camera.main;
    }

    private void ApplyMovement()
    {
        // We make sure, we are clear to make a movement (Input handler, captured a move request from the user,
        // calculated if it's a valid move, and the path it needs to transverse) then we check if we need to also move
        // any box that is in front of the player, we invoke the main Movement loop, and disable the flag that 
        // tells us to start a movement.
        if (!_timeToMove) return;
        CheckIfNeedsToMoveBox(); // this call doesn't return any value, since we are only setting the box parent
        floorStatScriptableObject.AddMoves(1); // We add a movement to the floor stats
        Invoke(nameof(Move),0.5f);
        _timeToMove = false;
    }

    private void CheckIfNeedsToMoveBox()
    {
        // Checks if we have a box in front of the player before it starts their movement, allowing us 
        // to parent the box to our character, making so that we can use the path queue, in the box as well,
        // without needing to recalculate the final point where the box should end up
        //
        // If we don't have a box in out way, we make sure we un-parent any leftover box from any previous movement
        // In order to not move the box if we don't have it "selected"
        RaycastHit2D hit = Physics2D.Raycast(transform.position,
            (_path.Peek().transform.position - transform.position).normalized,0.32f,boxLayer);
        if (hit.collider is { gameObject: { tag: "MovableBlock" } })
        {
            _boxBeingHeld = hit.collider.gameObject;
            _boxBeingHeld.transform.parent = transform;
        }
        else if (_boxBeingHeld != null) _boxBeingHeld.transform.parent = null;
    }

    private void Move()
    {
        // This is the main Movement loop, apply a movement from the queue, and then it invokes itself
        // after a given delay, to make the movement not immediate to the destination, but show all the tiles
        // the character had to transverse to reach the destination
        //
        // We Dequeue a tile from the queue, and then move the player to that position
        Vector3 targetPosition = _path.Dequeue().transform.position;
        transform.position = targetPosition;
        if (_path.Count > 0)
        {
            //if we still have tiles to transverse, we invoke Move again until we transversed all tiles
            Invoke(nameof(Move),0.5f);
        }
    }

    private void Update()
    {
        // In oder to make maintaining the code easier, we decided on separating each action this 
        // script needs to process separately, and we can also control the order on which action is called
        // before, allowing more vertical control over the behaviour of the script
        ProcessInputs();
        ApplyMovement();
    }

    private void ProcessInputs()
    {
        // Only listen to Left Mouse Input, since UI is handled Seperated
        if (!Input.GetMouseButtonDown(0)) return;
        if (_path.Count > 0) return;
        // Gets the corresponding point in the world the user clicked on and then scales with a vector (1,1,0)
        // in order to keep the z coordinate 0
        Vector3 point = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        point.Scale(new Vector3(1,1,0f));
        // Checks if it's a valid tile that the player can move to
        if (!CheckValidTile(point)) return;
        // Obtains the path the player character needs to perform to go to desired location
        _path = FindPathToPoint(transform.position, point);
        // Since the path can have obstacles, if we get a null path, we know we can't move
        // to the request tile
        if (_path != null) _timeToMove = true;
    }

    private Queue<GameObject> FindPathToPoint(Vector3 start, Vector3 end)
    {
        // This works in a semi-recursive way, from a starting point the game will check if the player
        // can move forwards, if so adds said tile to the queue.
        // We initialize a Queue here, in order to avoid problems, where the game tries to access
        // We use a queue, since it has access policy of first in first out, allowing us to keep queuing all
        // tiles before executing all movements
        // the queue before its initialized completely
        Queue<GameObject> path = new Queue<GameObject>();
        Vector3 currentPosition = start;
        while (Vector3.Distance(currentPosition, end) > 0.1f)
        {
            // Calculates the next position Coordinates, by using the current position
            // plus the direction of the movement multiplied by the grid size, in this case since each block is at
            // 0.32 units from each other, we multiply by that
            Vector3 nextPosition = currentPosition + (end - currentPosition).normalized * 0.32f;
            // After calculating the next position Vector, we can now check if we got a walkable tile, first we obtain
            // the game object that we found at that point, and then we check if it's a walkable tile, if we found
            // a tile in the path that is not walkable, we should return null, so that the method responsible
            // for applying the movements knows we couldn't find a valid path to the destination
            GameObject checking = GetObjectAtPoint(nextPosition, walkableLayer);
            if (!IsWalkable(checking)) return null;
            // if all checks pass, it means the tile is valid, and we can put it int the queue of movements to be 
            // performed by the character
            path.Enqueue(checking);
            currentPosition = nextPosition;
        }
        //after this is all done we return the path we calculated
        return path;
    }

    private bool CheckValidTile(Vector3 point)
    {
        // We only consider valid destination tiles if the tile is on the same horizontal position, or vertical
        // position, anything else is considered a non-valid tile, that accounts for diagonal movements
        // we can only move to Walkable Tiles, so after we confirm it's not an invalid movement, we can
        // check if the destination tile is Walkable, and if so we return true
        return !IsDiagonalMove(point) && IsWalkable(GetObjectAtPoint(point, walkableLayer));
    }

    private bool IsDiagonalMove(Vector3 point)
    {
        // This checks that the target point can only be either on the same x position, given an offset or the
        // same position on the y, given an offset, this ensures that regardless of the movement, the player
        // can only move horizontal or vertically from their current position
        if (!(point.y - transform.position.y >= 0.16) && !(point.y - transform.position.y <= -0.16f)) return false;
        return point.x - transform.position.x >= 0.16 || point.x - transform.position.x <= -0.16;
    }

    private GameObject GetObjectAtPoint(Vector3 origin, LayerMask layerMask)
    {
        // This performs a CircleCast on a given origin point, and a reference LayerMask to be tested for, it checks 
        // if the collider of the resulting RaycastHit2D is not null, and if so returns the GameObject 
        // associated with the collider found in the RaycastHit2D
        RaycastHit2D hit = Physics2D.CircleCast
            (origin, castSize, new Vector2(castSize,0),castSize,layerMask);
        return hit.collider == null ? null : hit.collider.gameObject;
    }

    private bool IsWalkable(GameObject tile)
    {
        // Makes sure that for any given GameObject we return true if given GameObject isn't null and if the GameObject
        // set tag is "WalkableTile"
        return tile is { gameObject:{tag:"WalkableTile"}};
    }
}
