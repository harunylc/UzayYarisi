using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CarPowerUpHandler : MonoBehaviour
{
    // enum yerine string kullanıyoruz. "None", "Gravity", "DarkScreen" gibi metinler tutacak.
    private string currentPowerUp = "None";

    private Rigidbody2D rb;
    
    private DriveMyCar driveScriptP1;
    private DriveMyCar_Player2 driveScriptP2;
    private float originalNitroRechargeRate; // Orijinal değeri saklamak için

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Bu arabanın üzerinde hangi sürüş script'i varsa onu bul ve sakla
        driveScriptP1 = GetComponent<DriveMyCar>();
        driveScriptP2 = GetComponent<DriveMyCar_Player2>();
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
        if (currentPowerUp == "Gravity") UseGravityPowerUp();
        else if (currentPowerUp == "DarkScreen") UseDarkScreenPowerUp();
        else if (currentPowerUp == "Nitro") UseNitroPowerUp(); // YENİ
        
        currentPowerUp = "None"; // Her güçlendirme tek kullanımlıktır
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
    private void UseNitroPowerUp()
    {
        Debug.Log($"{gameObject.name} Nitro güçlendirmesini kullandı!");
        StartCoroutine(NitroBoostRoutine(10f)); // 10 saniyelik etki
    }
    private IEnumerator NitroBoostRoutine(float duration)
    {
        // Önce arabanın üzerinde hangi sürüş script'i olduğunu bulalım
        if (driveScriptP1 != null)
        {
            // Player 1 arabası
            originalNitroRechargeRate = 15f; // Orijinal değeri biliyoruz (script'ten)
            float boostedRate = originalNitroRechargeRate * 1.5f; // %50 artır
            driveScriptP1.SetNitroRechargeRate(boostedRate);
            Debug.Log($"P1 Nitro dolum hızı {boostedRate} oldu.");
        }
        else if (driveScriptP2 != null)
        {
            // Player 2 arabası
            originalNitroRechargeRate = 15f; // Orijinal değeri biliyoruz
            float boostedRate = originalNitroRechargeRate * 1.5f;
            driveScriptP2.SetNitroRechargeRate(boostedRate);
            Debug.Log($"P2 Nitro dolum hızı {boostedRate} oldu.");
        }

        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(duration);

        // Süre bittiğinde, her şeyi normale döndür
        if (driveScriptP1 != null)
        {
            driveScriptP1.SetNitroRechargeRate(originalNitroRechargeRate);
            Debug.Log($"P1 Nitro dolum hızı normale döndü: {originalNitroRechargeRate}.");
        }
        else if (driveScriptP2 != null)
        {
            driveScriptP2.SetNitroRechargeRate(originalNitroRechargeRate);
            Debug.Log($"P2 Nitro dolum hızı normale döndü: {originalNitroRechargeRate}.");
        }
    }
}