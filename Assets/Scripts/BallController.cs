using UnityEngine;

/// <summary>
/// Placeholder for future ball control logic including kicks and reset routines.
/// Currently only caches component references and exposes configuration fields.
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public class BallController : MonoBehaviour
{
    [Header("Physics")]
    public float maxLinearSpeed = 30f;
    public float maxAngularSpeed = 50f;
    public float kickImpulse = 8f;
    public PhysicMaterial contactMaterial;

    [HideInInspector]
    public Rigidbody body;

    [HideInInspector]
    public SphereCollider sphereCollider;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }
}
