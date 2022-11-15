using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _realPhysicsBody;

    [SerializeField] private CustomRigidBody _customPhysicsBody;

    private float _sampleForce = 20f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_realPhysicsBody != null && _realPhysicsBody.CompareTag("Player"))
            {
                _realPhysicsBody.AddForce(new Vector2(0, _sampleForce), ForceMode2D.Impulse);
            }

            if (_customPhysicsBody != null && _customPhysicsBody.CompareTag("Player"))
            {
                _customPhysicsBody.AddForce(_sampleForce);
            }
        }
    }
}
