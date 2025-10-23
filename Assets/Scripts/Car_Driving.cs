using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input System için gerekli

public class Car_Driving : MonoBehaviour
{
    [SerializeField] private Rigidbody2D tireBackRb;
    [SerializeField] private Rigidbody2D tireFrontRb;
    [SerializeField] private Rigidbody2D carRb;
    [SerializeField] private float carRotation = 300f;  
    [SerializeField] private float speed = 150f;

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
        
        controls.Move.Options.performed += OnOptions;
    }
    
    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<float>();
    }

    private void OnOptions(InputAction.CallbackContext context)
    {
        if (settingsPanel.activeSelf)
        {
            settingsPanel.SetActive(false);
        }
        else
        {
            settingsPanel.SetActive(true);
        }
        Debug.Log("Ayarlar Açıldı");
    }

    private void OnEnable()
    {
        controls.Move.Enable();
    }

    private void OnDisable()
    {
        controls.Move.Disable();
    }

    private void FixedUpdate()
    {
        tireFrontRb.AddTorque(-moveInput * speed * Time.fixedDeltaTime);
        tireBackRb.AddTorque(-moveInput * speed * Time.fixedDeltaTime);
        
        carRb.AddTorque(moveInput * carRotation * Time.fixedDeltaTime);
    }
}