using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class DriveMyCar_Player2 : MonoBehaviour
{
    [Header("Tekerlek Ayarları")]
    // Element 0: ARKA, Element 1: ÖN
    public List<Rigidbody2D> drivingWheels = new List<Rigidbody2D>();

    [Header("Car Settings")]
    [SerializeField] private Rigidbody2D carRb;
    public float carRotationSpeed = 100f;
    [SerializeField] public float speed = 150f;
    [SerializeField] private float currentSpeed;

    [Header("Nitro Settings")]
    private bool nitroActive = false;
    [SerializeField] private Slider nitroSlider;
    [SerializeField] private float maxNitro = 100f;
    [SerializeField] private float nitroDrainRate = 30f;
    public float nitroRechargeRate = 15f; 
    public float nitroBoost = 300f;
    private float currentNitro;

    [Header("Nitro Particle")]
    [SerializeField] private ParticleSystem nitroParticle;

    // GroundCheck listesi SİLİNDİ.

    [Header("Raycast Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength = 45f;
    private bool hasScoredForFlip = false;
    
    [Header("Trigger Swap System")]
    private bool swapTriggers = false;
    
    [Header("Car Sound")]
    private AudioSource currentGasSound;
    private AudioSource currentBrakeSound;

    private float moveInput;
    // isGrounded SİLİNDİ.
    
    private bool controlsInverted = false;

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
        
    public void InvertTriggers(bool state)
    {
        swapTriggers = state;
    }

    public void OnThrotle(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();

        if (controlsInverted) input = -input;
        if (swapTriggers) input *= -1f;

        moveInput = -input;
        
        if (AudioManager.Instance == null) return;

        float deadZone = 0.1f;

        if (-input > deadZone)
        {
            StopBrakeSound(); 
            if (currentGasSound == null)
            {
                SoundSO gasSO = AudioManager.Instance.SoundsCollection.CarSounds[0];
                currentGasSound = AudioManager.Instance.SoundToPlay(gasSO);
            }
        }
        else if (-input < -deadZone)
        {
            StopGasSound(); 
            if (currentBrakeSound == null)
            {
                SoundSO brakeSO = AudioManager.Instance.SoundsCollection.CarSounds[1];
                currentBrakeSound = AudioManager.Instance.SoundToPlay(brakeSO);
            }
        }
        else
        {
            StopGasSound();
            StopBrakeSound();
        }
    }
    
    public void SetReverseControls(bool state)
    {
        controlsInverted = state;
    }

    public void OnNitro(InputAction.CallbackContext context)
    {
        if (context.performed) TryActivateNitro(true);
        else if (context.canceled) TryActivateNitro(false);
    }
    
    public void InvertControls(bool state)
    {
        controlsInverted = state;
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

        // --- NITRO ---
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
        if (nitroSlider != null) nitroSlider.value = currentNitro;

        if (currentNitro <= 0f && nitroActive) TryActivateNitro(false);

        // --- MOTOR GÜCÜ (AKILLI ÇEKİŞ SİSTEMİ) ---
        for (int i = 0; i < drivingWheels.Count; i++)
        {
            if (drivingWheels[i] != null)
            {
                float powerMultiplier = 1.0f;
                // Eğer 2 Tekerlek varsa (Standart)
                if (drivingWheels.Count == 2)
                {
                    if (i == 1) powerMultiplier = 1.2f; // Ön Teker
                    if (i == 0) powerMultiplier = 0.8f; // Arka Teker
                }
                drivingWheels[i].AddTorque(moveInput * currentSpeed * powerMultiplier, ForceMode2D.Force);
            }
        }

        // --- ROTASYON ---
        carRb.AddTorque(moveInput * carRotationSpeed, ForceMode2D.Force);
    }

    private void TryActivateNitro(bool active)
    {
        if (active && currentNitro <= 0f) return;

        nitroActive = active;

        if (nitroParticle != null)
        {
            if (active && currentNitro > 0f) nitroParticle.Play();
            else nitroParticle.Stop();
        }
    }
    
    public void OnPowerUp()
    {
        PowerUpManager.Instance.UsePowerUp_P2();
    }
    
    private void StopGasSound()
    {
        if (currentGasSound != null)
        {
            currentGasSound.Stop();
            Destroy(currentGasSound.gameObject);
            currentGasSound = null;
        }
    }

    private void StopBrakeSound()
    {
        if (currentBrakeSound != null)
        {
            currentBrakeSound.Stop();
            Destroy(currentBrakeSound.gameObject);
            currentBrakeSound = null;
        }
    }
}