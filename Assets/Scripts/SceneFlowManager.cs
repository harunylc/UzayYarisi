using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    public static SceneFlowManager Instance;

    
    public List<SceneData> AllSceneDataList;
    
   
    public string NextLevelSceneName { get; private set; } 
   
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
        PrepareForUpgradeScene();
    }

  
    public void LevelCompleted()
    {
        if (remainingScenes.Count > 0)
        {
             PrepareForUpgradeScene();
        }
        else
        {
            LoadMainMenu();
        }
    }
    
    public void PrepareForUpgradeScene()
    {
        if (remainingScenes.Count > 0)
        {
            NextLevelSceneName = remainingScenes[0]; 
        }
        else
        {
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
    
    public SceneData GetNextLevelData()
    {
        if (string.IsNullOrEmpty(NextLevelSceneName))
        {
            return null;
        }
        return AllSceneDataList.Find(data => data.SceneName == NextLevelSceneName);
    }

    private void LoadSceneWithFade(string sceneName)
    {
        if (Fade_Manager.Instance != null)
        {
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