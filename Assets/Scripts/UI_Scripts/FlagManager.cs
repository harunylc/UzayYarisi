// using UnityEngine;
//
// public class FlagManager : MonoBehaviour
// {
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (GameRoundManager.Instance == null)
//         {
//             Debug.LogError("GameRoundManager bulunamadı!");
//             return;
//         }
//
//         if (other.CompareTag("Player"))
//         {
//             GameRoundManager.Instance.PlayerReachedFlag(1);
//         }
//         else if (other.CompareTag("Player2"))
//         {
//             GameRoundManager.Instance.PlayerReachedFlag(2);
//         }
//     }
// }
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameRoundManager.Instance == null)
        {
            Debug.LogError("[FlagManager] GameRoundManager bulunamadı!");
            return;
        }

        // Oyuncu 1 tag kontrolü
        if (other.CompareTag("Player"))
        {
            Debug.Log("[FlagManager] Player (1. oyuncu) bayrağa ulaştı!");
            GameRoundManager.Instance.PlayerReachedFlag(1);
        }
        // Oyuncu 2 tag kontrolü
        else if (other.CompareTag("Player2"))
        {
            Debug.Log("[FlagManager] Player2 (2. oyuncu) bayrağa ulaştı!");
            GameRoundManager.Instance.PlayerReachedFlag(2);
        }
        // Eğer farklı bir obje çarparsa logla (örnek: NPC, item vs)
        else
        {
            Debug.Log($"[FlagManager] '{other.tag}' etiketi tanınmadı — çarpışma yok sayıldı.");
        }
    }
}
