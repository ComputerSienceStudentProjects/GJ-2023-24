using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other is { gameObject: { tag: "MovableBlock" } })
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
