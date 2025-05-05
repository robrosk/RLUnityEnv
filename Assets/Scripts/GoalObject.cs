using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObject : MonoBehaviour
{
    private void Awake()
    {
        // Ensure the GameObject has the "Goal" tag
        if (gameObject.tag != "Goal")
        {
            gameObject.tag = "Goal";
            Debug.Log($"Set tag to 'Goal' for {gameObject.name}");
        }
        
        // Ensure there's a trigger collider attached
        if (GetComponent<Collider>() == null)
        {
            Debug.LogWarning($"No collider found on {gameObject.name}. Adding SphereCollider.");
            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 1f;
        }
        else if (!GetComponent<Collider>().isTrigger)
        {
            GetComponent<Collider>().isTrigger = true;
            Debug.Log($"Set collider to trigger for {gameObject.name}");
        }
    }
} 