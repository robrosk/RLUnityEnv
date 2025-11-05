using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Thin wrapper for the goal trigger volumes so they can notify managers when
/// a scoring event occurs. Implementation will be added later; this component
/// ensures the collider is configured as a trigger and exposes a UnityEvent for
/// inspector-driven wiring.
/// </summary>
[RequireComponent(typeof(Collider))]
public class GoalTrigger : MonoBehaviour
{
    public string goalIdentifier = "A";
    public UnityEvent onGoalScored;

    [HideInInspector]
    public Collider triggerCollider;

    private void Awake()
    {
        triggerCollider = GetComponent<Collider>();
        triggerCollider.isTrigger = true;
    }
}
