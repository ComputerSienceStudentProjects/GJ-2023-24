using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleaner : MonoBehaviour
{
    // Simple box collider destroys floor triggering the spawn of the next floor.
    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(col.gameObject);
    }
}
