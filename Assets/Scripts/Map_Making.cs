using System;
using UnityEngine;

using UnityEngine.U2D;
[ExecuteInEditMode]
public class Map_Making : MonoBehaviour
{
   [SerializeField] private SpriteShapeController spriteShapeController;
   
   [SerializeField, Range(3f, 100f)] private int levelLenght=50;
   [SerializeField, Range(1f,50f)] private float xMultiplier=2f;
   [SerializeField, Range(1f,50f)] private float yMultiplier=2f;
   [SerializeField, Range(0f,1f)] private float curveSmoothness=0.5f;
   [SerializeField] private float noise=0.5f;
   [SerializeField] private float bottom = 10f;

   private Vector3 lastPos;

   private void OnValidate()
   {
      spriteShapeController.spline.Clear();

      for (int i = 0; i < levelLenght; i++)
      {
         lastPos = transform.position + new Vector3(i * xMultiplier, Mathf.PerlinNoise(0, i * noise) * yMultiplier);
         spriteShapeController.spline.InsertPointAt(i,lastPos);
         
         if (i!=0 && i!=levelLenght-1)
         {
            spriteShapeController.spline.SetTangentMode(i,ShapeTangentMode.Continuous);
            spriteShapeController.spline.SetRightTangent(i,Vector3.right*xMultiplier*curveSmoothness);
            spriteShapeController.spline.SetLeftTangent(i,Vector3.left*xMultiplier*curveSmoothness);
         }
      }
      spriteShapeController.spline.InsertPointAt(levelLenght,new Vector3(lastPos.x,transform.position.y-bottom));
      spriteShapeController.spline.InsertPointAt(levelLenght+1,new Vector3(lastPos.x,transform.position.y-bottom));

      
   }
}
