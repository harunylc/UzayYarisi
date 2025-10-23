using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input System için gerekli
using UnityEngine.UI;

public class Car_Driving : MonoBehaviour
{
    [SerializeField] private Rigidbody2D tireBackRb;
    [SerializeField] private Rigidbody2D tireFrontRb;
    [SerializeField] private Rigidbody2D carRb;
    [SerializeField] private float carRotation = 300f;  
    [SerializeField] private float speed = 150f;
    //Nitro
    [SerializeField] private float nitroForce = 300f;
    [SerializeField] private float maxNitro = 100f;
    [SerializeField] private float nitroDrain = 30f;
    [SerializeField] private float nitroRecharge = 10f;
    [SerializeField] private Slider nitroSlider;
    private bool isUsingNitro;
    private float currentNitro;
    //Raycast
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLength = 15f; 
    
    [SerializeField] private GameObject settingsPanel;

    private CarController controls;
    private float moveInput;

    private void Awake()
    {
        controls = new CarController();

        controls.Move.P1_Throtle.performed += OnMove;
        controls.Move.P1_Throtle.canceled += OnMove;
        controls.Move.P2_Throtle.performed += OnMove;
        controls.Move.P2_Throtle.canceled += OnMove;

        //Nitro
        controls.Move.P1_Nitro.performed += ctx => StartNitro();
        controls.Move.P1_Nitro.canceled += ctx => StopNitro();

        controls.Move.Options.performed += OnOptions;
    }

    private void Start()
    {
        currentNitro = maxNitro;
        if (nitroSlider != null)
        {
            nitroSlider.maxValue = maxNitro;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    private void OnOptions(InputAction.CallbackContext context)
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
        Debug.Log("Ayarlar Açıldı");
    }

    private void OnEnable() => controls.Move.Enable();
    private void OnDisable() => controls.Move.Disable();

    private void FixedUpdate()
    {
        //Raycast
        Vector2 rayOrigin = transform.position;
        Vector2 rayDirection = transform.up;
        
        Debug.DrawRay(rayOrigin, rayDirection * rayLength, Color.yellow, 0.02f);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayLength, groundLayer);

        //Nitro hesaplama
        float totalSpeed = speed;

        if (isUsingNitro && currentNitro > 0)
        {
            totalSpeed += nitroForce;
            currentNitro -= nitroDrain * Time.fixedDeltaTime;
            if (currentNitro <= 0)
            {
                currentNitro = 0;
                StopNitro();
            }
        }
        else if (!isUsingNitro && currentNitro < maxNitro)
        {
            currentNitro += nitroRecharge * Time.fixedDeltaTime;
            if (currentNitro > maxNitro) currentNitro = maxNitro;
        }

        if (nitroSlider != null)
            nitroSlider.value = currentNitro;

        //Takla kontrolü
        if (hit.collider != null)
        {
            currentNitro += 10f;
            if (currentNitro > maxNitro)
            {
                currentNitro = maxNitro;
            }

            Debug.Log("Nitro +10");
        }
        
        tireFrontRb.AddTorque(-moveInput * totalSpeed * Time.fixedDeltaTime);
        tireBackRb.AddTorque(-moveInput * totalSpeed * Time.fixedDeltaTime);
        carRb.AddTorque(moveInput * carRotation * Time.fixedDeltaTime);
    }

    private void StartNitro()
    {
        if (currentNitro > 0)
        {
            isUsingNitro = true;
        }
    }

    private void StopNitro()
    {
        {
            isUsingNitro = false;
        }
    }
}
