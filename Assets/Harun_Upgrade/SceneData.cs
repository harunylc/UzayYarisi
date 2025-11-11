using UnityEngine;
using System;

[System.Serializable]
public class SceneData
{
    // SceneFlowManager'daki 'scenes' listesi ile eşleşmeli
    public string SceneName; 
    
    // Sahnenin resmini tutacak değişken
    public Sprite SceneImage;
    
    // Sahnenin başlığını tutacak metin
    public string SceneTitleText; 
}

