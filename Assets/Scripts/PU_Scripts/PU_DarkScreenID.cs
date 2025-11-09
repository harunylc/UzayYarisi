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