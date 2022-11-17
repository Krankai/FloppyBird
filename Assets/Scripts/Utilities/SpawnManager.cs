using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Asset")]
    [SerializeField] private GameObject _pipePrefab;

    [SerializeField] private Transform _pipeParentTransform;

    [SerializeField] private GameObject _scoreTrackerPrefab;

    [SerializeField] private Transform _trackersParentTransform;

    [Header("Spawning Positions")]
    [SerializeField] private float _spawnInterval = 1.5f;

    [SerializeField] private float _spawnPostionMinX = 27f;

    [SerializeField] private float _spawnPostionMaxX = 33f;

    [SerializeField] private float _minGapSize = 3;

    [SerializeField] private float _maxGapSize = 7;

    [SerializeField] private float _backgroundHeight = 20f;

    private Vector2 _spawnUpperPosition;

    private Vector2 _spawnLowerPosition;

    private Quaternion _inversePrefabRotation;

    private float _upperScaleY;

    private float _lowerScaleY;

    private float _pipeSpriteHeight;

    private float _pipeSpriteWidth;

    public void DisableSpawning()
    {
        CancelInvoke();
    }

    private void Awake()
    {
        _inversePrefabRotation = Quaternion.Inverse(_pipePrefab.transform.rotation);

        var spriteRenderer = _pipePrefab.GetComponent<SpriteRenderer>();
        _pipeSpriteHeight = spriteRenderer.sprite.bounds.size.y;
        _pipeSpriteWidth = spriteRenderer.sprite.bounds.size.x;
    }

    private void Start()
    {
        InvokeRepeating("SpawnPipeObject", GameManager.Instance.GetStartDelay(), _spawnInterval);
    }

    private void SpawnPipeObject()
    {
        GenerateHeightScale();
        GenerateSpawnPositions();

        SetPipePositionAndScale();

        SpawnScoreTracker();

        //CustomPhysicsEngine.Instance.UpdateColliders();
    }

    private void GenerateSpawnPositions()
    {
        // y axis
        float upperPositionY = (_backgroundHeight - _upperScaleY * _pipeSpriteHeight) / 2f;
        float lowerPositionY = (-_backgroundHeight + _lowerScaleY * _pipeSpriteHeight) / 2f;

        // x axis
        float randomPositionX = Random.Range(_spawnPostionMinX, _spawnPostionMaxX);

        _spawnUpperPosition = new Vector2(randomPositionX, upperPositionY);
        _spawnLowerPosition = new Vector2(randomPositionX, lowerPositionY);
    }

    private void GenerateHeightScale()
    {
        float gapSize = Random.Range(_minGapSize, _maxGapSize);
        float upperGapSize = Random.Range(0, gapSize);
        float lowerGapSize = gapSize - upperGapSize;

        _upperScaleY = (_backgroundHeight / 2f - upperGapSize) / _pipeSpriteHeight;
        _lowerScaleY = (_backgroundHeight / 2f - lowerGapSize) / _pipeSpriteHeight;
    }

    private void SetPipePositionAndScale()
    {
        var upperPipe = Instantiate(_pipePrefab, _spawnUpperPosition, _pipePrefab.transform.rotation, _pipeParentTransform);
        var upperPipeLocalScale = upperPipe.transform.localScale;
        upperPipe.transform.localScale = new Vector3(upperPipeLocalScale.x, _upperScaleY, upperPipeLocalScale.z);

        var lowerPipe = Instantiate(_pipePrefab, _spawnLowerPosition, _inversePrefabRotation, _pipeParentTransform);
        var lowerPipeLocalScale = lowerPipe.transform.localScale;
        lowerPipe.transform.localScale = new Vector3(lowerPipeLocalScale.x, _lowerScaleY, lowerPipeLocalScale.z);

        // Update newly spawned collider list
        CustomPhysicsEngine.Instance.UpdateCollider(upperPipe.GetComponent<BoxCollider2D>());
        CustomPhysicsEngine.Instance.UpdateCollider(lowerPipe.GetComponent<BoxCollider2D>());
    }

    private void SpawnScoreTracker()
    {
        float positionX = Mathf.Max(_spawnUpperPosition.x, _spawnLowerPosition.x) + _pipeSpriteWidth / 2f;
        Vector2 spawnLocation = new Vector3(positionX, 0);

        var scoreTracker = Instantiate(_scoreTrackerPrefab, spawnLocation, _scoreTrackerPrefab.transform.rotation, _trackersParentTransform);

        // Update newly spawned collider list
        CustomPhysicsEngine.Instance.UpdateCollider(scoreTracker.GetComponent<BoxCollider2D>());
    }
}
