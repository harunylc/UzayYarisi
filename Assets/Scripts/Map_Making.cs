using System;
using UnityEngine;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class Map_Making_Corrected : MonoBehaviour
{
    [SerializeField] private SpriteShapeController spriteShapeController;

    [SerializeField, Range(3, 100)] private int levelLength = 50;
    [SerializeField, Range(1f, 50f)] private float xMultiplier = 2f;
    [SerializeField, Range(1f, 50f)] private float yMultiplier = 2f;
    [SerializeField, Range(0f, 1f)] private float noiseScale = 0.1f; 
    [SerializeField, Range(0f, 1f)] private float curveSmoothness = 0.5f;
    [SerializeField] private float bottomDepth = 20f; 

    private void OnValidate()
    {
        if (spriteShapeController == null)
        {
            Debug.LogError("SpriteShapeController atanmamış.");
            return;
        }

        GenerateMap();
    }

    private void GenerateMap()
    {
        spriteShapeController.spline.Clear();

        for (int i = 0; i < levelLength; i++)
        {
            float xPos = i * xMultiplier;
            float yPos = Mathf.PerlinNoise(0, i * noiseScale) * yMultiplier;
            
            Vector3 currentPos = new Vector3(xPos, yPos);
            
            spriteShapeController.spline.InsertPointAt(i, currentPos);
            
            if (i != 0 && i != levelLength - 1)
            {
                spriteShapeController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spriteShapeController.spline.SetRightTangent(i, Vector3.right * xMultiplier * curveSmoothness);
                spriteShapeController.spline.SetLeftTangent(i, Vector3.left * xMultiplier * curveSmoothness);
            }
        }
        
        
        float endX = (levelLength - 1) * xMultiplier;

        int rightBottomIndex = levelLength;
        Vector3 rightBottomPos = new Vector3(endX, -bottomDepth); 
        spriteShapeController.spline.InsertPointAt(rightBottomIndex, rightBottomPos);
        
        int leftBottomIndex = levelLength + 1;
        Vector3 leftBottomPos = new Vector3(0, -bottomDepth);
        spriteShapeController.spline.InsertPointAt(leftBottomIndex, leftBottomPos);

    }
}