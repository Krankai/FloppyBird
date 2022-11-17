using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Winner Settings")]
    [SerializeField] private float _winRotationRadius = 3f;

    [SerializeField] private float _winRotationAngle = -180f;

    [Header("Loser Settings")]
    [SerializeField] private float _loseRotationAngle = 60f;

    [SerializeField] private float _loseFallingSpeed = 5f;

    private bool _isWinning = false;

    private bool _isLosing = false;

    private Vector3 _winRotationCenter;

    public void WinAnimation()
    {
        _isWinning = true;
        _winRotationCenter = new Vector3(transform.position.x + _winRotationRadius, 0, 0);

        transform.Rotate(transform.forward, 90);
    }

    public void LoseAnimation()
    {
        _isLosing = true;
    }

    private void Update()
    {
        if (_isWinning)
        {
            transform.RotateAround(_winRotationCenter, Vector3.forward, _winRotationAngle * Time.deltaTime);
            return;
        }

        if (_isLosing && transform.position.y >= GameManager.Instance.GetDeadZoneLimit())
        {
            transform.Rotate(transform.forward, _loseRotationAngle * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y - _loseFallingSpeed * Time.deltaTime, 0);
            return;
        }
    }
}
