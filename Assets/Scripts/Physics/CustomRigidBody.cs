using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRigidBody : MonoBehaviour
{
    // Todo #1: generalize gravity direction

    [SerializeField] private float _mass = 1;

    [field:SerializeField] public float Velocity { get; private set; }

    private bool _hasImpulseImpact;

    private float _impulseVelocity;

    private Vector3 _positionAtImpact;

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
        _positionAtImpact = Vector3.zero;
    }

    private void FixedUpdate()
    {
        Velocity += CustomPhysicsEngine.Instance.GravityAcceleration * Time.fixedDeltaTime;
        //Velocity += Physics.gravity.magnitude * Time.fixedDeltaTime;

        // Check to apply 'upward' impulse
        if (_hasImpulseImpact)
        {
            Velocity -= _impulseVelocity;
            _positionAtImpact = this.transform.position;

            _hasImpulseImpact = false;
            _impulseVelocity = 0f;
        }

        if (_positionAtImpact.magnitude > 0 && Vector3.Distance(_positionAtImpact, this.transform.position) >= CustomPhysicsEngine.Instance.MaximumUpwardDistance)
        {
            Velocity = Mathf.Sign(Velocity) * CustomPhysicsEngine.Instance.BrakeVelocity;
            _positionAtImpact = Vector3.zero;
        }

        float verticalDisplacement = Velocity * Time.fixedDeltaTime;
        transform.position += new Vector3(0, -verticalDisplacement, 0);
    }
}
