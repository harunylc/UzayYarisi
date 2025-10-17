using System;
using UnityEngine;

public class Car_Driving : MonoBehaviour
{
    [SerializeField] private Rigidbody2D tireBackRb;
    [SerializeField] private Rigidbody2D tireFrontRb;
    [SerializeField] private Rigidbody2D carRb;
    [SerializeField] private float carRotation = 600f;  
    [SerializeField] private float speed = 150f;

    private float moveInput;

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        tireFrontRb.AddTorque(-moveInput*speed*Time.fixedDeltaTime);
        tireBackRb.AddTorque(-moveInput*speed*Time.fixedDeltaTime);
        carRb.AddTorque(moveInput*carRotation*Time.fixedDeltaTime);
    }
}
