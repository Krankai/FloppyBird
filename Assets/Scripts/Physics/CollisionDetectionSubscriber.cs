using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionSubscriber : MonoBehaviour
{
    // Triggered on collision via custom physics engine
    private List<GameObject> _colliders;     // list of box colliders of all collided objects

    // For debug
    private Color _originalColor;

    public void OnInvokeCollision(GameObject targetObject, bool isCollide)
    {
        // Debug.Log("InvokeCollision: " + this.gameObject.name + " at " + this.transform.position + " with target at " + targetObject.transform.position);

        bool foundMatchedCollider = _colliders.Exists(element => element.GetInstanceID() == targetObject.GetInstanceID());
        if (isCollide)
        {
            if (!foundMatchedCollider)
            {
                // Enter
                OnCollisionEnterCustom();

                _colliders.Add(targetObject);
            }
            else
            {
                // Stay
                OnCollisionStayCustom();
            }
        }
        else if (foundMatchedCollider)
        {
            // Exit
            OnCollisionExitCustom();

            _colliders.Remove(targetObject);
        }
    }

    public void OnCollisionEnterCustom()
    {
        // Debug.Log("OnCollisionEnter: " + this.gameObject.name + " at " + this.transform.position);
    }

    public void OnCollisionStayCustom()
    {
        // Debug.Log("OnCollisionStay: " + this.gameObject.name + " at " + this.transform.position);
    }

    public void OnCollisionExitCustom()
    {
        // Debug.Log("OnCollisionExit: " + this.gameObject.name + " at " + this.transform.position);
    }

    private void Awake()
    {
        _colliders = new List<GameObject>();
    }

    private void Start()
    {
        _originalColor = GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {
        ChangePlayerColorOnCollision();
    }

    private void ChangePlayerColorOnCollision()
    {
        if (!CompareTag("Player")) return;

        if (_colliders.Count > 0)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = _originalColor;
        }
    }
}
