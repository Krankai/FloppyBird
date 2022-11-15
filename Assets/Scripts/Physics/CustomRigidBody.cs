using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomRigidBody : MonoBehaviour
{
    // Todo #1: generalize gravity direction

    // DEBUG
    public BoxCollider2D targetCollider;

    [field:SerializeField] public float Velocity { get; private set; } = 0f;

    [SerializeField] private bool _isAppplyGravity = true;

    [SerializeField] private float _mass = 1;

    private bool _hasImpulseImpact = false;

    private float _impulseVelocity;

    private bool _isUpdateImpactPosition = false;

    private Vector3 _positionAtImpact;

    public void AddForce(float forcePower)
    {
        // Assumption: impuse force
        _impulseVelocity = forcePower / _mass;
        _hasImpulseImpact = true;
    }

    private void Start()
    {
        // debug!!!
        CheckCollision(targetCollider);
    }

    private void FixedUpdate()
    {
        ApplyGravity();

        // Check to apply 'upward' impulse
        ApplyImpulseForce();

        // Tweak velocity to simulate 'falling' effect like in Flappy Bird
        TweakVelocity();

        UpdateRigidBodyPosition();
    }

    private void ApplyGravity()
    {
        if (!_isAppplyGravity) return;

        Velocity += CustomPhysicsEngine.Instance.GetGravityAcceleration * Time.fixedDeltaTime;
    }

    private void ApplyImpulseForce()
    {
        if (!_hasImpulseImpact) return;

        Velocity -= _impulseVelocity;

        _positionAtImpact = this.transform.position;
        _isUpdateImpactPosition = true;

        _hasImpulseImpact = false;
        _impulseVelocity = 0f;
    }

    private void TweakVelocity()
    {
        if (!_isUpdateImpactPosition) return;
        if (Vector3.Distance(_positionAtImpact, this.transform.position) < CustomPhysicsEngine.Instance.GetMaxUpwardDistance) return;

        Velocity = Mathf.Sign(Velocity) * CustomPhysicsEngine.Instance.GetBrakeVelocity;
        _isUpdateImpactPosition = false;
    }

    private void UpdateRigidBodyPosition()
    {
        float verticalDisplacement = Velocity * Time.fixedDeltaTime;
        transform.position += new Vector3(0, -verticalDisplacement, 0);
    }

    private void CheckCollision(BoxCollider2D boxCollider)
    {
        // note: assuming both BoxCollider
        List<Vector2> listEdgeNormals = new List<Vector2>();

        BoxCollider2D selfBoxCollider = GetComponent<BoxCollider2D>();
        if (selfBoxCollider == null) return;

        // Get all normals
        PopulateNormalVectors(selfBoxCollider, ref listEdgeNormals);
        PopulateNormalVectors(boxCollider, ref listEdgeNormals);

        // Debug!!!
        // foreach (var normal in listEdgeNormals)
        // {
        //     Debug.Log(normal);
        // }

        // TODO:...
    }

    private void PopulateNormalVectors(BoxCollider2D boxCollider, ref List<Vector2> list)
    {
        Transform colliderTransform = boxCollider.transform;
        Vector2 colliderCenter = Vector2.zero;      // local

        Vector2 min = colliderCenter - boxCollider.size * 0.5f;
        Vector2 max = colliderCenter + boxCollider.size * 0.5f;

        // Compute local position of 4 vertices
        Vector3 localTopLeft = new Vector2(min.x, max.y);
        Vector3 localTopRight = new Vector2(max.x, max.y);
        Vector3 localBotLeft = new Vector2(min.x, min.y);
        Vector3 localBotRight = new Vector2(max.x, min.y);

        // Compute normal vectors of 4 sides
        Vector2 localMiddleTop = localTopLeft + (localTopRight - localTopLeft) * 0.5f;
        Vector2 verticalNormal = colliderTransform.TransformVector(localMiddleTop - colliderCenter).normalized;

        Vector2 localMiddleLeft = localTopLeft + (localBotLeft - localTopLeft) * 0.5f;
        Vector2 horzontalNormal = colliderTransform.TransformVector(localMiddleLeft - colliderCenter).normalized;

        // Populate the list with computed normal vectors
        list.Add(verticalNormal);
        list.Add(horzontalNormal);
    }
}
