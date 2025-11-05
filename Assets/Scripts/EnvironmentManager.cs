using UnityEngine;

/// <summary>
/// Root environment coordinator responsible for high-level lifecycle hooks.
/// Logic will be implemented in future iterations; this class currently only exposes
/// inspector references for the scene setup.
/// </summary>
public class EnvironmentManager : MonoBehaviour
{
    [Header("Agents")]
    public AgentController agentA;
    public AgentController agentB;

    [Header("Ball")]
    public BallController ball;

    [Header("Goals")]
    public GoalTrigger goalATrigger;
    public GoalTrigger goalBTrigger;

    [Header("Field Bounds")]
    public Collider fieldCollider;

    [Header("Episode Settings")]
    [Tooltip("Maximum episode length in seconds before a forced reset.")]
    public float maxEpisodeLengthSeconds = 300f;

    [Tooltip("Delay before physics objects are released after a reset.")]
    public float postResetDelaySeconds = 0.2f;

    [Tooltip("Optional soft limit for stalemate situations with no goals scored.")]
    public float stalemateTimeoutSeconds = 45f;
}
