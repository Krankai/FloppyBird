using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Overall Settings")]
    [SerializeField] private Direction _movingDirection = Direction.Left;

    [SerializeField] private float _overallSpeed = 5f;

    [Header("Layer Settings")]
    [SerializeField] private SpriteRenderer _baseLayerSprite;

    [SerializeField] private float[] _layerParallaxEffect;

    [SerializeField] private SpriteRenderer[] _layerSprites;

    private int _totalEffectiveLayers;        // note: only count layers with enough information (i.e. parallax effect and sprite)

    private float[] _layerStartPositionX;

    private float[] _layerBoundSizeX;

    private void Start()
    {
        _totalEffectiveLayers = Mathf.Min(_layerParallaxEffect.Length, _layerSprites.Length);

        _layerStartPositionX = new float[_totalEffectiveLayers];
        _layerBoundSizeX = new float[_totalEffectiveLayers];
        
        for (int i = 0; i < _totalEffectiveLayers; ++i)
        {
            _layerStartPositionX[i] = 0f;
            _layerBoundSizeX[i] = _layerSprites[i].bounds.size.x;        // including scale modifier
        }
    }

    private void Update()
    {
        for (int i = 0; i < _totalEffectiveLayers; ++i)
        {
            var layerSprite = _layerSprites[i];

            MoveLayer(layerSprite, _layerParallaxEffect[i]);
            RepositionLayer(layerSprite, _layerStartPositionX[i], _layerBoundSizeX[i]);
        }
    }

    private void MoveLayer(SpriteRenderer layerSprite, float parallaxEffect)
    {
        layerSprite.transform.position += Vector3.right * (int)_movingDirection * _overallSpeed * parallaxEffect * Time.deltaTime;
    }

    private void RepositionLayer(SpriteRenderer layerSprite, float startPositionX, float boundSizeX)
    {
        if (Mathf.Abs(layerSprite.transform.position.x - startPositionX) >= boundSizeX)
        {
            layerSprite.transform.position = new Vector2(startPositionX, layerSprite.transform.position.y);
        }
    }
}

public enum Direction
{
    Left = -1,
    Right = 1,
}
