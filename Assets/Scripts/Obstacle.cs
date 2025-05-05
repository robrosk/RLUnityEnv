using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void Awake()
    {
        // Ensure the GameObject has the "Obstacle" tag
        if (gameObject.tag != "Obstacle")
        {
            gameObject.tag = "Obstacle";
            Debug.Log($"Set tag to 'Obstacle' for {gameObject.name}");
        }
        
        // Ensure there's a collider attached
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"No collider found on {gameObject.name}. Adding BoxCollider.");
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.isTrigger = true;
        }
        else if (!GetComponent<Collider>().isTrigger)
        {
            // Make sure it's a trigger collider
            GetComponent<Collider>().isTrigger = true;
            Debug.Log($"Set collider to trigger for {gameObject.name}");
        }
    }
} 