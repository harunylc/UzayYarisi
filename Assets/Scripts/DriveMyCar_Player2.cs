using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DriveMyCar_Player2 : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private Rigidbody2D tireBackRb;
    [SerializeField] private Rigidbody2D tireFrontRb;
    [SerializeField] private Rigidbody2D carRb;
    [SerializeField] private float carRotationSpeed = 100f;
    [SerializeField] private float speed = 150f;
    [SerializeField] private float currentSpeed;

    [Header("Nitro Settings")]
    private bool nitroActive = false;
    [SerializeField] private Slider nitroSlider;
    [SerializeField] private float maxNitro = 100f;
    [SerializeField] private float nitroDrainRate = 30f;
    [SerializeField] private float nitroRechargeRate = 15f;
    [SerializeField] private float nitroBoost = 300f;
    private float currentNitro;

    [Header("Nitro Particle")]
    [SerializeField] private ParticleSystem nitroParticle;

    [Header("Ground Check")]
    [SerializeField] public TireGrounded tireGrounded;

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength = 45f;
    private bool hasScoredForFlip = false;

    private float moveInput;
    private bool isGrounded;

    private void Start()
    {
        currentNitro = maxNitro;

        nitroSlider = GameObject.FindGameObjectWithTag("NitroSliderP2")?.GetComponent<Slider>();
        if (nitroSlider != null)
        {
            nitroSlider.maxValue = maxNitro;
            nitroSlider.value = currentNitro;
        }

        if (nitroParticle != null) nitroParticle.Stop();
    }

    public void OnThrotle(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    public void OnNitro(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TryActivateNitro(true);
        }
        else if (context.canceled)
        {
            TryActivateNitro(false);
        }
    }

    private void FixedUpdate()
    {
        Vector2 rayOrigin = (Vector2)transform.position + Vector2.up * 1f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up, rayLength, groundLayer);
        Debug.DrawRay(rayOrigin, transform.up * rayLength, Color.red);

        if (hit.collider != null && !hasScoredForFlip)
        {
            currentNitro = Mathf.Min(currentNitro + 10f, maxNitro);
            hasScoredForFlip = true;
        }
        else if (hit.collider == null)
        {
            hasScoredForFlip = false;
        }

        if (tireGrounded != null)
        {
            isGrounded = tireGrounded.tireGrounded;
        }

        bool canUseNitro = nitroActive && currentNitro > 0f && moveInput > 0f;

        if (canUseNitro)
        {
            currentSpeed = speed + nitroBoost;
            currentNitro -= nitroDrainRate * Time.fixedDeltaTime;
        }
        else
        {
            currentSpeed = speed;
            currentNitro += nitroRechargeRate * Time.fixedDeltaTime;
        }

        currentNitro = Mathf.Clamp(currentNitro, 0f, maxNitro);
        if (nitroSlider != null)
        {
            nitroSlider.value = currentNitro;
        }

        if (currentNitro <= 0f && nitroActive)
        {
            TryActivateNitro(false);
        }

        tireFrontRb.AddTorque(-moveInput * currentSpeed, ForceMode2D.Force);
        tireBackRb.AddTorque(-moveInput * currentSpeed, ForceMode2D.Force);

        float currentRotation = isGrounded ? carRotationSpeed / 5f : carRotationSpeed;
        carRb.AddTorque(moveInput * currentRotation, ForceMode2D.Force);
    }

    private void TryActivateNitro(bool active)
    {
        if (active && currentNitro <= 0f)
        {
            return;
        }

        nitroActive = active;

        if (nitroParticle != null)
        {
            if (active && currentNitro > 0f)
            {
                nitroParticle.Play();
            }
            else
            {
                nitroParticle.Stop();
            }
        }
    }
}
