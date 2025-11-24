using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class SoundsCollectionSO : ScriptableObject
{
    [Header("SFX")] 
    public SoundSO[] PickUp;
    
    [Header("Music")] 
    public SoundSO[] Music;
    
    [Header("Car SFX")]
    public SoundSO[] CarSounds;
    
    [Header("Win SFX")]
    public SoundSO WinSound;
    
    [Header("Explosion SFX")]
    public SoundSO Explosion;
    
    [Header("Alerts")]
    public SoundSO[] WarningSounds;

}