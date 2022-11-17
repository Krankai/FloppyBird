using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance = null;

    [Header("Settings")]
    [SerializeField] private Transform _playerTransform;

    [SerializeField] private int _maximumScore = 10;

    [SerializeField] private float _deadZoneLimit = -12f;
    
    [SerializeField] private int _startDelay = 4;

    [field:Header("Status")]
    [field:SerializeField] public int CurrentScore { get; private set; }

    [field:SerializeField] public bool IsFinish { get; private set; }

    [field:SerializeField] public bool IsPaused { get; private set; }

    [Header("Events")]
    [SerializeField] private UnityEvent _gameOverFailEvents;

    [SerializeField] private UnityEvent _gameOverSucceedEvents;

    [SerializeField] private UnityEvent _pauseEvents;

    public float GetDeadZoneLimit() => _deadZoneLimit;

    public int GetStartDelay() => _startDelay;

    public void UpdateScore()
    {
        UIManager.Instance.OnUpdateScore(++CurrentScore);
        AudioManager.Instance.OnPlayScoreSound();
    }

    public void GameOver(bool isWinner)
    {
        // Debug.Log(isWinner ? "Winner" : "Loser");
        IsFinish = true;

        if (isWinner)
        {
            _gameOverSucceedEvents.Invoke();
        }
        else
        {
            _gameOverFailEvents.Invoke();
        }
    }

    public void TogglePauseState()
    {
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0 : 1;
        _pauseEvents.Invoke();
    }

    public void MoveToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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

        if (_playerTransform == null)
        {
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        }
    }

    private void Start()
    {
        CurrentScore = 0;
        IsFinish = false;
        IsPaused = false;
    }

    private void Update()
    {
        if (IsFinish) return;

        if (CurrentScore >= _maximumScore)
        {
            GameOver(true);
            return;
        }

        if (_playerTransform.position.y <= _deadZoneLimit)
        {
            GameOver(false);
            return;
        }
    }
}
