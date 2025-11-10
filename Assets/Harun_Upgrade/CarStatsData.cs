using UnityEngine;

[CreateAssetMenu(fileName = "NewCarStats", menuName = "Car System/Car Stats")] 
public class CarStatsData : ScriptableObject
{
    public int carIndex; 
    
    [Header("Temel Özellikler")]
    public float temelHızlanma;
    public float temelFren;
    public float temelNitro;
    public float temelYoltutus;
    public float temelAgırlık;

    [Header("Yükseltme Etkisi")]
    public float hızlanmaIncreasePerLevel = 2f;
    public float frenIncreasePerLevel = 1.5f;
    public float nitroIncreasePerLevel = 0.5f;
    public float yoltutusIncreasePerLevel = 0.5f;
    public float agırlıkIncreasePerLevel = 0.5f;
    
}
