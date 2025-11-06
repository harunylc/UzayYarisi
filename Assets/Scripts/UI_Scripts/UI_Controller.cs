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
    public GameObject exitPanel;
    public GameObject p1WinPanel; 
    public GameObject p2WinPanel;
    private List<GameObject> allPanels;

    [Header("Gamepad Input")] public InputActionReference cancelAction;

    [Header("İlk Seçilecek Butonlar")] public GameObject firstMainMenuButton;
    public GameObject firstSettingsButton;
    public GameObject firstHowToPlayButton;
    public GameObject firstCreatersButton;
    public GameObject firstExitButton; 


    private void Awake()
    {
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

        void Start()
        {
            // Önce TÜM panelleri kapatalım.
            foreach (var panel in allPanels)
            {
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }
    
            // Şimdi GameRoundManager'dan gelen bir "kazanan" bilgisi var mı diye kontrol et.
            if (GameRoundManager.LastWinner != 0)
            {
                // Eğer bir kazanan varsa, ilgili paneli göster.
                if (GameRoundManager.LastWinner == 1)
                {
                    p1WinPanel.SetActive(true);
                }
                else if (GameRoundManager.LastWinner == 2)
                {
                    p2WinPanel.SetActive(true);
                }
        
                // Kazanan bilgisini sıfırla ki menüye bir daha girildiğinde bu ekran tekrar çıkmasın.
                GameRoundManager.LastWinner = 0;
        
                // 3 saniye sonra ana menüyü göstermek için bir Coroutine başlat.
                StartCoroutine(ShowMainMenuAfterDelay(3f));
            }
            else
            {
                // Eğer bir kazanan bilgisi yoksa (oyun normal şekilde açıldıysa),
                // direkt ana menüyü göster.
                mainMenu.SetActive(true);
                EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
            }
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
    // UI_Controller.cs'in en altına ekleyin.

    private IEnumerator ShowMainMenuAfterDelay(float delay)
    {
        // Belirtilen süre kadar bekle.
        yield return new WaitForSeconds(delay);

        // Tüm panelleri tekrar kapat (kazanan paneli de dahil).
        foreach (var panel in allPanels)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        // Şimdi ana menüyü aç ve ilk butonu seç.
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstMainMenuButton);
    }
}