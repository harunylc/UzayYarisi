using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PU_DarkScreen : MonoBehaviour
{
    // [Header("Rol Ayarları")]
    // public bool isCollectableObject;

    [Header("Panel Ayarları (Eğer Panel ise)")]
    public Image panelImage;

    // --- ROL 1: Toplanabilir Obje Olarak Çalışma ---
    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (!isCollectableObject) return; // Eğer panel ise, bu kodu çalıştırma.
    //
    //     if (other.CompareTag("Player") || other.CompareTag("Player2"))
    //     {
    //         CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
    //         if (carHandler != null)
    //         {
    //             carHandler.GivePowerUp("DarkScreen");
    //             Destroy(gameObject);
    //         }
    //     }
    // }

    public IEnumerator DarkenScreenRoutine(float duration)
    {
        if (panelImage == null) 
        {
            Debug.LogError("HATA: 'panelImage' alanı " + gameObject.name + " üzerinde atanmamış!");
            yield break; 
        }
        
        Color color = panelImage.color;

        color.a = 242.25f / 255f;

        panelImage.color = color;

        yield return new WaitForSeconds(duration);

        color.a = 0f;

        panelImage.color = color;
    }
}