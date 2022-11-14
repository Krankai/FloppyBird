using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsBody : MonoBehaviour
{
    // Todo #1: generalize gravity direction

    [SerializeField] private float _mass = 1;

    [field:SerializeField] public float Velocity { get; private set; }

    private bool _hasImpulseImpact;

    private float _impulseVelocity;

    public void AddForce(float forcePower)
    {
        // Assumption: impuse force
        _impulseVelocity = forcePower / _mass;
        _hasImpulseImpact = true;
    }

    private void Start()
    {
        Velocity = 0f;
        _hasImpulseImpact = false;
    }

    private void FixedUpdate()
    {
        Velocity += CustomPhysicsEngine.Instance.GravityAcceleration * Time.fixedDeltaTime;
        //Velocity += Physics.gravity.magnitude * Time.fixedDeltaTime;

        if (_hasImpulseImpact)
        {
            Velocity -= _impulseVelocity;

            _hasImpulseImpact = false;
            _impulseVelocity = 0f;
        }

        float verticalDisplacement = Velocity * Time.fixedDeltaTime;
        transform.position += new Vector3(0, -verticalDisplacement, 0);
    }
}
