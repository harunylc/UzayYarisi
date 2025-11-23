using UnityEngine;

[CreateAssetMenu(fileName = "NewCarStats", menuName = "Car System/Car Stats")] 
public class CarStatsData : ScriptableObject
{
    public int carIndex; 
    
    // Değerleri değiştirmek istersen buradaki sayıları değiştireceksin.
    public const float MinHiz = 6000f;     public const float MaxHiz = 14000f; 
    public const float MinFren = 20f;      public const float MaxFren = 70f;
    public const float MinNitro = 3000f;   public const float MaxNitro = 9000f;
    public const float MinYol = 2000f;     public const float MaxYol = 7000f;
    public const float MinAgir = 1000f;    public const float MaxAgir = 2500f;
    
    [Header("Temel Özellikler")]
    [Tooltip("Aracın başlangıç hızı. Min-Max değerleri koddan ayarlanır.")]
    
    [Range(MinHiz, MaxHiz)] 
    public float temelHızlanma; 

    [Range(MinFren, MaxFren)]
    public float temelFren;

    [Range(MinNitro, MaxNitro)]
    public float temelNitro;

    [Range(MinYol, MaxYol)]
    public float temelYoltutus;

    [Range(MinAgir, MaxAgir)]
    public float temelAgırlık;
}