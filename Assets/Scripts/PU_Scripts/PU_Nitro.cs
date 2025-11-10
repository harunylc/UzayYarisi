// using UnityEngine;
//
// public class PU_Nitro : MonoBehaviour
// {
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") || other.CompareTag("Player2"))
//         {
//             CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
//             if (carHandler != null)
//             {
//                 // Arabanın "cüzdanına" Nitro güçlendirmesini ekle
//                 carHandler.GivePowerUp("Nitro");
//                 // Ve kendini yok et
//                 Destroy(gameObject);
//             }
//         }
//     }
// }
using UnityEngine;
using System.Collections;

public class PU_NitroID : MonoBehaviour
{

    // --- BU FONKSİYONU PLAYER 1 ÇAĞIRACAK ---
    public IEnumerator BoostPlayer1NitroRecharge(float duration)
    {
        // Player 1'in arabasını ve sürüş script'ini bul
        DriveMyCar player1DriveScript = FindObjectOfType<DriveMyCar>();

        if (player1DriveScript != null)
        {
            // Orijinal değeri kaydet
            float originalRate = player1DriveScript.nitroRechargeRate;
            // Yeni, %50 artırılmış değeri hesapla
            float boostedRate = originalRate * 1.5f;

            // Değeri güncelle
            player1DriveScript.nitroRechargeRate = boostedRate;
            Debug.Log($"Player 1 Nitro dolum hızı {boostedRate} oldu.");

            // Belirtilen süre kadar bekle
            yield return new WaitForSeconds(duration);

            // Süre bitince, değeri eski haline geri getir
            player1DriveScript.nitroRechargeRate = originalRate;
            Debug.Log($"Player 1 Nitro dolum hızı normale döndü: {originalRate}.");
        }
    }

    // --- BU FONKSİYONU PLAYER 2 ÇAĞIRACAK ---
    public IEnumerator BoostPlayer2NitroRecharge(float duration)
    {
        // Player 2'nin arabasını ve sürüş script'ini bul
        DriveMyCar_Player2 player2DriveScript = FindObjectOfType<DriveMyCar_Player2>();

        if (player2DriveScript != null)
        {
            // Orijinal değeri kaydet
            float originalRate = player2DriveScript.nitroRechargeRate;
            // Yeni, %50 artırılmış değeri hesapla
            float boostedRate = originalRate * 1.5f;

            // Değeri güncelle
            player2DriveScript.nitroRechargeRate = boostedRate;
            Debug.Log($"Player 2 Nitro dolum hızı {boostedRate} oldu.");

            // Belirtilen süre kadar bekle
            yield return new WaitForSeconds(duration);

            // Süre bitince, değeri eski haline geri getir
            player2DriveScript.nitroRechargeRate = originalRate;
            Debug.Log($"Player 2 Nitro dolum hızı normale döndü: {originalRate}.");
        }
    }
}