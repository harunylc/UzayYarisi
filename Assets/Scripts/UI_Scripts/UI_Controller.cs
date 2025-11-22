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
    private List<GameObject> allPanels;

    [Header("Gamepad Input")] 
    public InputActionReference cancelAction;

    [Header("İlk Seçilecek Butonlar")] 
    public GameObject firstMainMenuButton;
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
        foreach (var panel in allPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
        
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
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
        if (SceneFlowManager.Instance != null)
        {
            SceneFlowManager.Instance.StartGame();
        }
        else
        {
            // Acil durum için eski kod
            Fade_Manager.Instance.StartFadeOutAndLoadScene(sceneName);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}