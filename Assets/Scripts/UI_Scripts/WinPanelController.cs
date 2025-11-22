using UnityEngine;

public class WinPanelController : MonoBehaviour
{
    public GameObject p1WinPanel;
    public GameObject p2WinPanel;
    private float displayTime = 3f;

    void OnEnable()
    {
        GameRoundManager.OnGameOver += ShowWinner;
    }

    void OnDisable()
    {
        GameRoundManager.OnGameOver -= ShowWinner;
    }

    private void ShowWinner(int winner)
    {
        Time.timeScale = 0.001f;

        if (winner == 1 && p1WinPanel != null)
        {
            p1WinPanel.SetActive(true);
            Invoke(nameof(HidePanels), displayTime / Time.timeScale);
        }
        else if (winner == 2 && p2WinPanel != null)
        {
            p2WinPanel.SetActive(true);
            Invoke(nameof(HidePanels), displayTime / Time.timeScale);
        }
    }

    private void HidePanels()
    {
        if(p1WinPanel != null) p1WinPanel.SetActive(false);
        if(p2WinPanel != null) p2WinPanel.SetActive(false);
    }
}