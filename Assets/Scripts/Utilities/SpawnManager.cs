using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // TODO #1: Stop spawning if enough obstacles (and score tracker) to reach maximum score

    // Singleton
    public static SpawnManager Instance = null;

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

    private PoolManager _poolManager;

    private GameObject _scoreTracker;

    private Queue<Vector3> _trackerSpawnLocationQueue;

    private float _upperScaleY;

    private float _lowerScaleY;

    private float _pipeSpriteHeight;

    private float _pipeSpriteWidth;

    private bool _isInitScoreTrackerLocation = false;

    private bool IsUsePool => _poolManager != null;

    public void DisableSpawning()
    {
        CancelInvoke();
    }

    public void RespawnScoreTracker()
    {
        Vector3 spawnLocation = _trackerSpawnLocationQueue.Dequeue();
        _scoreTracker.transform.position = _pipeParentTransform.TransformPoint(spawnLocation);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        _inversePrefabRotation = Quaternion.Inverse(_pipePrefab.transform.rotation);

        var spriteRenderer = _pipePrefab.GetComponent<SpriteRenderer>();
        _pipeSpriteHeight = spriteRenderer.sprite.bounds.size.y;
        _pipeSpriteWidth = spriteRenderer.sprite.bounds.size.x;

        _poolManager = GetComponent<PoolManager>();

        _trackerSpawnLocationQueue = new Queue<Vector3>();
    }

    private void Start()
    {
        InvokeRepeating("SpawnPipeObject", GameManager.Instance.GetStartDelay(), _spawnInterval);

        _scoreTracker = Instantiate(_scoreTrackerPrefab, new Vector3(100, 0, 0), _scoreTrackerPrefab.transform.rotation, _trackersParentTransform);
        CustomPhysicsEngine.Instance.UpdateCollider(_scoreTracker.GetComponent<BoxCollider2D>());
    }

    private void SpawnPipeObject()
    {
        GenerateHeightScale();
        GenerateSpawnPositions();

        SetPipePositionAndScale();

        UpdateTrackerLocations();
        if (!_isInitScoreTrackerLocation)
        {
            RespawnScoreTracker();
            _isInitScoreTrackerLocation = true;
        }
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
        var upperPipe = InstantiateObjectFromPool(_spawnUpperPosition, _pipePrefab.transform.rotation, _upperScaleY);
        var lowerPipe = InstantiateObjectFromPool(_spawnLowerPosition, _inversePrefabRotation, _lowerScaleY);

        // Update newly spawned collider list
        if (!IsUsePool)
        {
            CustomPhysicsEngine.Instance.UpdateCollider(upperPipe.GetComponent<BoxCollider2D>());
            CustomPhysicsEngine.Instance.UpdateCollider(lowerPipe.GetComponent<BoxCollider2D>());
        }
    }

    private void UpdateTrackerLocations()
    {
        float positionX = Mathf.Max(_spawnUpperPosition.x, _spawnLowerPosition.x) + _pipeSpriteWidth / 2f;
        Vector3 spawnLocation = new Vector3(positionX, 0, 0);

        _trackerSpawnLocationQueue.Enqueue(_pipeParentTransform.InverseTransformPoint(spawnLocation));
    }

    private GameObject InstantiateObjectFromPool(Vector3 spawnLocation, Quaternion spawnRotation, float scale)
    {
        GameObject spawnedObject = null;
        if (!IsUsePool)
        {
            Debug.LogWarning("No PoolManager attached. Instantiate object normally.");
            spawnedObject = Instantiate(_pipePrefab, spawnLocation, spawnRotation, _pipeParentTransform);
        }
        else
        {
            spawnedObject = _poolManager.GetPoolObject();
            spawnedObject.transform.position = spawnLocation;
            spawnedObject.transform.rotation = spawnRotation;
        }

        var currentLocalScale = spawnedObject.transform.localScale;
        spawnedObject.transform.localScale = new Vector3(currentLocalScale.x, scale, currentLocalScale.z);

        return spawnedObject;
    }
}
