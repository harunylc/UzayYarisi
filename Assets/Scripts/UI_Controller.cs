using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class UI_Controller : MonoBehaviour
{
[Header("Paneller (Tümünü Sürükleyin)")]
public GameObject mainMenu;
public GameObject settingsPanel;
public GameObject howToPlayPanel;
public GameObject creatersPanel;
public GameObject exitPanel;
private List<GameObject> allPanels;

[Header("Gamepad Input")]
public InputActionReference cancelAction; // Inspector'dan Cancel eylemini sürükle

[Header("İlk Seçilecek Butonlar")]
public GameObject firstMainMenuButton;
public GameObject firstSettingsButton;
public GameObject firstHowToPlayButton;
public GameObject firstCreatersButton;

private void Awake()
{
    Debug.Log("UI_Controller::Awake");
    // Panelleri kolayca yönetmek için bir listeye doldur
    allPanels = new List<GameObject> { mainMenu, settingsPanel, howToPlayPanel, creatersPanel, exitPanel };
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
    // Başlangıçta Ana Menü dışındaki tüm panelleri kapat
    foreach (var panel in allPanels)
    {
        if (panel != null && panel != mainMenu)
        {
            panel.SetActive(false);
        }
    }
    mainMenu.SetActive(true);
    
    // Gamepad navigasyonunu başlatmak için ilk butonu seç
    EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
}

void Update()
{
    // Eğer B tuşuna basıldıysa ve ana menüde değilsek...
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
}