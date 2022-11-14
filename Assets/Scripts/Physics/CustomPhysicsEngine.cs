using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsEngine : MonoBehaviour
{
    // The global instance for reference - Singleton design pattern
    public static CustomPhysicsEngine Instance = null;

    [SerializeField] private float _gravityAcceleration = 9.80665f;

    //private CustomPhysicsBody[] _physicsBodies;

    public float GravityAcceleration => _gravityAcceleration;

    private void Awake()
    {
        // When this component is first added or activated, setup the global reference
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
