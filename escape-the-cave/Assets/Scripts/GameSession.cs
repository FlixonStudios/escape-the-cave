using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int playerScore = 0;
    [SerializeField] TextMeshProUGUI livesCount;
    [SerializeField] TextMeshProUGUI scoreCount;

    

    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if(numGameSessions>1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        UpdatePlayerLives();
        UpdatePlayerScore();
    }
    public void ProcessPlayerDeath()
    {
        if (playerLives>1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }
    public void AddToScore(int scoreToAdd)
    {
        playerScore += scoreToAdd;
        UpdatePlayerScore();
    }


    private void TakeLife()
    {
        playerLives--;
        UpdatePlayerLives();

        StartCoroutine(DelayLoadScene());
        
    }
    private void UpdatePlayerLives()
    {
        livesCount.text = playerLives.ToString();
    }
    private void UpdatePlayerScore()
    {
        scoreCount.text = playerScore.ToString();
    }
    private void ResetGameSession()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    IEnumerator DelayLoadScene()
    {
        yield return new WaitForSeconds(2f);
        LoadSameScene();
    }
    private void LoadSameScene()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

}
