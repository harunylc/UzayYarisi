using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PU_DarkScreen : MonoBehaviour
{
    [Header("Rol Ayarları")]
    public bool isPowerUpObject;

    [Header("Panel Ayarları (Eğer Panel ise)")]
    public Image panelImage;

    // --- ROL 1: Toplanabilir Obje Olarak Çalışma ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPowerUpObject) return;

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
    // Bu fonksiyonun tek görevi, bir plan (IEnumerator) sunmaktır.
    // Dışarıdan çağrılabilmesi için "public" olmalıdır.
    public IEnumerator DarkenScreenRoutine(float duration)
    {
        if (panelImage == null) 
        {
            Debug.LogError("Panel Image, " + gameObject.name + " üzerinde atanmamış!");
            yield break; // Hata varsa Coroutine'i durdur.
        }
        
        // Önce paneli aktif et.
        panelImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        // Süre bitince paneli tekrar kapat.
        panelImage.gameObject.SetActive(false);
    }
}