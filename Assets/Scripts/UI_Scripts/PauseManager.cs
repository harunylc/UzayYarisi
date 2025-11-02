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
        // 🔹 Pause menüsünü aç/kapat
        if (InputManager.instance.PauseOpenClose)
        {
            if (!isPaused)
                Pause();
            else if (isPaused && !_settingsPanel.activeSelf)
                Unpause();
        }

        // 🔹 B tuşu (Cancel) geri adım mantığı
        if (InputManager.instance.CancelPressed())
        {
            if (_settingsPanel.activeSelf)
            {
                // Ayarlardan pause menüsüne dön
                _settingsPanel.SetActive(false);
                _pausePanel.SetActive(true);

                // ✅ Pause menüsü geri geldiğinde buton tekrar seçili olsun
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

        // ✅ Pause açılır açılmaz ilk buton seçili hale gelsin
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

        // ✅ Ayarlarda seçili buton kalmasın (aksi halde UI karışır)
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(GameObject.Find("SoundSlider"));
    }
    //
    // public void ExitGame()
    // {
    //     Debug.Log("Oyundan çıkılıyor...");
    //     Application.Quit();
    // }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Oyunu tekrar normale al
        InputManager.instance.SwitchToGame(); // Action map’i resetle
        SceneManager.LoadScene("MainMenuScene"); // Ana menü sahnesine dön
    }
}