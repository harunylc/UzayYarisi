using UnityEngine;

[CreateAssetMenu(fileName = "NewCarStats", menuName = "Car System/Car Stats")] 
public class CarStatsData : ScriptableObject
{
    public int carIndex; 
    
    // Değerleri değiştirmek istersen buradaki sayıları değiştireceksin.
    public const float MinHiz = 4000f;     public const float MaxHiz = 12000f;
    public const float MinFren = 10f;      public const float MaxFren = 60f; // Kod bunu 20'ye bölüp Drag yapacak
    public const float MinNitro = 2000f;   public const float MaxNitro = 8000f;
    public const float MinYol = 1500f;     public const float MaxYol = 6000f;
    public const float MinAgir = 800f;     public const float MaxAgir = 2200f;
    
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