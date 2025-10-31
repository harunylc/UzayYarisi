using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Car_Driving : MonoBehaviour
{
    [Header("Car Settings")]
    [SerializeField] private Rigidbody2D tireBackRb;
    [SerializeField] private Rigidbody2D tireFrontRb;
    [SerializeField] private Rigidbody2D carRb;
    [SerializeField] private float carRotation = 300f;
    [SerializeField] private float speed = 150f;
    
    [Header("Debug")]
    [SerializeField]private float currentSpeed;
    
    [Header("Nitro Settings")]
    private bool nitroActive = false;
    [SerializeField] private Slider nitroSlider;
    //[SerializeField] private float nitroSpeed = 300f;
    [SerializeField] private float maxNitro = 100f;
    [SerializeField] private float nitroDrainRate = 30f;
    [SerializeField] private float nitroRechargeRate = 15f; 
    [SerializeField] private float nitroBoost = 300f;
    private float currentNitro;
    
    [Header("Rotation Settings")]
    private float totalRotation = 0f;
    private float lastRotation = 0f;
    private int flipCount = 0;

    [Header("Other Settings")]
    private CarController controls;
    private float moveInput;
    [SerializeField] private GameObject settingsPanel;
    private bool isNewInputSystemActive = false;
    
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength = 45f;
    private bool hasScoredForFlip = false;

    private void Awake()
    {
        try
        {
            controls = new CarController();
            isNewInputSystemActive = true;

            controls.Move.P1_Throtle.performed += OnMove;
            controls.Move.P1_Throtle.canceled += OnMove;

            // controls.Move.Options.performed += OnOptions;
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

    // private void OnOptions(InputAction.CallbackContext context)
    // {
    //     if (settingsPanel == null)
    //     {
    //         Debug.LogWarning("Ayarlar paneli atanmamış!");
    //         return;
    //     }
    //
    //     settingsPanel.SetActive(!settingsPanel.activeSelf);
    //     Debug.Log("Ayarlar Açıldı/Kapandı");
    // }

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

    private void FixedUpdate()
    {
        Vector2 rayOrigin = (Vector2)transform.position + Vector2.up * 1f;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up, rayLength, groundLayer);
        Debug.DrawRay(rayOrigin, transform.up * rayLength, Color.red);
    
        if (hit.collider != null && !hasScoredForFlip)
        {
            currentNitro = Mathf.Min(currentNitro + 10f, maxNitro);
            hasScoredForFlip = true;
            Debug.Log("Takla bonusu! Nitro +10");
        }
        else if (hit.collider == null)
        {
            hasScoredForFlip = false;
        }
        
        float currentMoveInput = 0f;

        if (isNewInputSystemActive)
        {
            currentMoveInput = moveInput;
        }

        if (!isNewInputSystemActive || Mathf.Approximately(currentMoveInput, 0f))
        {
            currentMoveInput = Input.GetAxis("Horizontal");
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
        
        tireFrontRb.AddTorque(-currentMoveInput * speed);
        tireBackRb.AddTorque(-currentMoveInput * speed);

        carRb.AddTorque(currentMoveInput * carRotation * Time.fixedDeltaTime);
        
        //Debug.Log($"Nitro: {nitroActive} | Mevcut hız: {currentSpeed}");

    }
    
}