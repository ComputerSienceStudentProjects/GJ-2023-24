using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private FloorStatScriptableObject floorState;
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision");
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
