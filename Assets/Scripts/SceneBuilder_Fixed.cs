using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Actuators;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneBuilder_Fixed : MonoBehaviour
{
    [Header("Scene Generation")]
    public bool createOnStart = false;
    public bool createNow = false;

    [Header("Prefabs")]
    public GameObject agentPrefab;

    private void Start()
    {
        if (createOnStart)
        {
            CreateBasicScene();
        }
    }

    private void Update()
    {
        if (createNow)
        {
            createNow = false;
            CreateBasicScene();
        }
    }

    public void CreateBasicScene()
    {
        Debug.Log("Creating basic scene...");
        
        // 1. Create Floor
        GameObject floor = CreateFloor();
        
        // 2. Create Walls
        GameObject boundaries = CreateBoundaries();
        
        // 3. Create Goal
        GameObject goal = CreateGoal();
        
        // 4. Create Agent Prefab (if it doesn't exist)
        GameObject agent = CreateAgentPrefab();
        
        // 5. Create Spawner
        GameObject spawner = CreateSpawner(agent, goal);
        
        Debug.Log("Basic scene created successfully!");
    }

    private GameObject CreateFloor()
    {
        GameObject floor = GameObject.Find("Floor");
        if (floor == null)
        {
            floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
            floor.name = "Floor";
            floor.transform.position = Vector3.zero;
            floor.transform.localScale = new Vector3(1, 1, 1);
            
            // Add material
            Renderer renderer = floor.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(0.7f, 0.7f, 0.7f);
            }
            
            // Add BoundaryWall script
            floor.AddComponent<BoundaryWall>();
        }
        
        return floor;
    }

    private GameObject CreateBoundaries()
    {
        GameObject boundariesParent = GameObject.Find("Boundaries");
        if (boundariesParent == null)
        {
            boundariesParent = new GameObject("Boundaries");
            
            // Create 4 walls
            CreateWall("Wall_North", new Vector3(0, 1.5f, 5), new Vector3(10, 3, 0.1f), boundariesParent.transform);
            CreateWall("Wall_South", new Vector3(0, 1.5f, -5), new Vector3(10, 3, 0.1f), boundariesParent.transform);
            CreateWall("Wall_East", new Vector3(5, 1.5f, 0), new Vector3(0.1f, 3, 10), boundariesParent.transform);
            CreateWall("Wall_West", new Vector3(-5, 1.5f, 0), new Vector3(0.1f, 3, 10), boundariesParent.transform);
            
            // Create ceiling
            CreateWall("Ceiling", new Vector3(0, 3, 0), new Vector3(10, 0.1f, 10), boundariesParent.transform);
        }
        
        return boundariesParent;
    }

    private GameObject CreateWall(string name, Vector3 position, Vector3 scale, Transform parent)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.transform.SetParent(parent);
        
        // Add BoundaryWall script
        wall.AddComponent<BoundaryWall>();
        
        // Set color
        Renderer renderer = wall.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = new Color(0.8f, 0.8f, 0.8f, 0.2f);
        }
        
        return wall;
    }

    private GameObject CreateGoal()
    {
        GameObject goal = GameObject.Find("Goal");
        if (goal == null)
        {
            goal = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            goal.name = "Goal";
            goal.transform.position = new Vector3(4, 0.5f, 4);
            goal.transform.localScale = new Vector3(1, 1, 1);
            
            // Add material
            Renderer renderer = goal.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = Color.green;
            }
            
            // Add GoalObject script
            goal.AddComponent<GoalObject>();
        }
        
        return goal;
    }

    private GameObject CreateAgentPrefab()
    {
        // If a prefab is already assigned, use it
        if (agentPrefab != null)
        {
            return agentPrefab;
        }
        
        // Create the agent parent object
        GameObject agent = new GameObject("Agent");
        
        // Create a simple humanoid character
        CreateHumanoidVisual(agent.transform);
        
        // Add Rigidbody
        Rigidbody rb = agent.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.mass = 1f;
        rb.linearDamping = 1f;
        rb.angularDamping = 1f;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        
        // Add capsule collider for the body
        CapsuleCollider collider = agent.AddComponent<CapsuleCollider>();
        collider.center = new Vector3(0, 0.75f, 0);
        collider.height = 1.5f;
        collider.radius = 0.25f;
        
        // Add AvoidAgent script
        AvoidAgent avoidAgent = agent.AddComponent<AvoidAgent>();
        avoidAgent.maxSpeed = 5f;
        avoidAgent.raycastDistance = 10f;
        
        // Add Ray Perception Sensor
        RayPerceptionSensorComponent3D raySensor = agent.AddComponent<RayPerceptionSensorComponent3D>();
        raySensor.RaysPerDirection = 4;
        raySensor.MaxRayDegrees = 90;
        raySensor.SphereCastRadius = 0.5f;
        raySensor.RayLength = 10;
        
        // Add tags to detect
        raySensor.DetectableTags = new List<string> { "Wall", "Obstacle", "Goal" };
        
        // Add ML-Agents components
        var behaviorParams = agent.AddComponent<BehaviorParameters>();
        behaviorParams.BehaviorName = "AvoidAgent";
        
        // Basic setup that should work with most ML-Agents versions
        // No preprocessors or actuator components
        
        // Add Decision Requester
        var decisionRequester = agent.AddComponent<DecisionRequester>();
        decisionRequester.DecisionPeriod = 5;
        
        agentPrefab = agent;
        return agent;
    }

    private void CreateHumanoidVisual(Transform parent)
    {
        // Create orange material for the agent
        Material orangeMaterial = new Material(Shader.Find("Standard"));
        orangeMaterial.color = new Color(1.0f, 0.5f, 0.0f); // Orange
        
        // Create head (sphere)
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.name = "Head";
        head.transform.SetParent(parent);
        head.transform.localPosition = new Vector3(0, 1.25f, 0);
        head.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        head.GetComponent<Renderer>().material = orangeMaterial;
        
        // Create body (cube)
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.name = "Body";
        body.transform.SetParent(parent);
        body.transform.localPosition = new Vector3(0, 0.75f, 0);
        body.transform.localScale = new Vector3(0.4f, 0.6f, 0.2f);
        body.GetComponent<Renderer>().material = orangeMaterial;
        
        // Create simple arms (cubes)
        GameObject leftArm = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftArm.name = "LeftArm";
        leftArm.transform.SetParent(parent);
        leftArm.transform.localPosition = new Vector3(-0.3f, 0.8f, 0);
        leftArm.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
        leftArm.GetComponent<Renderer>().material = orangeMaterial;
        
        GameObject rightArm = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightArm.name = "RightArm";
        rightArm.transform.SetParent(parent);
        rightArm.transform.localPosition = new Vector3(0.3f, 0.8f, 0);
        rightArm.transform.localScale = new Vector3(0.2f, 0.4f, 0.2f);
        rightArm.GetComponent<Renderer>().material = orangeMaterial;
        
        // Create simple legs (cubes)
        GameObject leftLeg = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftLeg.name = "LeftLeg";
        leftLeg.transform.SetParent(parent);
        leftLeg.transform.localPosition = new Vector3(-0.15f, 0.25f, 0);
        leftLeg.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
        leftLeg.GetComponent<Renderer>().material = orangeMaterial;
        
        GameObject rightLeg = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightLeg.name = "RightLeg";
        rightLeg.transform.SetParent(parent);
        rightLeg.transform.localPosition = new Vector3(0.15f, 0.25f, 0);
        rightLeg.transform.localScale = new Vector3(0.2f, 0.5f, 0.2f);
        rightLeg.GetComponent<Renderer>().material = orangeMaterial;
    }

    private GameObject CreateSpawner(GameObject agentPrefab, GameObject goal)
    {
        GameObject spawner = GameObject.Find("Spawner");
        if (spawner == null)
        {
            spawner = new GameObject("Spawner");
            
            // Add AgentSpawner component
            AgentSpawner spawnerComponent = spawner.AddComponent<AgentSpawner>();
            spawnerComponent.agentPrefab = agentPrefab;
            spawnerComponent.goalTransform = goal.transform;
            spawnerComponent.spawnOnStart = true;
            
            // Create spawn points
            GameObject spawnPoint = new GameObject("SpawnPoint_1");
            spawnPoint.transform.position = new Vector3(-4, 0.5f, -4);
            spawnPoint.transform.SetParent(spawner.transform);
            
            spawnerComponent.spawnPoints = new List<Transform> { spawnPoint.transform };
        }
        
        return spawner;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SceneBuilder_Fixed))]
    public class SceneBuilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            SceneBuilder_Fixed myScript = (SceneBuilder_Fixed)target;
            
            if (GUILayout.Button("Generate Basic Scene"))
            {
                myScript.CreateBasicScene();
            }
        }
    }
#endif
} 