using UnityEngine;

public class GameFlowConnector : MonoBehaviour
{
    public static GameFlowConnector Instance;

    private bool p1Ready = false;
    private bool p2Ready = false;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayerReady(int playerNumber)
    {
        
        if (GameRoundManager.Instance != null && GameRoundManager.Instance.IsGameOver) 
        {
            return;
        }
        
        if (playerNumber == 1)
            p1Ready = true;
        else if (playerNumber == 2)
            p2Ready = true;


        if (p1Ready && p2Ready)
        {
            StartNextRound();
        }
    }

    private void StartNextRound()
    {
        if (GameRoundManager.Instance != null && GameRoundManager.Instance.IsGameOver)
        {
            return;
        }
        
        p1Ready = false;
        p2Ready = false;
        
        if (SceneFlowManager.Instance != null)
        {
            SceneFlowManager.Instance.LoadNextLevelOrEndGame();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("simplescene");
        }
    }
}