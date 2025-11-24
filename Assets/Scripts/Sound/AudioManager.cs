using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("AudioMixer")]  
    [SerializeField] private AudioMixer _audioMixer;  

    [Header("UI Sliders")]  
    [SerializeField] private Slider musicSlider;  
    [SerializeField] private Slider sfxSlider;  

    public float MusicSliderValue { get; private set; } = 1f;  
    public float SFXSliderValue { get; private set; } = 1f;  

    [Range(0f, 2f)] [SerializeField] private float _masterVolume = 1f;  
    
    [SerializeField] private SoundsCollectionSO _soundsCollectionSo;  
    public SoundsCollectionSO SoundsCollection => _soundsCollectionSo; 

    [SerializeField] private AudioMixerGroup _sfxMixerGroup;  
    [SerializeField] private AudioMixerGroup _musicMixerGroup;  

    private AudioSource _currentMusic;  

    private readonly string[] gameScenes = { "Dünya", "Mars", "Merkür", "Neptün", "Satürn" }; 
    private readonly string[] menuScenes = { "UpgradeLobby", "MainMenuScene" };


    private void Awake()  
    {  
        if (Instance != null && Instance != this)  
        {  
            Destroy(gameObject);  
            return;  
        }  
        Instance = this;  
        DontDestroyOnLoad(gameObject);  
    }  

    private void OnEnable()  
    {  
        SceneManager.sceneLoaded += OnSceneLoaded;  
    }  

    private void OnDisable()  
    {  
        SceneManager.sceneLoaded -= OnSceneLoaded;  
    }  

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)  
    {  
        if (_currentMusic != null)
        {
            _currentMusic.Stop();
            Destroy(_currentMusic.gameObject);
            _currentMusic = null;
        }
    
        foreach (string gameScene in gameScenes)  
        {  
            if (scene.name == gameScene)  
            {  
                PlayGameMusic();  
                return;
            }  
        }  
    
        foreach (string menuScene in menuScenes)  
        {  
            if (scene.name == menuScene)  
            {  
                if (_soundsCollectionSo.Music.Length > 1) 
                {
                    SoundToPlay(_soundsCollectionSo.Music[1]);
                }
                return;
            }
        }
    } 

    private void Start()  
    {  
        if (_audioMixer)
        {
            _audioMixer.GetFloat("MusicVolume", out float musicVol);  
            if(musicSlider) musicSlider.value = Mathf.Pow(10, musicVol / 20);  

            _audioMixer.GetFloat("SFXVolume", out float sfxVol);  
            if(sfxSlider) sfxSlider.value = Mathf.Pow(10, sfxVol / 20);  
        }

        if(musicSlider) musicSlider.onValueChanged.AddListener(SetMusicVolume);  
        if(sfxSlider) sfxSlider.onValueChanged.AddListener(SetSFXVolume);  
    }  

    #region Sound Methods  

    private void PlayRandomSound(SoundSO[] sounds)  
    {  
        if (sounds != null && sounds.Length > 0)  
        {  
            SoundSO soundSo = sounds[Random.Range(0, sounds.Length)];  
            SoundToPlay(soundSo);  
        }  
    }  

    public AudioSource SoundToPlay(SoundSO soundSO)  
    {  
        if (soundSO == null) return null;

        AudioClip clip = soundSO.Clip;  

        float pitch = soundSO.Pitch;  
        float volume = soundSO.Volume * _masterVolume;  
        bool loop = soundSO.Loop;  
        AudioMixerGroup audioMixerGroup;  

        pitch = RandomizePitch(soundSO, pitch);  
        audioMixerGroup = DetermineAudioMixerGroup(soundSO);  
        
        return PlaySound(clip, pitch, volume, loop, audioMixerGroup);  
    }  

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO soundSO)  
    {  
        switch (soundSO.AudioType)  
        {  
            case SoundSO.AudioTypes.SFX: return _sfxMixerGroup;  
            case SoundSO.AudioTypes.Music: return _musicMixerGroup;  
            default: return null;  
        }  
    }  

    private static float RandomizePitch(SoundSO soundSO, float pitch)  
    {  
        if (soundSO.RandomizePitch)  
        {  
            float randomPitchModifier = Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);  
            pitch = soundSO.Pitch + randomPitchModifier;  
        }  
        return pitch;  
    }  

    private AudioSource PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)  
    {  
        GameObject soundObject = new GameObject("Temp Audio Source");  
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();  

        audioSource.clip = clip;  
        audioSource.pitch = pitch;  
        audioSource.volume = volume;  
        audioSource.loop = loop;  
        audioSource.outputAudioMixerGroup = audioMixerGroup;  

        audioSource.Play();  

        if (!loop) Destroy(soundObject, clip.length);  

        if (audioMixerGroup == _musicMixerGroup) _currentMusic = audioSource; 
        
        return audioSource;
    }  

    #endregion  

    #region Volume Controls  
    public void SetMusicVolume(float value)  
    {  
        MusicSliderValue = value;  
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;  
        _audioMixer.SetFloat("MusicVolume", dB);  
    }  

    public void SetSFXVolume(float value)  
    {  
        SFXSliderValue = value;  
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;  
        _audioMixer.SetFloat("SFXVolume", dB);  
    }  
    #endregion  

    #region SFX  
    public void PlayPickUp()  
    {  
        PlayRandomSound(_soundsCollectionSo.PickUp);  
    }  
    #endregion  

    #region Music  
    public void PlayGameMusic()  
    {  
        if (_soundsCollectionSo.Music != null && _soundsCollectionSo.Music.Length > 0)  
        {  
            SoundToPlay(_soundsCollectionSo.Music[0]);  
        }  
    }  
    #endregion
    
   #region Game State Sounds
    public void PlayWinSound()
    {
        if (_soundsCollectionSo.WinSound != null)
        {
            SoundToPlay(_soundsCollectionSo.WinSound);
        }
    }
    #endregion
    
    public void PlayExplosion()
    {
        if (_soundsCollectionSo.Explosion != null)
        {
            
            SoundToPlay(_soundsCollectionSo.Explosion);
        }
    }
    
    public void PlayWarningSound(int index) 
    {
        SoundSO[] sounds = _soundsCollectionSo.WarningSounds;

        if (sounds != null && index >= 0 && index < sounds.Length)
        {
            SoundToPlay(sounds[index]);
        }
    }

}