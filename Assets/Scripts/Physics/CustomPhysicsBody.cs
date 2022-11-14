using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysicsBody : MonoBehaviour
{
    [SerializeField] private float _mass = 100;

    [field:SerializeField] public float Velocity { get; private set; }

    private void Start()
    {
        Velocity = 0f;
    }

    private void FixedUpdate()
    {
        Velocity += CustomPhysicsEngine.Instance.GravityAcceleration * Time.fixedDeltaTime;

        float verticalDisplacement = Velocity * Time.fixedDeltaTime;
        transform.position += new Vector3(0, -verticalDisplacement, 0);
    }
}
