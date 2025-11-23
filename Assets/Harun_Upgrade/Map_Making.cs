using System;
using UnityEngine;
using UnityEngine.U2D;

// OnValidate hatalarını önlemek için editör modunda çalışır ama kontrollü olmalı
[ExecuteInEditMode]
public class Map_Making_Corrected : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;

    [Header("Map Settings")]
    [SerializeField, Range(3, 2000)] private int levelLength = 2000;
    [SerializeField, Range(1f, 100f)] private float xMultiplier = 2f;
    [SerializeField, Range(1f, 100f)] private float yMultiplier = 10f; // Yüksekliği artırdım ki fark görülsün
    [SerializeField, Range(0f, 1f)] private float noiseScale = 0.1f; 
    [SerializeField, Range(0f, 1f)] private float curveSmoothness = 0.5f;
    [SerializeField] private float bottomDepth = 20f; 

    // Bu değişken gereksiz yenilemeleri (loop) engeller
    private bool _needsUpdate = false;

    private void OnValidate()
    {
        // Değerler değiştiğinde haritayı yeniden oluşturmak için işaretle
        _needsUpdate = true;
    }

    private void Update()
    {
        // Sadece editör modunda ve bir değişiklik olduğunda çalıştır
        if (_needsUpdate && !Application.isPlaying)
        {
            GenerateMap();
            _needsUpdate = false;
        }
    }

    private void GenerateMap()
    {
        if (spriteShapeController == null) return;

        Spline spline = spriteShapeController.spline;
        spline.Clear();

        // --- 1. Üst Yüzey Noktalarını Oluştur ---
        for (int i = 0; i < levelLength; i++)
        {
            float xPos = i * xMultiplier;
            float yPos = Mathf.PerlinNoise(0, i * noiseScale) * yMultiplier;
            
            // Z eksenini her zaman 0 tutmalıyız
            Vector3 currentPos = new Vector3(xPos, yPos, 0);
            
            spline.InsertPointAt(i, currentPos);
            
            // Başlangıç ve Bitiş noktaları HARİÇ aradaki noktalara eğim ver
            if (i > 0 && i < levelLength - 1)
            {
                spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                
                // Teğetlerin uzunluğunu noktalar arası mesafeye göre sınırlamazsak "loop" oluşur
                Vector3 tangent = Vector3.right * xMultiplier * curveSmoothness;
                spline.SetRightTangent(i, tangent);
                spline.SetLeftTangent(i, -tangent); // Left tangent negatiftir
            }
            else
            {
                // İLK ve SON üst nokta "Broken" (Keskin) olmalı ki aşağıya düz insin
                spline.SetTangentMode(i, ShapeTangentMode.Broken); // ÖNEMLİ DÜZELTME
                spline.SetRightTangent(i, Vector3.zero);
                spline.SetLeftTangent(i, Vector3.zero);
            }
        }
        
        // --- 2. Alt Köşeleri Oluştur ve Şekli Kapat ---
        
        float endX = (levelLength - 1) * xMultiplier;

        // Sağ Alt Köşe
        int rightBottomIndex = levelLength;
        Vector3 rightBottomPos = new Vector3(endX, -bottomDepth, 0); 
        spline.InsertPointAt(rightBottomIndex, rightBottomPos);
        spline.SetTangentMode(rightBottomIndex, ShapeTangentMode.Broken); // Keskin Köşe

        // Sol Alt Köşe
        int leftBottomIndex = levelLength + 1;
        Vector3 leftBottomPos = new Vector3(0, -bottomDepth, 0);
        spline.InsertPointAt(leftBottomIndex, leftBottomPos);
        spline.SetTangentMode(leftBottomIndex, ShapeTangentMode.Broken); // Keskin Köşe

        // --- 3. Sprite Shape'i Yenile ---
        // Bu komut bazen takılı kalan geometrileri temizler
        spriteShapeController.BakeMesh();
    }
}