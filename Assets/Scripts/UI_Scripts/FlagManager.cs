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
            return;
        }

        if (other.CompareTag("Player"))
        {
            GameRoundManager.Instance.PlayerReachedFlag(1);
        }
        else if (other.CompareTag("Player2"))
        {
            GameRoundManager.Instance.PlayerReachedFlag(2);
        }
    }
}
