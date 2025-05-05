using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour
{
    // This method will be called automatically when Unity starts
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void InitializeScene()
    {
        Debug.Log("Scene Initializer: Auto-generating environment...");
        
        // Check if a SceneBuilder_Fixed already exists (use the fixed version)
        SceneBuilder_Fixed existingBuilder = Object.FindObjectOfType<SceneBuilder_Fixed>();
        
        if (existingBuilder != null)
        {
            Debug.Log("Scene Initializer: Found existing SceneBuilder_Fixed, using it.");
            existingBuilder.CreateBasicScene();
        }
        else
        {
            Debug.Log("Scene Initializer: Creating new SceneBuilder_Fixed.");
            // Create a new GameObject with SceneBuilder_Fixed
            GameObject sceneBuilderObj = new GameObject("SceneBuilder_Fixed");
            SceneBuilder_Fixed builder = sceneBuilderObj.AddComponent<SceneBuilder_Fixed>();
            
            // Generate the scene
            builder.CreateBasicScene();
        }
    }
} 