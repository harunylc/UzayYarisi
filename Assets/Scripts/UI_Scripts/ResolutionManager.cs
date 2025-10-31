using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private void Start()
    {
        // 🔹 Kaydedilmiş çözünürlük ve fullscreen durumunu yükle
        int savedResolution = PlayerPrefs.GetInt("ResolutionIndex", 0);
        bool isFullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;

        // 🔹 Uygula
        ApplyResolution(savedResolution, isFullScreen);

        // 🔹 UI senkronizasyonu
        resolutionDropdown.value = savedResolution;
        fullScreenToggle.isOn = isFullScreen;

        // 🔹 Listener ekle
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        fullScreenToggle.onValueChanged.AddListener(OnFullScreenChanged);
    }

    public void OnResolutionChanged(int index)
    {
        bool isFullScreen = fullScreenToggle.isOn;
        ApplyResolution(index, isFullScreen);

        // 🔹 PlayerPrefs’e kaydet
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void OnFullScreenChanged(bool isFullScreen)
    {
        int index = resolutionDropdown.value;
        ApplyResolution(index, isFullScreen);

        // 🔹 PlayerPrefs’e kaydet
        PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyResolution(int index, bool fullScreen)
    {
        int width = 1920;
        int height = 1080;

        if (index == 1)
        {
            width = 1280;
            height = 720;
        }

        Screen.SetResolution(width, height, fullScreen);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("ResolutionIndex");
        PlayerPrefs.DeleteKey("FullScreen");
    }
}