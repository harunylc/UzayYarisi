using UnityEngine;
using System;
using System.Collections; 
using UnityEngine.SceneManagement;

public class GameRoundManager : MonoBehaviour
{
    public static event Action<int> OnGameOver;
    
    public static GameRoundManager Instance;
    
    [Header("Puan Ayarları")]
    public int p1Score = 0;
    public int p2Score = 0;
    public int maxScore = 3;
    

    private bool roundFinished = false;
    private bool gameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public bool IsGameOver => gameOver;

    public void PlayerReachedFlag(int playerNumber)
    {
        if (gameOver) return;
        if (roundFinished) return;

        roundFinished = true;

        if (playerNumber == 1)
            p1Score++;
        else if (playerNumber == 2)
            p2Score++;

        if (p1Score >= maxScore)
        {
            EndGame(1);

        }
        else if (p2Score >= maxScore)
        {
            EndGame(2);
            
        }
        else
        {
            Invoke(nameof(GoToUpgradeScene), 0.01f);
        }
    }

    private void GoToUpgradeScene()
    {
        if (gameOver) return;
        roundFinished = false;
        SceneFlowManager.Instance.LoadUpgradeScene();
    }

    private void EndGame(int winner)
    {
        gameOver = true;
        OnGameOver?.Invoke(winner);
        StartCoroutine(ReturnToMenuAfterDelay(4f));
    }
    
    private IEnumerator ReturnToMenuAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        if (Fade_Manager.Instance != null)
            Fade_Manager.Instance.StartFadeOutAndLoadScene("MainMenuScene");
        else
            SceneManager.LoadScene("MainMenuScene");
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene")
        {
            p1Score = 0;
            p2Score = 0;
            roundFinished = false;
            gameOver = false;
            
            //<<<<<<<<
            CarSelectionLocker.UnlockSelections();
            //>>>>>>>>
        }
    }
}
