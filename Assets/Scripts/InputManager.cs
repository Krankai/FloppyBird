using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _realPhysicsBody;

    [SerializeField] private CustomPhysicsBody _customPhysicsBody;

    private float _sampleForce = 20f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _realPhysicsBody.AddForce(new Vector2(0, _sampleForce), ForceMode2D.Impulse);
            _customPhysicsBody.AddForce(_sampleForce);
        }
    }
}
