using UnityEngine;

public class GameRoundManager : MonoBehaviour
{
    public static GameRoundManager Instance;
    
    public static int LastWinner = 0; 

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
            EndGame(1);
        else if (p2Score >= maxScore)
            EndGame(2);
        else
            Invoke(nameof(GoToUpgradeScene), 2f);
    }

    private void GoToUpgradeScene()
    {
        if (gameOver) return;
        roundFinished = false;

        if (Fade_Manager.Instance != null)
            Fade_Manager.Instance.StartFadeOutAndLoadScene("UpgradeLobbyScene");
        else if (SceneFlowManager.Instance != null)
            SceneFlowManager.Instance.LoadUpgradeScene();
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("UpgradeLobbyScene");
    }

    private void EndGame(int winner)
    {
        gameOver = true;
        Debug.Log($"🎉 Oyuncu {winner} oyunu kazandı! Ana menüye dönülüyor...");
        
        LastWinner = winner;

        if (Fade_Manager.Instance != null)
            Fade_Manager.Instance.StartFadeOutAndLoadScene("MainMenuScene");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene")
        {
            p1Score = 0;
            p2Score = 0;
            roundFinished = false;
            gameOver = false;
        }
    }
}
