using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionSubscriber : MonoBehaviour
{
    // Triggered on collision via custom physics engine
    private List<GameObject> _colliders;     // list of box colliders of all collided objects

    public void OnInvokeCollision(GameObject targetObject, bool isCollide)
    {
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
        // Debug.Log("OnCollisionEnter: " + this.gameObject.name);
    }

    public void OnCollisionStayCustom()
    {
        // Debug.Log("OnCollisionStay: " + this.gameObject.name);
    }

    public void OnCollisionExitCustom()
    {
        // Debug.Log("OnCollisionExit: " + this.gameObject.name);
    }

    private void Awake()
    {
        _colliders = new List<GameObject>();
    }
}
