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
                OnCollisionEnterCustom(targetObject);

                _colliders.Add(targetObject);
            }
            else
            {
                // Stay
                OnCollisionStayCustom(targetObject);
            }
        }
        else if (foundMatchedCollider)
        {
            // Exit
            OnCollisionExitCustom(targetObject);

            _colliders.Remove(targetObject);
        }
    }

    public void OnCollisionEnterCustom(GameObject collideObject)
    {
        // Debug.Log("OnCollisionEnter: " + this.gameObject.name + " at " + this.transform.position);

        if (CompareTag("Player") && collideObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.GameOver(false);
        }
    }

    public void OnCollisionStayCustom(GameObject collideObject)
    {
        // Debug.Log("OnCollisionStay: " + this.gameObject.name + " at " + this.transform.position);
    }

    public void OnCollisionExitCustom(GameObject collideObject)
    {
        // Debug.Log("OnCollisionExit: " + this.gameObject.name + " at " + this.transform.position);

        if (CompareTag("Player") && collideObject.CompareTag("ScoreTracker"))
        {
            GameManager.Instance.UpdateScore();
        }
    }

    private void Awake()
    {
        _colliders = new List<GameObject>();
    }

    private void Start()
    {
        if (CompareTag("Player"))
        {
            _originalColor = GetComponent<SpriteRenderer>().color;
        }
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
