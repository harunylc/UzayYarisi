using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFlowManager : MonoBehaviour
{
    //KAZANMA KOŞULUNDA LoadUpgradeScene(); Çağıralacak
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public List<String> scenes = new List<string>{"Dünya","Mars","Venüs","Satürn","Neptün"};
    private List<string> remainingScenes = new List<string>();
    
    private string upgradeScene="UpgradeLobbyScene";
    private string mainMenuScene="MainMenuScene";

    public void StartGame()
    {
        remainingScenes=new List<string>(scenes);
        ShuffleList(remainingScenes);
        LoadUpgradeScene();
    }

    public void LoadUpgradeScene()
    {
        SceneManager.LoadScene(upgradeScene);
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
    
    public void LoadNextLevelOrEndGame()
    {
        if (remainingScenes.Count > 0)
        {
            // Sıradaki rastgele leveli al
            string nextLevel = remainingScenes[0];
            remainingScenes.RemoveAt(0);

            // Leveli yükle
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            // Tüm leveller tamamlandı, Ana Menü'ye dön
            Debug.Log("Tüm Leveller Tamamlandı! Ana Menü'ye dönülüyor.");
            SceneManager.LoadScene(mainMenuScene); 
        
            // Veya GameFlowManager'ı yok et (isteğe bağlı)
            Destroy(gameObject); 
        }
    }
    
    
}
