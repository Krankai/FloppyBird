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

    private BoxCollider2D _selfBoxCollider;

    private Color _originalColor;

    public void AddForce(float forcePower)
    {
        // Assumption: impuse force
        _impulseVelocity = forcePower / _mass;
        _hasImpulseImpact = true;
    }

    private void Awake()
    {
        _selfBoxCollider = GetComponent<BoxCollider2D>();
        _originalColor = GetComponent<SpriteRenderer>().color;
    }

    private void Start()
    {
        // debug!!!
        // CheckCollision(targetCollider);
    }

    private void FixedUpdate()
    {
        ApplyGravity();

        ApplyImpulseForce();

        // Tweak modification to mimic 'rea' gameplay (Flappy Bird)
        TweakVelocity();

        UpdateRigidBodyPosition();

        ProcessCollision();
    }

    private void ApplyGravity()
    {
        if (!_isAppplyGravity) return;

        Velocity += CustomPhysicsEngine.Instance.GravityAcceleration * Time.fixedDeltaTime;
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
        //if (Vector3.Distance(_positionAtImpact, this.transform.position) < CustomPhysicsEngine.Instance.MaxUpwardDistance) return;
        if (Velocity >= CustomPhysicsEngine.Instance.MinimumVelocity) return;

        Velocity = Mathf.Sign(Velocity) * CustomPhysicsEngine.Instance.BrakeVelocity;
        _isUpdateImpactPosition = false;
    }

    private void UpdateRigidBodyPosition()
    {
        float verticalDisplacement = Velocity * Time.fixedDeltaTime;
        transform.position += new Vector3(0, -verticalDisplacement, 0);
    }

    private void ProcessCollision()
    {
        Collider2D[] colliders = CustomPhysicsEngine.Instance.Colliders;
        foreach (var collider in colliders)
        {
            BoxCollider2D boxCollider = (BoxCollider2D)collider;

            if (boxCollider == null || !boxCollider.isActiveAndEnabled) continue;
            if (this.gameObject.GetInstanceID() == boxCollider.gameObject.GetInstanceID()) continue;

            // Debug.Log("Object at " + this.transform.position + " collide with object at " + boxCollider.transform.position);

            if (IsInCollisionWith(boxCollider))
            {
                // Debug.Log("Collide");
                RegisterCollidedObject(boxCollider);
            }
            else
            {
                UnRegisterCollidedObject(boxCollider);
            }
        }
    }

    // Check collision based on Separation Axis Theorem (SAT)
    private bool IsInCollisionWith(BoxCollider2D boxCollider)
    {
        if (_selfBoxCollider == null || boxCollider == null) return false;

        List<Vector2> listEdgeNormals = new List<Vector2>();
        List<Vector2> listSelfVertices = new List<Vector2>();
        List<Vector2> listTargetVertices = new List<Vector2>();

        // Get all normals
        PopulateVerticesAndNormals(_selfBoxCollider, ref listEdgeNormals, ref listSelfVertices);
        PopulateVerticesAndNormals(boxCollider, ref listEdgeNormals, ref listTargetVertices);

        // For each normal vector...
        bool haveGaps = false;
        foreach (var normal in listEdgeNormals)
        {
            // Find min and max value of all vertices' projection onto the current normal vector, for self collider
            Vector2 selfMinPoint = Vector2.zero, selfMaxPoint = Vector2.zero;
            FindMinMaxProjection(ref selfMinPoint, ref selfMaxPoint, listSelfVertices, normal);

            // Similarly, but for target collider
            Vector2 targetMinPoint = Vector2.zero, targetMaxPoint = Vector2.zero;
            FindMinMaxProjection(ref targetMinPoint, ref targetMaxPoint, listTargetVertices, normal);

            // Debug.Log("Self min: " + selfMinPoint);
            // Debug.Log("Self max: " + selfMaxPoint);
            // Debug.Log("Target min: " + targetMinPoint);
            // Debug.Log("Target max: " + targetMaxPoint);

            // Check for overlapping
            haveGaps |= CheckForOverlap(selfMinPoint, selfMaxPoint, targetMinPoint, targetMaxPoint);
            // Debug.Log("Gap: " + haveGaps);
            if (haveGaps)
            {
                break;
            }
        }

        return !haveGaps;
    }

    private void PopulateVerticesAndNormals(BoxCollider2D boxCollider, ref List<Vector2> listNormals, ref List<Vector2> listVertices)
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

        // Populate normals list
        listNormals.Add(verticalNormal);
        listNormals.Add(horzontalNormal);

        // Populate vertices list
        listVertices.Add(colliderTransform.TransformPoint(localTopLeft));
        listVertices.Add(colliderTransform.TransformPoint(localTopRight));
        listVertices.Add(colliderTransform.TransformPoint(localBotLeft));
        listVertices.Add(colliderTransform.TransformPoint(localBotRight));
    }

    private void FindMinMaxProjection(ref Vector2 min, ref Vector2 max, List<Vector2> vertices, Vector2 projectionBase)
    {
        bool isInitState = false;

        foreach (var vertice in vertices)
        {
            Vector2 projectedPoint = Vector3.Project(vertice, projectionBase);

            if (!isInitState)
            {
                min = max = projectedPoint;
                isInitState = true;

                continue;
            }

            if (IsNewMin(min, projectedPoint))
            {
                min = projectedPoint;
            }

            if (IsNewMax(max, projectedPoint))
            {
                max = projectedPoint;
            }
        }
    }

    private bool IsNewMin(Vector2 currentMin, Vector2 checkValue)
    {
        return checkValue.x < currentMin.x || (checkValue.x == currentMin.x && checkValue.y < currentMin.y);
    }

    private bool IsNewMax(Vector2 currentMax, Vector2 checkValue)
    {
        return checkValue.x > currentMax.x || (checkValue.x == currentMax.x && checkValue.y > currentMax.y);
    }

    private bool CheckForOverlap(Vector2 selfMinPoint, Vector2 selfMaxPoint, Vector2 targetMinPoint, Vector2 targetMaxPoint)
    {
        if (selfMinPoint.x > targetMaxPoint.x || targetMinPoint.x > selfMaxPoint.x)
        {
            return true;
        }
        
        if (selfMinPoint.x == targetMaxPoint.x && selfMinPoint.y > targetMaxPoint.y)
        {
            return true;
        }

        if (targetMinPoint.x == selfMaxPoint.x && targetMinPoint.y > selfMaxPoint.y)
        {
            return true;
        }

        return false;
    }

    private void RegisterCollidedObject(BoxCollider2D collider)
    {
        InvokeCollisionDetection(this.gameObject, collider.gameObject, true);
        InvokeCollisionDetection(collider.gameObject, this.gameObject, true);
    }

    private void UnRegisterCollidedObject(BoxCollider2D collider)
    {
        InvokeCollisionDetection(this.gameObject, collider.gameObject, false);
        InvokeCollisionDetection(collider.gameObject, this.gameObject, false);
    }

    private void InvokeCollisionDetection(GameObject selfObject, GameObject targetObject, bool isCollide)
    {
        CollisionDetectionSubscriber collisionDetectionComponent = selfObject.GetComponent<CollisionDetectionSubscriber>();
        if (collisionDetectionComponent != null)
        {
            collisionDetectionComponent.OnInvokeCollision(targetObject, isCollide);
        }
    }
}
