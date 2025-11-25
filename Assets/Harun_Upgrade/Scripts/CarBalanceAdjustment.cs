using UnityEngine;

public class CarBalanceAdjustment : MonoBehaviour
{
    [Header("Denge Ayarları")]
    [Tooltip("X: İleri (+)/Geri (-)\nY: Aşağı (-)/Yukarı (+)")]
    // Tavsiye Başlangıç: X = 0.2, Y = -0.5
    public Vector2 centerOfMassOffset = new Vector2(0.2f, -0.5f); 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            // Mevcut ağırlık merkezini alıp bizim ayarımızı ekliyoruz
            rb.centerOfMass += centerOfMassOffset;
        }
    }

    // Bu fonksiyon sadece Editörde çalışır, ayar yaparken görmeni sağlar
    private void OnDrawGizmosSelected()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            Gizmos.color = Color.red;
            
            // Ağırlık merkezinin dünyadaki yerini hesapla
            // Not: Unity bazen CoM'u tam güncellemeyebilir, bu tahmini yerdir.
            Vector3 worldCoM = transform.TransformPoint(rb.centerOfMass + centerOfMassOffset);
            
            // Kırmızı bir nokta çiz
            Gizmos.DrawSphere(worldCoM, 0.3f);
        }
    }
}