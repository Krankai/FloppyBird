using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsEngine : MonoBehaviour
{
    // Singleton
    public static CustomPhysicsEngine Instance = null;

    [Header("Gravity")]
    [Tooltip("Please do not change this. Update the modifier below instead")]
    [SerializeField] private float _gravityAcceleration = 9.80665f;

    [SerializeField] private float _gravityModifier = 1.5f;

    [Header("Upward Impulse")]
    [SerializeField] private float _impulseForce = 25f;

    [SerializeField] private float _minimumVelocity = -9f;

    [SerializeField] private float _brakeVelocity = 5f;

    private List<BoxCollider2D> _colliders;     // CustomRigidBody will use this list to check for potential collision with collider objects

    public float GravityAcceleration => _gravityAcceleration * _gravityModifier;

    public float ImpulseForce => _impulseForce;

    public float MinimumVelocity => _minimumVelocity;

    public float BrakeVelocity => _brakeVelocity;

    public List<BoxCollider2D> Colliders => _colliders;

    public void UpdateColliders()
    {
        var colliderArray = FindObjectsOfType<BoxCollider2D>();
        for (int i = 0; i < colliderArray.Length; ++i)
        {
            _colliders.Add(colliderArray[i]);
        }
    }

    public void UpdateCollider(BoxCollider2D newCollider)
    {
        _colliders.Add(newCollider);
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

        _colliders = new List<BoxCollider2D>();
    }

    private void Start()
    {
        UpdateColliders();
    }
}
