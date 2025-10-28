using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Car_Driving : MonoBehaviour
{
    [SerializeField] private Rigidbody2D tireBackRb;
    [SerializeField] private Rigidbody2D tireFrontRb;
    [SerializeField] private Rigidbody2D carRb;
    [SerializeField] private float carRotation = 300f;
    [SerializeField] private float speed = 150f;

    [SerializeField] private GameObject settingsPanel;
    
    private float totalRotation = 0f;
    private float lastRotation = 0f;
    private int flipCount = 0;

    private CarController controls;
    private float moveInput;

    private bool isNewInputSystemActive = false;

    private void Awake()
    {
        try
        {
            controls = new CarController();
            isNewInputSystemActive = true;

            controls.Move.P1_Throtle.performed += OnMove;
            controls.Move.P1_Throtle.canceled += OnMove;

            controls.Move.Options.performed += OnOptions;
        }
        catch (Exception e)
        {
            isNewInputSystemActive = false;
            controls = null;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    private void OnOptions(InputAction.CallbackContext context)
    {
        if (settingsPanel == null)
        {
            Debug.LogWarning("Ayarlar paneli atanmamış!");
            return;
        }

        settingsPanel.SetActive(!settingsPanel.activeSelf);
        Debug.Log("Ayarlar Açıldı/Kapandı");
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

        tireFrontRb.AddTorque(-currentMoveInput * speed );
        tireBackRb.AddTorque(-currentMoveInput * speed);

        carRb.AddTorque(currentMoveInput * carRotation * Time.fixedDeltaTime);
        
        float rotation = carRb.rotation;
        float deltaRotation = Mathf.DeltaAngle(lastRotation, rotation);
        totalRotation += deltaRotation;
        lastRotation = rotation;

        if (Mathf.Abs(totalRotation) >= 360f)
        {
            flipCount++;
            totalRotation = 0f;
            Debug.Log($"🔥 Takla sayısı: {flipCount}");
        }

    }
}