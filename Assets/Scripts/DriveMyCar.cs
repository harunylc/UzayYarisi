using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class DriveMyCar : MonoBehaviour
{
    [Header("Tekerlek Ayarları")]
    // Buraya tekerlek Rigidbody'lerini sürükle.
    // ÖNEMLİ: Sıralama şöyledir -> Element 0: ARKA Teker, Element 1: ÖN Teker
    public List<Rigidbody2D> drivingWheels = new List<Rigidbody2D>(); 

    [Header("Car Settings")]
    [SerializeField] private Rigidbody2D carRb;
    public float carRotationSpeed = 100f; // Şahlanmayı önlemek için bunu makul seviyede tut
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

    [Header("Raycast Settings (Sadece Skor/Nitro İçin)")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength = 45f;
    private bool hasScoredForFlip = false;
    
    [Header("Trigger Swap System")]
    private bool swapTriggers = false;
    
    [Header("Car Sound")]
    private AudioSource currentGasSound;
    private AudioSource currentBrakeSound;

    private float moveInput;
    // isGrounded değişkeni SİLİNDİ.
    
    private bool controlsInverted = false;

    private void Start()
    {
        currentNitro = maxNitro;

        nitroSlider = GameObject.FindGameObjectWithTag("NitroSliderP1")?.GetComponent<Slider>();
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

        moveInput = input;
        
        // --- SES KODLARI ---
        if (AudioManager.Instance == null) return;

        float deadZone = 0.1f;

        if (input > deadZone) 
        {
            StopBrakeSound();
            if (currentGasSound == null)
            {
                SoundSO gasSO = AudioManager.Instance.SoundsCollection.CarSounds[0];
                currentGasSound = AudioManager.Instance.SoundToPlay(gasSO);
            }
        }
        else if (input < -deadZone)
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

    private void FixedUpdate()
    {
        // Nitro doldurmak için yere yakınlık kontrolü (Fizik için değil, skor için)
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

        // --- NITRO MANTIĞI ---
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
        // Ön tekerlek arabayı çeksin, arka tekerlek hafif itsin. Bu şahlanmayı önler.
        for (int i = 0; i < drivingWheels.Count; i++)
        {
            if (drivingWheels[i] != null)
            {
                float powerMultiplier = 1.0f;

                // Eğer liste boyutu 2 ise (Standart Araba)
                if (drivingWheels.Count == 2)
                {
                    if (i == 1) powerMultiplier = 1.2f; // Ön Tekerlek (Daha Güçlü)
                    if (i == 0) powerMultiplier = 0.8f; // Arka Tekerlek (Daha Zayıf)
                }
                
                drivingWheels[i].AddTorque(-moveInput * currentSpeed * powerMultiplier, ForceMode2D.Force);
            }
        }

        // --- ROTASYON ---
        // GroundCheck olmadığı için her zaman çalışır.
        // Şahlanmayı önlemek için Inspector'da Rigidbody -> Angular Drag değerini artır (3-5 yap).
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
        PowerUpManager.Instance.UsePowerUp_P1();
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