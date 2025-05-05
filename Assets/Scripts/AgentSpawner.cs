using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject agentPrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    
    [Header("References")]
    public Transform goalTransform;
    
    [Header("Debug")]
    public bool spawnOnStart = true;
    public bool clearAgentsOnSpawn = true;
    public bool onlySpawnIfNoAgents = true;
    
    private List<GameObject> spawnedAgents = new List<GameObject>();
    
    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnAgents();
        }
    }
    
    public void SpawnAgents()
    {
        // Check if agents already exist in the scene
        if (onlySpawnIfNoAgents)
        {
            AvoidAgent[] existingAgents = FindObjectsOfType<AvoidAgent>();
            if (existingAgents.Length > 0)
            {
                Debug.Log($"AgentSpawner: Found {existingAgents.Length} existing agents, not spawning more.");
                
                // Update existing agents with goal reference if needed
                foreach (AvoidAgent agent in existingAgents)
                {
                    if (agent.goalTransform == null && goalTransform != null)
                    {
                        agent.goalTransform = goalTransform;
                        Debug.Log("Updated existing agent with goal reference");
                    }
                }
                
                return;
            }
        }
        
        if (clearAgentsOnSpawn)
        {
            ClearAgents();
        }
        
        if (agentPrefab == null)
        {
            Debug.LogError("Agent prefab is not assigned!");
            return;
        }
        
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }
        
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint == null) continue;
            
            GameObject agent = Instantiate(agentPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Set up references
            AvoidAgent avoidAgent = agent.GetComponent<AvoidAgent>();
            if (avoidAgent != null && goalTransform != null)
            {
                avoidAgent.goalTransform = goalTransform;
            }
            
            spawnedAgents.Add(agent);
        }
        
        Debug.Log($"Spawned {spawnedAgents.Count} agents");
    }
    
    public void ClearAgents()
    {
        foreach (GameObject agent in spawnedAgents)
        {
            if (agent != null)
            {
                Destroy(agent);
            }
        }
        
        spawnedAgents.Clear();
    }
    
    // For use with UI buttons
    public void OnSpawnButtonPressed()
    {
        SpawnAgents();
    }
    
    public void OnClearButtonPressed()
    {
        ClearAgents();
    }
} 