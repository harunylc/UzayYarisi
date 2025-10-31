using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DriveMyCar : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private Rigidbody2D tireBackRb;
    [SerializeField] private Rigidbody2D tireFrontRb;
    [SerializeField] private Rigidbody2D carRb;
    
    // DriveMyCar'dan alınan değerler/yaklaşımlar
    [SerializeField] private float carRotationSpeed = 100f; // DriveMyCar'daki rotationSpeed
    [SerializeField] private float speed = 150f;
    
    [Header("Debug")]
    [SerializeField]private float currentSpeed;
    
    [Header("Nitro Settings")]
    private bool nitroActive = false;
    [SerializeField] private Slider nitroSlider;
    [SerializeField] private float maxNitro = 100f;
    [SerializeField] private float nitroDrainRate = 30f;
    [SerializeField] private float nitroRechargeRate = 15f; 
    [SerializeField] private float nitroBoost = 300f;
    private float currentNitro;
    
    [Header("Rotation Settings")]
    // Takla sayımı için gerekli ancak bu revizyonda kullanılmayacak.
    private float totalRotation = 0f;
    private float lastRotation = 0f;
    private int flipCount = 0;

    [Header("Ground Check (TireGrounded Entegrasyonu)")]
    
    [SerializeField] public TireGrounded tireGrounded;

    [Header("Other Settings")]
    private CarController controls;
    private float moveInput;
    [SerializeField] private GameObject settingsPanel;
    private bool isNewInputSystemActive = false;
    
    // Raycast ayarları kaldırıldı

    private void Awake()
    {
        try
        {
            controls = new CarController();
            isNewInputSystemActive = true;

            controls.Move.P1_Throtle.performed += OnMove;
            controls.Move.P1_Throtle.canceled += OnMove;

            controls.Move.P1_Nitro.performed += ctx => nitroActive = true;
            controls.Move.P1_Nitro.canceled += ctx => nitroActive = false;
        }
        catch (Exception e)
        {
            isNewInputSystemActive = false;
            controls = null;
        }
    }

    private void Start()
    {
        currentNitro = maxNitro;
        if (nitroSlider != null)
        {
            nitroSlider.maxValue = maxNitro;
            nitroSlider.value = currentNitro;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    private void OnEnable()
    {
        if (isNewInputSystemActive)
        {
            controls.Move.Enable();
        }
    }

    private void OnDisable()
    {
        if (isNewInputSystemActive)
        {
            controls.Move.Disable();
        }
    }
    private bool isGrounded = false;
    private void FixedUpdate()
    {
        float currentMoveInput = 0f;

        if (isNewInputSystemActive)
        {
            currentMoveInput = moveInput;
        }

        if (!isNewInputSystemActive || Mathf.Approximately(currentMoveInput, 0f))
        {
            currentMoveInput = Input.GetAxis("Horizontal");
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
        
        
        tireFrontRb.AddTorque(-currentMoveInput * currentSpeed ,ForceMode2D.Force);
        tireBackRb.AddTorque(-currentMoveInput * currentSpeed ,ForceMode2D.Force);

        
        float currentRotation = carRotationSpeed;
        if (isGrounded)
        {
            currentRotation = carRotationSpeed / 5f; 
        }
        else
        {
            currentRotation = carRotationSpeed;
        }
        carRb.AddTorque(currentMoveInput * currentRotation ,ForceMode2D.Force);
        
        Debug.Log($"Nitro: {nitroActive} | Mevcut hız: {currentSpeed} | Yerde: {isGrounded}");
    }
}