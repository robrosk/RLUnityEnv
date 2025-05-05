using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryWall : MonoBehaviour
{
    private void Awake()
    {
        // Ensure the GameObject has the "Wall" tag
        if (gameObject.tag != "Wall")
        {
            gameObject.tag = "Wall";
            Debug.Log($"Set tag to 'Wall' for {gameObject.name}");
        }
        
        // Ensure there's a collider attached
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"No collider found on {gameObject.name}. Adding BoxCollider.");
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }
    }
} 