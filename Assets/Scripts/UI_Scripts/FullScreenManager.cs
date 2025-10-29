using UnityEngine;
using UnityEngine.UI;

public class FullScreenManager : MonoBehaviour
{
    [SerializeField] private Toggle fullScreenToggle;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    
        // Daha önce kaydedilmiş durum varsa yükle
        bool isFullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
    
        // Toggle değiştiğinde olayı bağla
        fullScreenToggle.onValueChanged.AddListener(OnFullScreenToggle);
    }
    
    private void OnFullScreenToggle(bool isOn)
    {
        Screen.fullScreen = isOn;
        PlayerPrefs.SetInt("FullScreen", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}