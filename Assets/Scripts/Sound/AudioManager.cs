using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [Header("AudioMixer")]
    [SerializeField] private AudioMixer _audioMixer;

    
    [Header("UI Sliders")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
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
    
    private void Start()
    {
        // Başlangıç değerlerini mixer’dan al ve slider’a uygula
        _audioMixer.GetFloat("MusicVolume", out float musicVol);
        musicSlider.value = Mathf.Pow(10, musicVol / 20); // dB -> 0-1

        _audioMixer.GetFloat("SFXVolume", out float sfxVol);
        sfxSlider.value = Mathf.Pow(10, sfxVol / 20);

        // Slider eventlerini ekle
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    
    [Range(0f, 2f)] [SerializeField] private float _masterVolume = 1f;
    [SerializeField] private SoundsCollectionSO _soundsCollectionSo;

    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private AudioSource _currentMusic;


    #region Sound Methods

    private void PlayRandomSound(SoundSO[] sounds)
    {
        if (sounds != null && sounds.Length > 0)
        {
            SoundSO soundSo = sounds[Random.Range(0, sounds.Length)];
            SoundToPlay(soundSo);
        }
    }

    public void SoundToPlay(SoundSO soundSO)
    {
        AudioClip clip = soundSO.Clip;

        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume * _masterVolume;
        bool loop = soundSO.Loop;
        AudioMixerGroup audioMixerGroup;

        pitch = RandomizePitch(soundSO, pitch);

        audioMixerGroup = DetermineAudioMixerGroup(soundSO);

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO soundSO)
    {
        AudioMixerGroup audioMixerGroup;
        switch (soundSO.AudioType)
        {
            case SoundSO.AudioTypes.SFX:
                audioMixerGroup = _sfxMixerGroup;
                break;
            case SoundSO.AudioTypes.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }

        return audioMixerGroup;
    }

    private static float RandomizePitch(SoundSO soundSO, float pitch)
    {
        if (soundSO.RandomizePitch)
        {
            float randomPitchModifier =
                Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);
            pitch = soundSO.Pitch + randomPitchModifier;
        }

        return pitch;
    }

    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        audioSource.Play();

        if (!loop)
        {
            Destroy(soundObject, clip.length);
        }

        DetermineMusic(audioMixerGroup, audioSource);
    }

    private void DetermineMusic(AudioMixerGroup audioMixerGroup, AudioSource audioSource)
    {
        if (audioSource.outputAudioMixerGroup == _musicMixerGroup)
        {
            if (_currentMusic != null) { }
            _currentMusic = audioSource;
        }
    }
    
    public void SetMusicVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20;
        _audioMixer.SetFloat("MusicVolume", dB);
    }

    public void SetSFXVolume(float value)
    {
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

    #endregion
}
