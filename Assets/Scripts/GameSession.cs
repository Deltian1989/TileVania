using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;

    [SerializeField] int coins = 0;

    [SerializeField] Text livesText;

    [SerializeField] Text scoreText;

    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = coins.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    public void AddCoins(int coinsToAdd)
    {
        coins += coinsToAdd;
        scoreText.text = coins.ToString();
    }

    private void TakeLife()
    {
        --playerLives;

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex);

        livesText.text = playerLives.ToString();
    }

    private void ResetGameSession()
    {
        playerLives = 3;
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}
