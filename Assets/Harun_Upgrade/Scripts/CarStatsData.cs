using UnityEngine;

[CreateAssetMenu(fileName = "NewCarStats", menuName = "Car System/Car Stats")] 
public class CarStatsData : ScriptableObject
{
    public int carIndex; 
    
    // Değerleri değiştirmek istersen buradaki sayıları değiştireceksin.
    
    public const float MinHiz = 15000f;   public const float MaxHiz = 40000f;
    
    public const float MinFren = 30f;     public const float MaxFren = 100f;

    public const float MinNitro = 5000f;  public const float MaxNitro = 15000f;

    public const float MinYol = 2500f;    public const float MaxYol = 8000f;

    public const float MinAgir = 1200f;   public const float MaxAgir = 2000f;
    
    
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