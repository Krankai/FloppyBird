using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfRange : MonoBehaviour
{
    [SerializeField] private float _destroyRangeX = -30f;

    private PoolManager _poolManager;

    private void Awake()
    {
        _poolManager = GameObject.Find("SpawnManager").GetComponent<PoolManager>();
    }

    private void Update()
    {
        if (this.transform.position.x <= _destroyRangeX)
        {
            // Destroy(this.gameObject);
            if (_poolManager != null)
            {
                _poolManager.ReturnPoolObject(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
