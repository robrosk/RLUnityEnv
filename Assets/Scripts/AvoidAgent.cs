using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AvoidAgent : Agent
{
    [Header("Movement")]
    public float maxSpeed = 5f;
    public float raycastDistance = 10f;
    
    [Header("References")]
    public Transform goalTransform;

    private Rigidbody agentRigidbody;
    private RayPerceptionSensorComponent3D raySensor;

    // Called once at the beginning of the simulation
    public override void Initialize()
    {
        // Cache references to components
        agentRigidbody = GetComponent<Rigidbody>();
        raySensor = GetComponent<RayPerceptionSensorComponent3D>();
    }

    // Called every time a new episode starts
    public override void OnEpisodeBegin()
    {
        // Reset the agent's velocity
        agentRigidbody.linearVelocity = Vector3.zero;
        agentRigidbody.angularVelocity = Vector3.zero;
    }

    // Called every step to collect observations from the environment
    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent's local position (x, y, z) - 3 values
        sensor.AddObservation(transform.localPosition);
        
        // Agent's local velocity (x, y, z) - 3 values
        sensor.AddObservation(agentRigidbody.linearVelocity);
        
        // Relative vector to the goal (goalPos - agentPos) - 3 values
        Vector3 relativeGoalPosition = goalTransform.position - transform.position;
        sensor.AddObservation(relativeGoalPosition);
        
        // Note: The eight ray-cast distance readings (8 values) will be added
        // automatically by the RayPerceptionSensorComponent3D
    }

    // Called every step to act based on provided action
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Extract the continuous action vector
        Vector3 moveDirection = new Vector3(
            actionBuffers.ContinuousActions[0],
            0f, // Constrain to horizontal plane
            actionBuffers.ContinuousActions[2]
        );
        
        // Apply velocity
        agentRigidbody.linearVelocity = moveDirection * maxSpeed;
    }

    // For manual testing with direct keyboard input
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[2] = Input.GetAxis("Vertical");
    }

    // Handle collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            SetReward(1.0f);
            EndEpisode();
        }
        else if (other.CompareTag("Obstacle") || other.CompareTag("Wall"))
        {
            SetReward(-1.0f);
            EndEpisode();
        }
    }
} 