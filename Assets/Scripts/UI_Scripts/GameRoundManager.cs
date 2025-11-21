using UnityEngine;
using System.Collections; 
using UnityEngine.SceneManagement;

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
        
        // EndGame'i başlatan Coroutine'i çağır
        StartCoroutine(EndGameRoutine(winner));

        /*
        if (Fade_Manager.Instance != null)
            Fade_Manager.Instance.StartFadeOutAndLoadScene("MainMenuScene");
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenuScene");
            */
    }
    
    //sonradan eklendi <<<<<<<<<<<<<<<<<<
    private IEnumerator EndGameRoutine(int winner)
    {
        // 1. Oyunu durdur/yavaşlat (Time.timeScale = 0 bazen Coroutine'leri de durdurabilir,
        // bu yüzden çok küçük bir değer kullanmak daha güvenlidir)
        Time.timeScale = 0.001f;

        // 2. Sahnedeki doğru "Kazandı" panelini bul
        string panelTag = (winner == 1) ? "P1_WinPanel" : "P2_WinPanel";
        GameObject winPanel = GameObject.FindWithTag(panelTag);

        if (winPanel != null)
        {
            // 3. Paneli aktif et
            winPanel.SetActive(true);

            // 4. Gerçek zaman kullanarak 3 saniye bekle (Time.timeScale'den etkilenmez)
            yield return new WaitForSecondsRealtime(6f);
            
            // 5. Paneli tekrar kapat
            winPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning(panelTag + " etiketli bir kazanan paneli bu sahnede bulunamadı!");
            // Panel bulunamazsa bile, yine de ana menüye dönmeden önce bekleyelim
            yield return new WaitForSecondsRealtime(6f);
        }

        // 6. Zamanı normale döndür
        Time.timeScale = 1f;
        
        // 7. Fade ile ana menüye geçiş yap
        if (Fade_Manager.Instance != null)
            Fade_Manager.Instance.StartFadeOutAndLoadScene("MainMenuScene");
        else
            SceneManager.LoadScene("MainMenuScene");
    }
    //>>>>>>>>>>>>>>>>>>>>

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
        }
    }
}
