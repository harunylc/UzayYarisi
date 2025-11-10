// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
//
// public class PU_DarkScreen : MonoBehaviour
// {
//     public bool isPowerUpObject;
//     public Image darkPanelImage;
//     private Coroutine activeCoroutine;
//
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (!isPowerUpObject) return;
//
//         if (other.CompareTag("Player") || other.CompareTag("Player2"))
//         {
//             CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
//             if (carHandler != null)
//             {
//                 // Artık güçlendirmenin adını bir string olarak veriyoruz.
//                 carHandler.GivePowerUp("DarkScreen");
//                 Destroy(gameObject);
//             }
//         }
//     }
//
//     public void Activate(float duration)
//     {
//         if (activeCoroutine != null)
//         {
//             StopCoroutine(activeCoroutine);
//         }
//         activeCoroutine = StartCoroutine(DarkenScreenRoutine(duration));
//     }
//
//     private IEnumerator DarkenScreenRoutine(float duration)
//     {
//         if (darkPanelImage == null) yield break;
//
//         Color color = darkPanelImage.color;
//         color.a = 204f / 255f;
//         darkPanelImage.color = color;
//         
//         yield return new WaitForSeconds(duration);
//
//         color.a = 0f;
//         darkPanelImage.color = color;
//         
//         activeCoroutine = null;
//     }
// }
using UnityEngine;
using System.Collections;

public class PU_DarkScreenID : MonoBehaviour
{
    // Bu script'i PU_DarkScreen prefab'ının üzerine ekleyeceksiniz.

    [Header("Hedef Paneller")]
    [Tooltip("CameraP1'in altındaki Panel objesini buraya sürükleyin.")]
    public GameObject player1_DarkPanel;
    
    [Tooltip("CameraP2'nin altındaki Panel objesini buraya sürükleyin.")]
    public GameObject player2_DarkPanel;

    // --- BU FONKSİYONU PLAYER 1 ÇAĞIRACAK (P2'nin ekranını karartmak için) ---
    public IEnumerator DarkenPlayer2Screen(float duration)
    {
        if (player2_DarkPanel != null)
        {
            player2_DarkPanel.SetActive(true);
            yield return new WaitForSeconds(duration);
            player2_DarkPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Player 2'nin Dark Panel'i PU_DarkScreenID script'ine atanmamış!");
        }
    }

    // --- BU FONKSİYONU PLAYER 2 ÇAĞIRACAK (P1'in ekranını karartmak için) ---
    public IEnumerator DarkenPlayer1Screen(float duration)
    {
        if (player1_DarkPanel != null)
        {
            player1_DarkPanel.SetActive(true);
            yield return new WaitForSeconds(duration);
            player1_DarkPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("Player 1'in Dark Panel'i PU_DarkScreenID script'ine atanmamış!");
        }
    }
}