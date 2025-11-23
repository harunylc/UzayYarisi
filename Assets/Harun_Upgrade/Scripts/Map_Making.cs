using System;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class Map_Making_Fixed : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;

    [Header("Map Settings")]
    [SerializeField, Range(3, 500)] private int levelLength = 500; 
    [SerializeField, Range(1f, 100f)] private float xMultiplier = 2f;
    [SerializeField, Range(1f, 100f)] private float yMultiplier = 10f;
    [SerializeField, Range(0f, 1f)] private float noiseScale = 0.1f; 
    
    [SerializeField, Range(0f, 0.5f)] private float curveSmoothness = 0.35f;
    
    [SerializeField] private float bottomDepth = 20f; 

    private bool _needsUpdate = false;

    private void OnValidate()
    {
        _needsUpdate = true;
    }

    private void Update()
    {
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

        // Noktaları tutmak için geçici dizi (Eğim hesabı için lazım)
        Vector3[] points = new Vector3[levelLength];

        for (int i = 0; i < levelLength; i++)
        {
            float xPos = i * xMultiplier;
            float yPos = Mathf.PerlinNoise(0, i * noiseScale) * yMultiplier;
            points[i] = new Vector3(xPos, yPos, 0);
            spline.InsertPointAt(i, points[i]);
        }
        
        // Loop içinde (i-1) ve (i+1) noktalarına bakarak daha doğal bir eğim vereceğiz.
        for (int i = 0; i < levelLength; i++)
        {
            // İlk ve Son nokta "Broken" (Keskin) kalmalı
            if (i == 0 || i == levelLength - 1)
            {
                spline.SetTangentMode(i, ShapeTangentMode.Broken);
                spline.SetRightTangent(i, Vector3.zero);
                spline.SetLeftTangent(i, Vector3.zero);
                continue;
            }

            spline.SetTangentMode(i, ShapeTangentMode.Continuous);

            // Önceki ve sonraki nokta arasındaki vektörü al
            Vector3 prevPoint = points[i - 1];
            Vector3 nextPoint = points[i + 1];
            
            // Bu vektör, noktanın eğim yönünü belirler
            Vector3 slopeDir = (nextPoint - prevPoint).normalized;

            // Teğet uzunluğunu iki nokta arasındaki mesafeye göre ölçekle
            float distance = (nextPoint - prevPoint).magnitude;
            Vector3 tangent = slopeDir * distance * curveSmoothness * 0.5f;

            spline.SetRightTangent(i, tangent);
            spline.SetLeftTangent(i, -tangent);
        }
        
        float endX = (levelLength - 1) * xMultiplier;

        // Sağ Alt Köşe
        spline.InsertPointAt(levelLength, new Vector3(endX, -bottomDepth, 0));
        spline.SetTangentMode(levelLength, ShapeTangentMode.Broken); 

        // Sol Alt Köşe
        spline.InsertPointAt(levelLength + 1, new Vector3(0, -bottomDepth, 0));
        spline.SetTangentMode(levelLength + 1, ShapeTangentMode.Broken); 
            
        
        spriteShapeController.BakeMesh();
        
    }
}