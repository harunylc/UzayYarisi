using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private static ResolutionManager instance;

    private static int currentResolutionIndex = 0;
    private static bool currentFullScreen = true;
    private static bool initialized = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

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
        StartCoroutine(DelayedUIUpdate());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(DelayedUIUpdate());
    }

    private IEnumerator DelayedUIUpdate()
    {
        float timeout = 5f;
        float elapsed = 0f;

        while (elapsed < timeout)
        {
            // UI var mı kontrol et
            var dropdownInScene = FindObjectsOfType<Dropdown>(true);
            var toggleInScene = FindObjectsOfType<Toggle>(true);

            if (dropdownInScene.Length > 0 || toggleInScene.Length > 0)
            {
                ConnectUI();
                yield break; 
            }

            yield return new WaitForSeconds(0.5f);
            elapsed += 0.5f;
        }

    }


    private void ConnectUI()
    {
        resolutionDropdown = null;
        fullScreenToggle = null;

        foreach (var drop in FindObjectsOfType<Dropdown>(true))
        {
            if (drop.name == "ResolutionDropDown")
            {
                resolutionDropdown = drop;
                break;
            }
        }

        foreach (var tog in FindObjectsOfType<Toggle>(true))
        {
            if (tog.name == "FullScreenToggle")
            {
                fullScreenToggle = tog;
                break;
            }
        }
            
        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.RemoveAllListeners();
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }

        if (fullScreenToggle != null)
        {
            fullScreenToggle.onValueChanged.RemoveAllListeners();
            fullScreenToggle.isOn = currentFullScreen;
            fullScreenToggle.onValueChanged.AddListener(OnFullScreenChanged);
        }
    }

    private void OnResolutionChanged(int index)
    {
        if (index == currentResolutionIndex) return;
        currentResolutionIndex = index;
        ApplyResolution(index, currentFullScreen);
        SavePrefs();
    }

    private void OnFullScreenChanged(bool isFull)
    {
        if (isFull == currentFullScreen) return;
        currentFullScreen = isFull;
        ApplyResolution(currentResolutionIndex, currentFullScreen);
        SavePrefs();
    }

    private void SavePrefs()
    {
        PlayerPrefs.SetInt("ResolutionIndex", currentResolutionIndex);
        PlayerPrefs.SetInt("FullScreen", currentFullScreen ? 1 : 0);
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
}
