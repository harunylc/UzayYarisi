using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UI_Controller : MonoBehaviour
{
    [Header("Paneller")]
    public GameObject mainMenu;
    public GameObject settingsPanel;
    public GameObject howToPlayPanel;
    public GameObject creatersPanel;
    public GameObject p1WinPanel;
    public GameObject p2WinPanel;
    private List<GameObject> allPanels;

    [Header("Gamepad Input")] public InputActionReference cancelAction;

    [Header("İlk Seçilecek Butonlar")] public GameObject firstMainMenuButton;
    public GameObject firstSettingsButton;
    public GameObject firstHowToPlayButton;
    public GameObject firstCreatersButton;


    private void Awake()
    {
        allPanels = new List<GameObject> { mainMenu, settingsPanel, howToPlayPanel, creatersPanel };
    }

    private void OnEnable()
    {
        if (cancelAction != null) cancelAction.action.Enable();
    }

    private void OnDisable()
    {
        if (cancelAction != null) cancelAction.action.Disable();
    }

    void Start()
    {
        // foreach (var panel in allPanels)
        // {
        //     if (panel != null && panel != mainMenu)
        //     {
        //         panel.SetActive(false);
        //     }
        // }
        //
        // mainMenu.SetActive(true);
        //
        // EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
        // UI_Controller.cs içindeki Start() fonksiyonunu tamamen bununla değiştirin.


        // Önce tüm panelleri kapat
        foreach (var panel in allPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        // 1. KAZANAN KONTROLÜ
        if (GameRoundManager.LastWinner != 0)
        {
            if (GameRoundManager.LastWinner == 1 && p1WinPanel != null) p1WinPanel.SetActive(true);
            else if (GameRoundManager.LastWinner == 2 && p2WinPanel != null) p2WinPanel.SetActive(true);
            
            GameRoundManager.LastWinner = 0;
            StartCoroutine(ShowMainMenuAfterDelay(3f));
        }
        // 2. NORMAL BAŞLANGIÇ
        else
        {
            if (mainMenu != null) mainMenu.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
        }
    }

    void Update()
    {
        if (cancelAction != null && cancelAction.action.WasPressedThisFrame())
        {
            if (mainMenu != null && !mainMenu.activeInHierarchy)
            {
                OpenPanel(mainMenu);
            }
        }
    }

    public void OpenPanel(GameObject targetPanel)
    {
        StartCoroutine(SwitchPanelRoutine(targetPanel));
    }

    private IEnumerator SwitchPanelRoutine(GameObject targetPanel)
    {
        yield return StartCoroutine(Fade_Manager.Instance.FadeOut());

        foreach (var panel in allPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        if (targetPanel != null) targetPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        if (targetPanel == mainMenu) EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
        if (targetPanel == settingsPanel) EventSystem.current.SetSelectedGameObject(firstSettingsButton);
        if (targetPanel == howToPlayPanel) EventSystem.current.SetSelectedGameObject(firstHowToPlayButton);
        if (targetPanel == creatersPanel) EventSystem.current.SetSelectedGameObject(firstCreatersButton);

        yield return StartCoroutine(Fade_Manager.Instance.FadeIn());
    }

    public void LoadSceneWithFade(string sceneName)
    {
        Fade_Manager.Instance.StartFadeOutAndLoadScene(sceneName);
    }

    private IEnumerator ShowMainMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var panel in allPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}