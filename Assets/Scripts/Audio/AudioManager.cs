using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singleton
    public static AudioManager Instance = null;

    [SerializeField] private AudioSource _buttonClickSound;

    [SerializeField] private AudioSource _countdownSound;

    [SerializeField] private AudioSource _startSound;

    [SerializeField] private AudioSource _jumpSound;

    [SerializeField] private AudioSource _scoreSound;

    [SerializeField] private AudioSource _winnerSound;

    [SerializeField] private AudioSource _loserSound;

    public void OnPlayButtonClickSound()
    {
        _buttonClickSound.Play();
    }

    public void OnPlayCountdownSound()
    {
        _countdownSound.Play();
    }

    public void OnPlayStartSound()
    {
        _startSound.Play();
    }

    public void OnPlayJumpSound()
    {
        _jumpSound.Play();
    }

    public void OnPlayScoreSound()
    {
        _scoreSound.Play();
    }

    public void OnPlayWinnerSound()
    {
        _winnerSound.Play();
    }

    public void OnPlayLoserSound()
    {
        _loserSound.Play();
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
}
