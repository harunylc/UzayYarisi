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

}