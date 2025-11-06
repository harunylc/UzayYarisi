using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Üstte ekli değilse ekle

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private Button _firstSelectedButton;

    private bool isPaused;

    private void Start()
    {
        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.instance.PauseOpenClose)
        {
            if (!isPaused)
                Pause();
            else if (isPaused && !_settingsPanel.activeSelf)
                Unpause();
        }

        if (InputManager.instance.CancelPressed())
        {
            if (_settingsPanel.activeSelf)
            {
                _settingsPanel.SetActive(false);
                _pausePanel.SetActive(true);

                EventSystem.current.SetSelectedGameObject(_firstSelectedButton.gameObject);
            }
            else if (isPaused)
            {
                Unpause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        _pausePanel.SetActive(true);
        InputManager.instance.SwitchToUI();

        EventSystem.current.SetSelectedGameObject(_firstSelectedButton.gameObject);
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        _pausePanel.SetActive(false);
        _settingsPanel.SetActive(false);
        InputManager.instance.SwitchToGame();

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
        _pausePanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("SoundSlider"));
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        InputManager.instance.SwitchToGame(); 
        SceneManager.LoadScene("MainMenuScene"); 
    }
}