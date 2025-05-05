# RL3DEnv - Unity ML-Agents 3D Navigation Environment

A 3D reinforcement learning environment built with Unity ML-Agents for training agents to navigate to a goal while avoiding obstacles.

## Requirements

- Unity 2020.3 or newer
- ML-Agents package (version 2.0.0 or newer)
- TensorFlow Sharp (for inference mode)

## Project Structure

```
Assets/
├── Prefabs/
│   ├── Agent.prefab
│   ├── Goal.prefab
│   ├── Obstacle.prefab
│   └── Wall.prefab
├── Scenes/
│   └── AvoidanceEnvironment.unity
└── Scripts/
    ├── AvoidAgent.cs
    ├── AgentSpawner.cs
    ├── BoundaryWall.cs
    ├── GoalObject.cs
    └── Obstacle.cs
```

## Setup Instructions

1. Open Unity Hub and create a new 3D project
2. Install the ML-Agents package via Package Manager:
   - Window > Package Manager > + > Add package from git URL
   - Enter `com.unity.ml-agents`
3. Import the project files
4. Open the `AvoidanceEnvironment` scene

## Scene Configuration

The scene includes:
- A 10x10x3 bounded environment
- Three static obstacles (pillars)
- A goal object (colored sphere)
- A spawner with four spawn points for the agent

## Agent Configuration

The `AvoidAgent` is configured with:
- Continuous action space (size 3)
- Observations including position, velocity, goal direction
- Ray perception sensors for obstacle detection
- Rigidbody-based movement

## Training

1. Configure the agent's behavior parameters:
   - Behavior Name: "AvoidAgent"
   - Vector Observation Space Size: 18
   - Vector Action Space: Continuous, Size: 3
   - Behavior Type: Default (for training via Python)

2. Use the ML-Agents Python API to train the agent with PPO:
   ```
   mlagents-learn config/ppo/AvoidAgent.yaml --run-id=avoid_agent_1
   ```

## Testing in Editor

1. Change Behavior Type to "Heuristic Only"
2. Enter Play mode
3. Control the agent using arrow keys/WASD
4. Press spacebar to spawn new agents
5. Use the UI buttons to manually reset or spawn agents

## Contact

For any questions or issues, please open an issue on the GitHub repository. 