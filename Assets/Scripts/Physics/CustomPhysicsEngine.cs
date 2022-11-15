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
    [SerializeField] private float _maximumUpwardDistance = 5.0f;       // distance at which custom rigid body object will be put on brake

    [SerializeField] private float _brakeVelocity = 5.0f;

    //private CustomPhysicsBody[] _physicsBodies;

    public float GravityAcceleration => _gravityAcceleration;

    public float MaximumUpwardDistance => _maximumUpwardDistance;

    public float BrakeVelocity => _brakeVelocity;

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
        //_physicsBodies = FindObjectsOfType<CustomPhysicsBody>();
    }
}
