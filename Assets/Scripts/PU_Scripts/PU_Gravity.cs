// using UnityEngine;
//
// public class PU_Gravity : MonoBehaviour
// {
//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player") || other.CompareTag("Player2"))
//         {
//             CarPowerUpHandler carHandler = other.GetComponent<CarPowerUpHandler>();
//             if (carHandler != null)
//             {
//                 // Artık güçlendirmenin adını bir string olarak veriyoruz.
//                 carHandler.GivePowerUp("Gravity");
//                 Destroy(gameObject);
//             }
//         }
//     }
// }
using UnityEngine;
using System.Collections;

public class PU_GravityID : MonoBehaviour
{
    /// <summary>
    /// Player 1 için kütle azaltma fonksiyonu.
    /// </summary>
    public void ApplyEffectToPlayer1()
    {
        GameObject player1 = GameObject.FindWithTag("Player");
        if (player1 != null)
        {
            Rigidbody2D rb = player1.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.mass *= 0.5f;
                Debug.Log("Player 1'in kütlesi %50 azaltıldı. Yeni kütle: " + rb.mass);
            }
        }
    }

    /// <summary>
    /// Player 2 için kütle azaltma fonksiyonu.
    /// </summary>
    public void ApplyEffectToPlayer2()
    {
        GameObject player2 = GameObject.FindWithTag("Player2");
        if (player2 != null)
        {
            Rigidbody2D rb = player2.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.mass *= 0.5f;
                Debug.Log("Player 2'nin kütlesi %50 azaltıldı. Yeni kütle: " + rb.mass);
            }
        }
    }
}