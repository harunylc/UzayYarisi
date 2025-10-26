using UnityEngine;

public class WheelFriction2D : MonoBehaviour
{
    public float lateralFrictionStrength = 8f;   // yan kaymayı önler
    public float forwardFrictionStrength = 0.5f; // ileri yön sürtünmesi
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ApplyLateralFriction();
        ApplyForwardDrag();
    }

    void ApplyLateralFriction()
    {
        // Tekerin sağ (yan) ekseni
        Vector2 right = transform.right;
        // Yan hız bileşeni
        Vector2 lateralVel = Vector2.Dot(rb.linearVelocity, right) * right;
        // Tersi yönde kuvvet uygula
        rb.AddForce(-lateralVel * lateralFrictionStrength, ForceMode2D.Force);
    }

    void ApplyForwardDrag()
    {
        // Tekerin ileri yönü
        Vector2 forward = transform.up;
        Vector2 forwardVel = Vector2.Dot(rb.linearVelocity, forward) * forward;
        // Hafif ileri direnç (rüzgar / yer sürtünmesi)
        rb.AddForce(-forwardVel * forwardFrictionStrength, ForceMode2D.Force);
    }
}
