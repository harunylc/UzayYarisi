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

    [Header("Ground Check")]
    [SerializeField] public TireGrounded tireGrounded;

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
    private bool isGrounded;
    
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

        if (controlsInverted)
            input = -input;

        if (swapTriggers)
            input *= -1f;

        moveInput = -input;
        
        if (AudioManager.Instance == null) return;

        float deadZone = 0.1f;

        // DURUM A: Input POZİTİF ise (R2 - Gaz)
        if (-input > deadZone)
        {
            StopBrakeSound(); // Fren varsa sustur

            if (currentGasSound == null)
            {
                // Array[0] -> GAZ Sesi
                SoundSO gasSO = AudioManager.Instance.SoundsCollection.CarSounds[0];
                currentGasSound = AudioManager.Instance.SoundToPlay(gasSO);
            }
        }
        // DURUM B: Input NEGATİF ise (L2 - Fren/Geri)
        else if (-input < -deadZone)
        {
            StopGasSound(); // Gaz varsa sustur

            if (currentBrakeSound == null)
            {
                // Array[1] -> FREN Sesi
                SoundSO brakeSO = AudioManager.Instance.SoundsCollection.CarSounds[1];
                currentBrakeSound = AudioManager.Instance.SoundToPlay(brakeSO);
            }
        }
        // DURUM C: Input SIFIR ise (Elimizi çektik)
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
        if (context.performed)
        {
            TryActivateNitro(true);
        }
        else if (context.canceled)
        {
            TryActivateNitro(false);
        }
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

        tireFrontRb.AddTorque(moveInput * currentSpeed, ForceMode2D.Force);
        tireBackRb.AddTorque(moveInput * currentSpeed, ForceMode2D.Force);

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
