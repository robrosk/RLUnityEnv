using UnityEngine;

/// <summary>
/// Handles applying movement and kick actions for an agent capsule.
/// Gameplay logic is deferred; this component currently exposes configurable
/// parameters and cached references for the RL bridge to use later.
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class AgentController : MonoBehaviour
{
    [Header("Movement")]
    public float moveForce = 400f;
    public float maxSpeed = 5f;
    public float sprintMultiplier = 1.5f;

    [Header("Kicking")]
    public float kickImpulse = 8f;
    public float kickCooldown = 0.35f;
    public float kickRange = 1f;

    [Header("References")]
    public Transform footTransform;
    public BallController trackedBall;

    [HideInInspector]
    public Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
}
