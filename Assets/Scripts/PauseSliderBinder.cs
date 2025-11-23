using UnityEngine;
using UnityEngine.UI;

public class PauseSliderBinder : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()  
    {  
        if (AudioManager.Instance == null) return;  

        musicSlider.value = AudioManager.Instance.MusicSliderValue;  
        sfxSlider.value = AudioManager.Instance.SFXSliderValue;  

        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);  
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);  
    }  

}