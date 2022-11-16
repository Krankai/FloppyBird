using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance = null;

    [field:SerializeField] public int Score { get; private set; }

    [SerializeField] private int _maximumScore = 10;

    public void UpdateScore() => ++Score;

    public void GameOver(bool isWinner)
    {
        Debug.Log(isWinner ? "Winner" : "Loser");
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

    private void Update()
    {
        if (Score >= _maximumScore)
        {
            GameOver(true);
        }
    }
}
