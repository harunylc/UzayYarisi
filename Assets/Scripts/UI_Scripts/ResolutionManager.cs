using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    // 🔹 Kalıcı ayarları tutan tek global değişkenler
    private static int currentResolutionIndex = -1;
    private static bool currentFullScreen = true;
    private static bool initialized = false;

    private void Awake()
    {
        // Aynı anda birden fazla ResolutionManager varsa yok et
        var managers = FindObjectsOfType<ResolutionManager>();
        if (managers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Ayar yöneticisi hep yaşasın

        // Sadece ilk sahnede PlayerPrefs'ten oku
        if (!initialized)
        {
            currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
            currentFullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;

            ApplyResolution(currentResolutionIndex, currentFullScreen);
            initialized = true;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        ConnectUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Yeni sahnede UI yeniden doğduysa bağla
        ConnectUI();
    }

    // 🔹 Sahnedeki yeni UI elemanlarını bulup güncelle
    private void ConnectUI()
    {
        if (resolutionDropdown == null)
            resolutionDropdown = FindObjectOfType<Dropdown>(true);

        if (fullScreenToggle == null)
            fullScreenToggle = FindObjectOfType<Toggle>(true);

        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
            resolutionDropdown.value = currentResolutionIndex;
        }

        if (fullScreenToggle != null)
        {
            fullScreenToggle.onValueChanged.RemoveAllListeners();
            fullScreenToggle.onValueChanged.AddListener(OnFullScreenChanged);
            fullScreenToggle.isOn = currentFullScreen;
        }
    }

    private void OnResolutionChanged(int index)
    {
        currentResolutionIndex = index;
        ApplyResolution(index, currentFullScreen);

        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    private void OnFullScreenChanged(bool isFull)
    {
        currentFullScreen = isFull;
        ApplyResolution(currentResolutionIndex, isFull);

        PlayerPrefs.SetInt("FullScreen", isFull ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyResolution(int index, bool fullScreen)
    {
        int width = 1920;
        int height = 1080;

        switch (index)
        {
            case 1: width = 1600; height = 900; break;
            case 2: width = 1366; height = 768; break;
            case 3: width = 1280; height = 720; break;
        }

        Screen.SetResolution(width, height, fullScreen);
        Debug.Log($"[ResolutionManager] Resolution applied: {width}x{height}, fullscreen={fullScreen}");
    }
}
