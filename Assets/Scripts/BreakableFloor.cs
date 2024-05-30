using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BreakableFloor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Breakable floors, will cause the player to fall, and therefore lose the game.
        // It will have the same effect on the MovableBoxes
        if (other is { gameObject: { tag: "MovableBlock" } } or { gameObject: { tag: "Player" }})
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
