using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager Instance = null;

    [SerializeField] private Image _pausedPanel;

    [SerializeField] private Image _gameOverPanel;

    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private TextMeshProUGUI _countdownText;

    [SerializeField] private TextMeshProUGUI _winnerText;

    [SerializeField] private TextMeshProUGUI _loserText;

    private WaitForSeconds _triviaWaitTimer1Sec = new WaitForSeconds(1);

    public void OnTogglePause()
    {
        if (GameManager.Instance.IsFinish) return;

        bool currentState = _pausedPanel.gameObject.activeInHierarchy;
        _pausedPanel.gameObject.SetActive(!currentState);
    }

    public void OnUpdateScore(int newScore)
    {
        _scoreText.SetText(newScore.ToString("00"));
    }

    public void OnGameOverWin()
    {
        _gameOverPanel.gameObject.SetActive(true);
        _winnerText.gameObject.SetActive(true);
        _loserText.gameObject.SetActive(false);
    }

    public void OnGameOverLose()
    {
        _gameOverPanel.gameObject.SetActive(true);
        _winnerText.gameObject.SetActive(false);
        _loserText.gameObject.SetActive(true);
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
    }

    private void Start()
    {
        _pausedPanel.gameObject.SetActive(false);
        _gameOverPanel.gameObject.SetActive(false);
        StartCoroutine(CountdownStartRoutine(GameManager.Instance.GetStartDelay()));
    }

    private IEnumerator CountdownStartRoutine(int startValue)
    {
        --startValue;
        while (startValue > 0)
        {
            _countdownText.SetText(startValue.ToString());
            AudioManager.Instance.OnPlayCountdownSound();

            yield return _triviaWaitTimer1Sec;
            --startValue;
        }

        _countdownText.SetText("Start");
        AudioManager.Instance.OnPlayStartSound();

        yield return _triviaWaitTimer1Sec;

        _countdownText.gameObject.SetActive(false);
    }
}
