using UnityEngine;

public class CarBalanceAdjustment : MonoBehaviour
{
    public Vector2 centerOfMassOffset = new Vector2(0.2f, -0.5f); 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.centerOfMass += centerOfMassOffset;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            Gizmos.color = Color.red;
            Vector3 worldCoM = transform.TransformPoint(rb.centerOfMass + centerOfMassOffset);
            Gizmos.DrawSphere(worldCoM, 0.3f);
        }
    }
}