using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private FloorStatScriptableObject floorState;

    // Checks when the player walks over a LevelEnd tile, increments the floorState scriptable object and calls the public function in the floor handler
    // to change to next level
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WalkableTile"))
            Physics2D.IgnoreCollision(collision.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("FloorSpawnPoint").GetComponent<FloorHandler>().NextFloor();
            floorState.SetMoves(0);
            floorState.IncreaseFloor();
            floorState.AddScore(50);
        }

    }
}
