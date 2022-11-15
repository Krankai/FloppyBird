using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetectionSubscriber : MonoBehaviour
{
    // Triggered on collision via custom physics engine

    public void OnCollisionStayCustom()
    {
        Debug.Log("OnCollisionStay: " + this.gameObject.name);
    }
}
