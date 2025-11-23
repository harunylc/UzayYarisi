using UnityEngine;

[CreateAssetMenu(fileName = "NewCarStats", menuName = "Car System/Car Stats")] 
public class CarStatsData : ScriptableObject
{
    public int carIndex; 
    
    // Değerleri değiştirmek istersen buradaki sayıları değiştireceksin.
    public const float MinHiz = 5f;     public const float MaxHiz = 20f;
    public const float MinFren = 10f;   public const float MaxFren = 40f;
    public const float MinNitro = 0f;   public const float MaxNitro = 100f;
    public const float MinYol = 0.5f;   public const float MaxYol = 2.5f;
    public const float MinAgir = 500f;  public const float MaxAgir = 2000f;
    
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