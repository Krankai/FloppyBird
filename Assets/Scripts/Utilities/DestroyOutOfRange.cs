using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfRange : MonoBehaviour
{
    [SerializeField] private float _destroyRangeX = -30f;

    private void Update()
    {
        if (this.transform.position.x <= _destroyRangeX)
        {
            Destroy(this.gameObject);
        }
    }
}
