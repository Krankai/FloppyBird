using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsEngine : MonoBehaviour
{
    // Singleton pattern
    public static CustomPhysicsEngine Instance = null;

    [Header("Gravity")]
    [Tooltip("Please do not change this. Update the modifier below instead")]
    [SerializeField] private float _gravityAcceleration = 9.80665f;

    [SerializeField] private float _gravityModifier = 1.5f;

    [Header("Upward Impulse")]
    [SerializeField] private float _impulseForce = 25f;

    // [SerializeField] private float _maximumUpwardDistance = 2f;       // distance at which custom rigid body object will be put on brake

    [SerializeField] private float _minimumVelocity = -9f;

    [SerializeField] private float _brakeVelocity = 5f;

    private Collider2D[] _colliders;                                    // custom rigid body will use this list to check for potential collision with collider objects

    public float GravityAcceleration => _gravityAcceleration * _gravityModifier;

    public float ImpulseForce => _impulseForce;

    // public float MaxUpwardDistance => _maximumUpwardDistance;

    public float MinimumVelocity => _minimumVelocity;

    public float BrakeVelocity => _brakeVelocity;

    public Collider2D[] Colliders => _colliders;

    public void UpdateColliders()
    {
        _colliders = FindObjectsOfType<Collider2D>();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        UpdateColliders();
    }
}
