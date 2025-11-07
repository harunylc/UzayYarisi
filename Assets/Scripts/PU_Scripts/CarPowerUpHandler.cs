using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CarPowerUpHandler : MonoBehaviour
{
    // enum yerine string kullanıyoruz. "None", "Gravity", "DarkScreen" gibi metinler tutacak.
    private string currentPowerUp = "None";

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Eğer bir güçlendirmemiz varsa ve oyuncu tuşa bastıysa...
        if (currentPowerUp != "None" && Gamepad.current != null && Gamepad.current.leftShoulder.wasPressedThisFrame)
        {
            UsePowerUp();
        }
    }

    // Güçlendirme objeleri bu fonksiyonu çağırarak güçlendirmenin adını "verir".
    public void GivePowerUp(string powerUpName)
    {
        Debug.Log($"{gameObject.name}, {powerUpName} güçlendirmesini aldı!");
        currentPowerUp = powerUpName;
    }

    private void UsePowerUp()
    {
        // Hangi güçlendirmeye sahip olduğumuza göre doğru fonksiyonu çağır.
        if (currentPowerUp == "Gravity")
        {
            UseGravityPowerUp();
        }
        else if (currentPowerUp == "DarkScreen")
        {
            UseDarkScreenPowerUp();
        }
        
        // Güçlendirmeyi kullandıktan sonra sıfırla.
        currentPowerUp = "None";
    }

    // --- ÖZEL GÜÇLENDİRME FONKSİYONLARI ---

    private void UseGravityPowerUp()
    {
        if (rb == null) return;
        float originalMass = rb.mass;
        rb.mass *= 0.5f;
        Debug.Log($"{gameObject.name} Gravity güçlendirmesini kullandı! Kütle: {originalMass} -> {rb.mass}");
    }

    private void UseDarkScreenPowerUp()
    {
        Debug.Log($"{gameObject.name} DarkScreen güçlendirmesini kullandı!");
        string targetTag = (gameObject.CompareTag("Player")) ? "Player2" : "Player";
        GameObject opponent = GameObject.FindWithTag(targetTag);

        if (opponent != null)
        {
            PU_DarkScreen opponentScreen = opponent.GetComponentInChildren<PU_DarkScreen>(true);
            if (opponentScreen != null)
            {
                opponentScreen.Activate(5f);
            }
        }
    }
}