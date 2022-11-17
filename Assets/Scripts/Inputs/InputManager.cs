using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private CustomRigidBody _playerRigidBody;

    private bool _isDisable = false;

    public void FakePause()
    {
        // note: use only when lose
        _isDisable = true;
    }

    public void EnableReceiveInputs()
    {
        _isDisable = false;
    }

    private void Start()
    {
        if (_playerRigidBody == null)
        {
            _playerRigidBody = GameObject.Find("Player").GetComponent<CustomRigidBody>();
        }

        _isDisable = true;
        Invoke("EnableReceiveInputs", GameManager.Instance.GetStartDelay());
    }

    private void Update()
    {
        if (_isDisable) return;

        CheckInputsForImpulseForce();
        CheckInputsForPause();
    }

    private void CheckInputsForImpulseForce()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_playerRigidBody != null && _playerRigidBody.CompareTag("Player"))
            {
                _playerRigidBody.AddForce(CustomPhysicsEngine.Instance.ImpulseForce);
                AudioManager.Instance.OnPlayJumpSound();
            }
        }
    }

    private void CheckInputsForPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.TogglePauseState();
        }
    }
}
