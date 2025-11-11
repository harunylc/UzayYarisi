using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CarPowerUpHandler : MonoBehaviour
{
    private string currentPowerUp = "None";

    private PU_DarkScreen player1_darkPanel;
    private PU_DarkScreen player2_darkPanel;

    private Rigidbody2D rb;
    private DriveMyCar driveScriptP1;
    private DriveMyCar_Player2 driveScriptP2;
    private float originalNitroRechargeRate;


    void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        driveScriptP1 = GetComponent<DriveMyCar>();
        driveScriptP2 = GetComponent<DriveMyCar_Player2>();
    }

    void Start()
    {
        Debug.LogError("--- CarPowerUpHandler: Paneller aranıyor... ---");
        GameObject p1PanelObject = GameObject.FindWithTag("P1_DarkPanel");
        if (p1PanelObject != null)
        {
            player1_darkPanel = p1PanelObject.GetComponent<PU_DarkScreen>();
            if(player1_darkPanel != null)
                Debug.LogError("BAŞARILI: P1_DarkPanel bulundu ve script'i alındı.");
            else
                Debug.LogError("!!!!!! HATA: P1_DarkPanel objesi bulundu ama üzerinde PU_DarkScreen script'i YOK! !!!!!!");
        }
        else
        {
            Debug.LogError("!!!!!! HATA: 'P1_DarkPanel' etiketine sahip hiçbir obje sahnede bulunamadı! !!!!!!");
        }

        GameObject p2PanelObject = GameObject.FindWithTag("P2_DarkPanel");
        if (p2PanelObject != null)
        {
            player2_darkPanel = p2PanelObject.GetComponent<PU_DarkScreen>();
            if(player2_darkPanel != null)
                Debug.LogError("BAŞARILI: P2_DarkPanel bulundu ve script'i alındı.");
            else
                Debug.LogError("!!!!!! HATA: P2_DarkPanel objesi bulundu ama üzerinde PU_DarkScreen script'i YOK! !!!!!!");
        }
        else
        {
            Debug.LogError("!!!!!! HATA: 'P2_DarkPanel' etiketine sahip hiçbir obje sahnede bulunamadı! !!!!!!");
        }
    }
    public void OnUsePowerUp(InputAction.CallbackContext context)
    {
        if (context.performed && currentPowerUp != "None")
        {
            UsePowerUp();
        }
    }
   // void Update()
   // {
   //     // 1. Gamepad'i kontrol et
   //     if (Gamepad.current == null) return; // Gamepad yoksa hiçbir şey yapma
   //
   //     // 2. Tuşa basıldı mı diye kontrol et
   //     if (Gamepad.current.leftShoulder.wasPressedThisFrame)
   //     {
   //         Debug.LogError("--- SOL BUMPER TUŞUNA BASILDIĞI ALGILANDI ---");
   //
   //         // 3. Güçlendirme var mı diye kontrol et
   //         if (currentPowerUp != "None")
   //         {
   //             Debug.LogError($"-> Güçlendirme var: '{currentPowerUp}'. Kullanılıyor...");
   //             UsePowerUp();
   //         }
   //         else
   //         {
   //             Debug.LogError("-> Ama 'currentPowerUp' = 'None'. Cepte güçlendirme yok.");
   //         }
   //     }
   // }
    public void GivePowerUp(string powerUpName)
    {
        currentPowerUp = powerUpName;
    }

    private void UsePowerUp()
    {
        // Hangi güce sahip olduğunu kontrol et ve ilgili fonksiyonu çağır.
        string powerUpToUse = currentPowerUp;
        currentPowerUp = "None"; // GÜCÜ HEMEN 'NONE' YAP Kİ TEKRAR KULLANILAMASIN

        if (powerUpToUse == "Gravity") UseGravityPowerUp();
        else if (powerUpToUse == "DarkScreen") UseDarkScreenPowerUp();
        else if (powerUpToUse == "Nitro") UseNitroPowerUp();
    }


    private void UseGravityPowerUp()
    {
        if (rb == null) return;
        rb.mass *= 0.5f;
    }

    private void UseDarkScreenPowerUp()
    {
    
        PU_DarkScreen targetPanel = null;
        if (gameObject.CompareTag("Player"))
        {
            targetPanel = player2_darkPanel;
        }
        else if (gameObject.CompareTag("Player2"))
        {
            targetPanel = player1_darkPanel;
        }

        if (targetPanel != null)
        {
            StartCoroutine(targetPanel.DarkenScreenRoutine(5f));
        }
        else
        {
            Debug.LogError("Hedef Dark Screen paneli bulunamadı!");
        }
    }

    private void UseNitroPowerUp()
    {
        StartCoroutine(NitroBoostRoutine(10f));
    }
    
    private IEnumerator NitroBoostRoutine(float duration)
    {
        if (driveScriptP1 != null)
        {
            originalNitroRechargeRate = driveScriptP1.GetNitroRechargeRate();
            float boostedRate = originalNitroRechargeRate * 1.5f;
            driveScriptP1.SetNitroRechargeRate(boostedRate);
            
            yield return new WaitForSeconds(duration);

            driveScriptP1.SetNitroRechargeRate(originalNitroRechargeRate);
        }
        else if (driveScriptP2 != null)
        {
            originalNitroRechargeRate = driveScriptP2.GetNitroRechargeRate();
            float boostedRate = originalNitroRechargeRate * 1.5f;
            driveScriptP2.SetNitroRechargeRate(boostedRate);

            yield return new WaitForSeconds(duration);

            driveScriptP2.SetNitroRechargeRate(originalNitroRechargeRate);
        }
    }
}