using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Asset")]
    [SerializeField] private GameObject _pipePrefab;

    [SerializeField] private Transform _parentTransform;

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

    private void Awake()
    {
        _inversePrefabRotation = Quaternion.Inverse(_pipePrefab.transform.rotation);

        var spriteRenderer = _pipePrefab.GetComponent<SpriteRenderer>();
        _pipeSpriteHeight = spriteRenderer.sprite.bounds.size.y;
    }

    private void Start()
    {
        InvokeRepeating("SpawnPipeObject", 1.0f, _spawnInterval);
    }

    private void SpawnPipeObject()
    {
        GenerateHeightScale();
        GenerateSpawnPositions();

        var upperPipe = Instantiate(_pipePrefab, _spawnUpperPosition, _pipePrefab.transform.rotation, _parentTransform);
        upperPipe.transform.localScale = new Vector2(upperPipe.transform.localScale.x, _upperScaleY);

        var lowerPipe = Instantiate(_pipePrefab, _spawnLowerPosition, _inversePrefabRotation, _parentTransform);
        lowerPipe.transform.localScale = new Vector2(lowerPipe.transform.localScale.x, _lowerScaleY);
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
}
