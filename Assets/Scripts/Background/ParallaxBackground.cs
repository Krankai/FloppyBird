using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public enum Direction
    {
        Left = -1,
        Right = 1,
    }

    [Header("Overall Settings")]
    [SerializeField] private Direction _movingDirection = Direction.Left;

    [SerializeField] private float _overallSpeed = 5f;

    [Header("Layer Settings")]
    [SerializeField] private float[] _layerParallaxEffect;

    [SerializeField] private SpriteRenderer[] _layerSprites;

    private int _totalEffectiveLayers;        // note: only count layers with enough information (i.e. parallax effect and sprite)

    private float[] _layerStartPositionX;

    private float[] _layerBoundSizeX;

    private bool _isDisable = false;

    private Vector3 _triviaRightVector = Vector3.right;

    public void FakePause()
    {
        // note: use only when lose
        _isDisable = true;
    }

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
        if (_isDisable) return;

        for (int i = 0; i < _totalEffectiveLayers; ++i)
        {
            var layerSprite = _layerSprites[i];

            MoveLayer(layerSprite, _layerParallaxEffect[i]);
            RepositionLayer(layerSprite, _layerStartPositionX[i], _layerBoundSizeX[i]);
        }
    }

    private void MoveLayer(SpriteRenderer layerSprite, float parallaxEffect)
    {
        layerSprite.transform.position += _triviaRightVector * (int)_movingDirection * _overallSpeed * parallaxEffect * Time.deltaTime;
    }

    private void RepositionLayer(SpriteRenderer layerSprite, float startPositionX, float boundSizeX)
    {
        if (Mathf.Abs(layerSprite.transform.position.x - startPositionX) >= boundSizeX)
        {
            layerSprite.transform.position = new Vector2(startPositionX, layerSprite.transform.position.y);
        }
    }
}
