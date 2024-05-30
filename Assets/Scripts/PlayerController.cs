using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerController : MonoBehaviour
{
    [Header("Character Controller Settings")]
    private bool _b;

    public float castsize = 0.1f;

    private Camera _mainCamera;
    private Queue<GameObject> _path = new Queue<GameObject>();
    private bool _timeToMove = false;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void ApplyMovement()
    {
        if (!_timeToMove) return;
        Invoke(nameof(Move),0.5f);
        _timeToMove = false;
    }

    private void Move()
    {
        Vector3 targetPosition = _path.Peek().transform.position;
        transform.position = targetPosition;
        _path.Dequeue();
        if (_path.Count > 0)
        {
            Invoke(nameof(Move),0.5f);
        }
    }

    private void Update()
    {
        ProcessInputs();
        ApplyMovement();
    }

    private void ProcessInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 point = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0f;
            Debug.Log(point);
            if (CheckValidTile(point))
            {
                _path = FindPathToPoint(transform.position, point);
                if (_path == null)
                {
                    Debug.Log("Failed to find valid path to point");
                }
                else
                {
                    Debug.Log("Found valid path moving now");
                    _timeToMove = true;
                }
            }
            else
            {
                Debug.Log("Invalid Selection");
            }
        }
    }

    private Queue<GameObject> FindPathToPoint(Vector3 start, Vector3 end)
    {
        Queue<GameObject> path = new Queue<GameObject>();

        Vector3 currentPosition = start;
        while (Vector3.Distance(currentPosition, end) > 0.1f)
        {
            Vector3 direction = (end - currentPosition).normalized;
            Vector3 nextPosition = currentPosition + direction * 0.32f; // Move one unit towards the target

            RaycastHit2D hit = Physics2D.CircleCast(nextPosition, castsize, new Vector2(castsize,0),castsize);
            GameObject checking = hit.collider.gameObject;
            if (checking != null)
            {
                switch (checking.tag)
                {
                    case "WalkableTile":
                        path.Enqueue(checking);
                        currentPosition = nextPosition;
                        break;
                    default:
                        return null;
                }
            }
        }
        return path;
    }

    private bool CheckValidTile(Vector3 point)
    {
        RaycastHit2D hit = Physics2D.CircleCast(point, castsize, new Vector2(castsize,0),castsize);
        if (hit.collider == null) return false;
        GameObject clickedOn = hit.collider.gameObject;
        if (clickedOn == null) return false;
        switch (clickedOn.tag)
        {
            case "WalkableTile":
                return true;
        }
        return false;
    }
}
