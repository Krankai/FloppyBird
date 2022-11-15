using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsEngine : MonoBehaviour
{
    // Singleton pattern
    public static CustomPhysicsEngine Instance = null;

    [Header("Universal Settings")]
    [SerializeField] private float _gravityAcceleration = 9.80665f;

    [Header("Tweak Settings")]
    [SerializeField] private float _maximumUpwardDistance = 1.0f;       // distance at which custom rigid body object will be put on brake

    [SerializeField] private float _brakeVelocity = 3.0f;

    private Collider2D[] _colliders;                                    // custom rigid body will use this list to check for potential collision with collider objects

    public float GetGravityAcceleration => _gravityAcceleration;

    public float GetMaxUpwardDistance => _maximumUpwardDistance;

    public float GetBrakeVelocity => _brakeVelocity;

    public Collider2D[] GetColliders => _colliders;

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
        _colliders = FindObjectsOfType<Collider2D>();

        // foreach (var collider in _colliders)
        // {
        //     if (!collider.isActiveAndEnabled) continue;

        //     Debug.Log(collider.gameObject.transform.position);
        // }
    }
}
