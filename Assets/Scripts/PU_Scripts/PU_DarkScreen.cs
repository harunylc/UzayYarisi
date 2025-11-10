using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PU_DarkScreen : MonoBehaviour
{
    [Header("Rol Ayarları")]
    public bool isCollectableObject;
    
    [Header("Panel Ayarları (Eğer Panel ise)")]
    public Image panelImage;

    // --- ROL 1: Toplanabilir Obje Olarak Çalışma ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollectableObject) return; // Eğer panel ise, bu kodu çalıştırma.
        
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
            if (carHandler != null)
            {
                carHandler.GivePowerUp("DarkScreen");
                Destroy(gameObject);
            }
        }
    }

    // --- ROL 2: Power-Up Yöneticisi Tarafından Başlatılacak Olan Coroutine ---
    public IEnumerator DarkenScreenRoutine(float duration)
    {
        if (panelImage == null) 
        {
            Debug.LogError("HATA: 'panelImage' alanı " + gameObject.name + " üzerinde atanmamış!");
            yield break; // Coroutine'i durdur.
        }
        
        Debug.Log("--- Ekran karartılıyor (Color.alpha metodu)... ---");

        // Mevcut rengi al
        Color color = panelImage.color;
        
        // Alfa değerini 204 yap (0-1 aralığında 0.8'e denk gelir)
        color.a = 204f / 255f;
        
        // Yeni rengi Image'a ata
        panelImage.color = color;

        // Belirtilen süre kadar bekle
        yield return new WaitForSeconds(duration);

        // Alfa değerini tekrar 0 yap
        color.a = 0f;
        
        // Rengi tekrar ata
        panelImage.color = color;

        Debug.Log("--- Ekran normale döndü. ---");
    }
}