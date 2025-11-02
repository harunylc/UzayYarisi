// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// public class SceneFlowManager : MonoBehaviour
// {
//     public static SceneFlowManager Instance; // yunus
//     private void Awake()//yunus
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//             DontDestroyOnLoad(gameObject);
//         }
//         else
//         {
//             Destroy(gameObject);
//             return;
//         }
//     }
//     
//     //KAZANMA KOŞULUNDA LoadUpgradeScene(); Çağıralacak
//     
//     public List<String> scenes = new List<string> { "Dünya", "Mars", "Merkür", "Satürn", "Neptün" };
//     private List<string> remainingScenes = new List<string>();
//
//     private string upgradeScene = "UpgradeLobbyScene";
//     private string mainMenuScene = "MainMenuScene";
//
//     public void StartGame()
//     {
//         
//         remainingScenes = new List<string>(scenes);
//         ShuffleList(remainingScenes);
//         
//         LoadUpgradeScene();
//     }
//
//     public void LoadUpgradeScene()
//     {
//         if (Fade_Manager.Instance != null)//yunus
//         {
//             Fade_Manager.Instance.StartFadeOutAndLoadScene(upgradeScene);
//         }
//         else
//         {
//             SceneManager.LoadScene(upgradeScene);
//         }
//         
//         // SceneManager.LoadScene(upgradeScene);
//     }
//     private void ShuffleList<T>(List<T> list)
//     {
//         int n = list.Count;
//         while (n > 1)
//         {
//             n--;
//             int k = UnityEngine.Random.Range(0, n + 1);
//             T value = list[k];
//             list[k] = list[n];
//             list[n] = value;
//         }
//     }
//
//     public void LoadNextLevelOrEndGame()
//     {
//         if (remainingScenes.Count > 0)
//         {
//             // Sıradaki rastgele leveli al
//             string nextLevel = remainingScenes[0];
//             remainingScenes.RemoveAt(0);
//
//             // Leveli yükle
//             // SceneManager.LoadScene(nextLevel);
//             Fade_Manager.Instance.StartFadeOutAndLoadScene(nextLevel);
//
//         }
//         else
//         {
//             // Tüm leveller tamamlandı, Ana Menü'ye dön
//             Debug.Log("Tüm Leveller Tamamlandı! Ana Menü'ye dönülüyor.");
//             // SceneManager.LoadScene(mainMenuScene);
//             
//             if (Fade_Manager.Instance != null)//yunus
//             {
//                 Fade_Manager.Instance.StartFadeOutAndLoadScene(mainMenuScene);
//             }
//             else
//             {
//                 SceneManager.LoadScene(mainMenuScene);
//             }
//
//         }
//     }
// }
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

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

    // 🎯 Ana menüye dönüldüğünde sistem sıfırlansın
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuScene)
        {
            Debug.Log("🔄 Ana menüye dönüldü — SceneFlowManager resetleniyor...");
            ResetSceneOrder();
        }
    }

    // ✅ Oyun ilk kez veya tekrar başladığında çağrılır
    public void StartGame()
    {
        ResetSceneOrder();
        LoadUpgradeScene();
    }

    // ✅ Harita listesini sıfırla ve karıştır
    private void ResetSceneOrder()
    {
        remainingScenes = new List<string>(scenes);
        ShuffleList(remainingScenes);
        Debug.Log($"🎲 Haritalar karıştırıldı: {string.Join(", ", remainingScenes)}");
    }

    public void LoadUpgradeScene()
    {
        // Artık fade burada değil — sadece sahne ismini döndürüyoruz
        LoadSceneWithFade(upgradeScene);
    }

    public void LoadNextLevelOrEndGame()
    {
        if (remainingScenes.Count > 0)
        {
            string nextLevel = remainingScenes[0];
            remainingScenes.RemoveAt(0);

            Debug.Log($"🌍 Sıradaki harita: {nextLevel}");
            LoadSceneWithFade(nextLevel);
        }
        else
        {
            Debug.Log("🟢 Tüm haritalar oynandı — upgrade ekranına dönülüyor (oyun bitmedi).");
            ResetSceneOrder(); // ✅ tekrar karıştır, oyun döngüsünü sıfırla
            LoadSceneWithFade(upgradeScene);
        }
    }

    private void LoadSceneWithFade(string sceneName)
    {
        // Artık fade kontrolü buradan sadece 1 kez yapılacak
        if (Fade_Manager.Instance != null)
        {
            StartCoroutine(Fade_Manager.Instance.FadeOutThen(() =>
            {
                SceneManager.LoadScene(sceneName);
            }));
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

