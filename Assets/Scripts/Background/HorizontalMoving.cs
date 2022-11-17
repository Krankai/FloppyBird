using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMoving : MonoBehaviour
{
    public enum Direction
    {
        Left = -1,
        Right = 1,
    }

    [Header("Settings")]
    [SerializeField] private Direction _movingDirection = Direction.Left;

    [SerializeField] private float _movingSpeed = 8f;

    private bool _isDisable = false;

    private Vector3 _triviaRightVector = Vector3.right;

    public void FakePause()
    {
        // note: use only when lose
        _isDisable = true;
    }

    private void Update()
    {
        if (_isDisable) return;
        this.transform.position += _triviaRightVector * (int)_movingDirection * _movingSpeed * Time.deltaTime;
    }
}
