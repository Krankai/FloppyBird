using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Singleton
    //public static PoolManager Instance = null;

    [SerializeField] private GameObject _poolObject;

    [SerializeField] private int _poolCount = 16;

    [SerializeField] private Transform _parentTransform;

    private GameObject[] _listPoolObjects;

    private Vector3 _spawnPosition = Vector3.zero;

    public GameObject GetPoolObject()
    {
        foreach (var poolObject in _listPoolObjects)
        {
            if (poolObject.activeInHierarchy) continue;

            ActivatePoolObject(poolObject);
            return poolObject;
        }

        Debug.LogWarning("Exceeding pool count. Please increase!!!");
        return null;        // exceeding pool count
    }

    public void ReturnPoolObject(GameObject poolObject)
    {
        DeactivatePoolObject(poolObject);
    }

    private void Awake()
    {
        // if (Instance == null)
        // {
        //     Instance = this;
        // }
        // else
        // {
        //     Destroy(this.gameObject);
        // }

        _listPoolObjects = new GameObject[_poolCount];
    }

    private void Start()
    {
        InitPoolObjects();
    }

    private void InitPoolObjects()
    {
        for (int i = 0; i < _poolCount; ++i)
        {
            _listPoolObjects[i] = Instantiate(_poolObject, _spawnPosition, _poolObject.transform.rotation, _parentTransform);

            var poolObjectCollider = _listPoolObjects[i].GetComponent<BoxCollider2D>();
            CustomPhysicsEngine.Instance.UpdateCollider(poolObjectCollider);

            DeactivatePoolObject(_listPoolObjects[i]);
        }
    }

    private void DeactivatePoolObject(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }

    private void ActivatePoolObject(GameObject poolObject)
    {
        poolObject.SetActive(true);
    }
}
