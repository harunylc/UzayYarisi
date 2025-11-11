using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    // Harita verilerini tutan liste (Inspector'da doldurulmalı)
    public List<SceneData> AllSceneDataList;
    
    // YENİ: Upgrade ekranında gösterilecek bir sonraki levelin adı
    public string NextLevelSceneName { get; private set; } 
    // Mevcut level adını tutar (Şimdilik kullanılmıyor, ama faydalıdır)
    public string CurrentSceneName { get; private set; }
    
    public List<string> scenes = new List<string> { "Dünya", "Mars", "Merkür", "Satürn", "Neptün" };
    private List<string> remainingScenes = new List<string>();

    private string upgradeScene = "UpgradeLobbyScene";
    private string mainMenuScene = "MainMenuScene";

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
        CurrentSceneName = scene.name;

        if (scene.name == mainMenuScene)
        {
            ResetSceneOrder();
        }
    }

    public void StartGame()
    {
        ResetSceneOrder();
        PrepareForUpgradeScene(); // İlk upgrade ekranını yüklemeden önce sonraki leveli kaydeder.
    }

    // YENİ: Leveli tamamladığında upgrade ekranına geçişi hazırlar
    public void LevelCompleted()
    {
        // remainingScenes'den biten leveli kaldırdıktan sonra çağrılmalı
        // veya LoadNextLevelOrEndGame'in çağrılmasıyla bu seviyeden çıkılır.
        
        // Eğer level bittiyse ve hala level varsa, upgrade ekranına hazırlar
        if (remainingScenes.Count > 0)
        {
             PrepareForUpgradeScene();
        }
        else
        {
            // Tüm leveller bitti
            LoadMainMenu();
        }
    }
    
    // YENİ METOT: Upgrade lobisine gitmeden önce hangi levelin geleceğini ayarlar ve lobiyi yükler.
    public void PrepareForUpgradeScene()
    {
        if (remainingScenes.Count > 0)
        {
            // Bir sonraki level adını al ve NextLevelSceneName'e kaydet (remainingScenes'den ÇIKARMA!)
            NextLevelSceneName = remainingScenes[0]; 
        }
        else
        {
            // Tüm leveller bitti, ana menüye dönülecek bir durum için NextLevelSceneName'i temizle
            NextLevelSceneName = null; 
        }
        
        LoadUpgradeScene();
    }
    
    private void ResetSceneOrder()
    {
        remainingScenes = new List<string>(scenes);
        ShuffleList(remainingScenes);
    }

    public void LoadUpgradeScene()
    {
        LoadSceneWithFade(upgradeScene);
    }
    
    private void LoadMainMenu()
    {
        LoadSceneWithFade(mainMenuScene);
    }
    
    public void LoadNextLevelOrEndGame()
    {
        if (remainingScenes.Count > 0)
        {
            string nextLevel = remainingScenes[0];
            remainingScenes.RemoveAt(0); // Leveli listeden çıkar

            LoadSceneWithFade(nextLevel);
        }
        else
        {
            Debug.Log("Tüm Leveller Tamamlandı! Ana Menü'ye dönülüyor.");
            LoadMainMenu();
        }
    }
    
    // YENİ METOT: Upgrade ekranı için bir sonraki level verisini döndürür.
    public SceneData GetNextLevelData()
    {
        if (string.IsNullOrEmpty(NextLevelSceneName))
        {
            // Level kalmamışsa (oyun bittiyse)
            return null;
        }
        
        // Kaydedilen NextLevelSceneName'i kullanarak listeyi ara
        return AllSceneDataList.Find(data => data.SceneName == NextLevelSceneName);
    }

    private void LoadSceneWithFade(string sceneName)
    {
        if (Fade_Manager.Instance != null)
        {
            // Varsayım: Fade_Manager.Instance.FadeOutThen mevcut ve çalışıyor.
            // Bu kısmı projenizdeki Fade Manager'a göre ayarlayın.
            // Örnekteki orijinal kodunuzu kullanıyorum:
            // Fade_Manager.Instance.StartFadeOutAndLoadScene(sceneName); // (Sizin orijinal kodunuz)
            
            // Eğer `FadeOutThen` kullanılıyorsa:
            // StartCoroutine(Fade_Manager.Instance.FadeOutThen(() => { SceneManager.LoadScene(sceneName); }));
            
            // Eğer orijinal kodunuzdaki gibi tek bir metot varsa:
             Fade_Manager.Instance.StartFadeOutAndLoadScene(sceneName); 
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}