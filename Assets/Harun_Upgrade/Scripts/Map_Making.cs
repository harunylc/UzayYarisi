using System;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class Map_Making_Fixed : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;

    [Header("Map Settings")]
    [SerializeField, Range(3, 500)] private int levelLength = 500; // Test için düşürdüm, artırabilirsin
    [SerializeField, Range(1f, 100f)] private float xMultiplier = 2f;
    [SerializeField, Range(1f, 100f)] private float yMultiplier = 10f;
    [SerializeField, Range(0f, 1f)] private float noiseScale = 0.1f; 
    
    [SerializeField, Range(0f, 0.5f)] private float curveSmoothness = 0.35f;
    [SerializeField] private float bottomDepth = 20f; 

    private bool _needsUpdate = false;

    private void OnValidate()
    {
        // Değer değişince işaretle
        _needsUpdate = true;
    }

    // OYUN BAŞLAYINCA GARANTİ OLUŞTURMA
    private void Awake() // Start yerine Awake daha güvenlidir
    {
        GenerateMap();
    }

    private void Update()
    {
        // Sadece Edit modundayken ve değişiklik varsa çalıştır
        // Play modunda sürekli çalışmasını istemeyiz (performans için)
        if (_needsUpdate && !Application.isPlaying)
        {
            GenerateMap();
            _needsUpdate = false;
        }
    }
    
    public void GenerateMap()
    {
        if (spriteShapeController == null) return;

        Spline spline = spriteShapeController.spline;
        spline.Clear();

        Vector3[] points = new Vector3[levelLength];

        // 1. Noktaları Oluştur
        for (int i = 0; i < levelLength; i++)
        {
            float xPos = i * xMultiplier;
            // Perlin Noise her zaman aynı seed ile çalışsın diye ofset eklemedik, 
            // ama farklı haritalar istersen buraya rastgelelik ekleyebilirsin.
            float yPos = Mathf.PerlinNoise(0, i * noiseScale) * yMultiplier;
            
            points[i] = new Vector3(xPos, yPos, 0);
            spline.InsertPointAt(i, points[i]);
        }
        
        // 2. Teğetleri (Tangents) Ayarla - Yumuşatma
        for (int i = 0; i < levelLength; i++)
        {
            if (i == 0 || i == levelLength - 1)
            {
                spline.SetTangentMode(i, ShapeTangentMode.Broken);
                spline.SetRightTangent(i, Vector3.zero);
                spline.SetLeftTangent(i, Vector3.zero);
                continue;
            }

            spline.SetTangentMode(i, ShapeTangentMode.Continuous);

            Vector3 prevPoint = points[i - 1];
            Vector3 nextPoint = points[i + 1];
            Vector3 slopeDir = (nextPoint - prevPoint).normalized;
            float distance = (nextPoint - prevPoint).magnitude;
            Vector3 tangent = slopeDir * distance * curveSmoothness * 0.5f;

            spline.SetRightTangent(i, tangent);
            spline.SetLeftTangent(i, -tangent);
        }
        
        // 3. Alt Tabanı Kapat (Zemin altı)
        float endX = (levelLength - 1) * xMultiplier;
        spline.InsertPointAt(levelLength, new Vector3(endX, -bottomDepth, 0));
        spline.SetTangentMode(levelLength, ShapeTangentMode.Broken); 

        spline.InsertPointAt(levelLength + 1, new Vector3(0, -bottomDepth, 0));
        spline.SetTangentMode(levelLength + 1, ShapeTangentMode.Broken); 
            
        // 4. KRİTİK GÜNCELLEMELER
        // SpriteShape'in görselini yenile
        spriteShapeController.RefreshSpriteShape(); 
        
        // Görseli oluştur
        spriteShapeController.BakeMesh();
        
        // FİZİK MOTORU İÇİN COLLIDER'I YENİLE (Sorunun ana çözümü burası)
        // EdgeCollider2D componentinin bu objede ekli olduğundan emin ol.
        spriteShapeController.BakeCollider();
    }
}