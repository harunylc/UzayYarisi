using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input System için gerekli

public class Car_Driving : MonoBehaviour
{
    [SerializeField] private Rigidbody2D tireBackRb;
    [SerializeField] private Rigidbody2D tireFrontRb;
    [SerializeField] private Rigidbody2D carRb;
    [SerializeField] private float carRotation = 300f;  
    [SerializeField] private float speed = 150f;

    // Otomatik oluşturulan C# Class'ına referans
    private CarController controls;
    
    // FixedUpdate'te kullanmak üzere hareket değerini tutacak değişken
    private float moveInput;

    private void Awake()
    {
        // Otomatik oluşturulan Input Actions class'ının örneğini oluştur
        controls = new CarController();

        // Hareket Action'ına abone ol (RT/LT'ye basıldığında/değeri değiştiğinde)
        // Value Action'ları için genellikle .performed event'ini kullanırız.
        controls.Move.Throtle.performed += OnMove;
        
        // Tetikleyiciler bırakıldığında (hız 0'a düştüğünde)
        controls.Move.Throtle.canceled += OnMove;
    }
    
    // Input Action'ın geri çağırma metodu
    private void OnMove(InputAction.CallbackContext context)
    {
        // Tetikleyici değeri okundu. (RT: 0..1, LT: 0..-1)
        // Değer değiştiği anda (performed) veya bırakıldığı anda (canceled) çalışır.
        moveInput = context.ReadValue<float>();
    }

    private void OnEnable()
    {
        // Action Map'i etkinleştir
        controls.Move.Enable();
    }

    private void OnDisable()
    {
        controls.Move.Disable();
    }

    private void FixedUpdate()
    {
        // moveInput değeri RT/LT durumuna göre -1 ile 1 arasında bir değer tutar.
        
        // Tekerleklere tork uygulama
        tireFrontRb.AddTorque(-moveInput * speed * Time.fixedDeltaTime);
        tireBackRb.AddTorque(-moveInput * speed * Time.fixedDeltaTime);
        
        // Arabanın havada dengelenmesi için tork uygulama
        carRb.AddTorque(moveInput * carRotation * Time.fixedDeltaTime);
    }
}