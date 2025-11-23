using UnityEngine;
using System.Collections;

public class RaceStartManager : MonoBehaviour
{
    [Header("Geri Sayım Ayarları")]
    public GameObject countdownObject;
    void Start()
    {
        StartCoroutine(StartRaceCountdown());
    }

    private IEnumerator StartRaceCountdown()
    {
        if (InputManager.instance != null)
        {
            InputManager.instance.DisableAllInput();
        }

        Time.timeScale = 0f;

        if (countdownObject != null)
        {
            countdownObject.SetActive(true);
        }
        yield return null; 
    }

    public void OnCountdownFinished()
    {
        if (countdownObject != null)
        {
            countdownObject.SetActive(false);
        }

        if (InputManager.instance != null)
        {
            InputManager.instance.EnableAllInput();
        }
    
        Time.timeScale = 1f;
    }
}