using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class UI_Controller : MonoBehaviour
{
    public GameObject MainMenu,SettingsMenu,CreatersMenu,HowToPlayMenu,ExitMenu;
    private GameObject[] uiPanels; // panellerin bulundugu array

    public Fade_Manager fadeManager;  
    private void Awake()
    {
        uiPanels = new GameObject[] { MainMenu, SettingsMenu, CreatersMenu,HowToPlayMenu, ExitMenu };
    }

    void Start()
    {
        // Butun Paneller Kapat
        foreach (GameObject panel in uiPanels)
        {
            panel.SetActive(false);
        }

        // MainMenu Ac
        MainMenu.SetActive(true);
    }
    public void OpenPanel(GameObject targetPanel)
    {
        StartCoroutine(SwitchWithFade(targetPanel));
    }

    private IEnumerator SwitchWithFade(GameObject targetPanel)
    {
        // 1. Fade karart
        yield return StartCoroutine(fadeManager.FadeOut());

        // 2. Panelleri kapat
        foreach (GameObject panel in uiPanels)
        {
            panel.SetActive(false);
        }

        // 3. Yeni paneli aç
        targetPanel.SetActive(true);

        // 4. Fade geri aç
        yield return StartCoroutine(fadeManager.FadeIn());
    }
<<<<<<< HEAD
=======
    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return StartCoroutine(fadeManager.FadeOut());

        SceneManager.LoadScene(sceneName);
    }
}
